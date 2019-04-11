using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.IO.Ports;
namespace SerialPortTCP
{
    public partial class TCPServer : Form
    {
        public TCPServer()
        {
            InitializeComponent();
            sp.DataReceived += Sp_DataReceived;
            this.receiveCallBack = new ReceiveMsgCallBack(ReceiveMsg);
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                sp.PortName = ports[0];
            }
            comboBox1.Items.Add(sp.PortName);
            comboBox1.Text = sp.PortName;
        }

        private bool _isDMYTcp = false;

        //定义回调:解决跨线程访问问题
        private delegate void SetTextValueCallBack(string strValue);
        //定义接收客户端发送消息的回调
        private delegate void ReceiveMsgCallBack(string strReceive);

        private delegate void IPCallBack(string strReceive);

        //声明回调
        private SetTextValueCallBack setCallBack;
        //声明
        private ReceiveMsgCallBack receiveCallBack;

        private IPCallBack ipCallBack;
        //定义回调：给ComboBox控件添加元素
        private delegate void SetCmbCallBack(string strItem);
        //声明
        private SetCmbCallBack setCmbCallBack;
        //定义发送文件的回调
        private delegate void SendFileCallBack(byte[] bf);
        //声明
        private SendFileCallBack sendCallBack;

        //用于通信的Socket
        Socket socketSend;
        //用于监听的SOCKET
        Socket socketWatch;

        //将远程连接的客户端的IP地址和Socket存入集合中
        static Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();

        //创建监听连接的线程
        Thread AcceptSocketThread;
        //接收客户端发送消息的线程
        Thread threadReceive;

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Start_Click(object sender, EventArgs e)
        {
            //当点击开始监听的时候 在服务器端创建一个负责监听IP地址和端口号的Socket
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //获取ip地址
            IPAddress ip = IPAddress.Parse(this.txt_IP.Text.Trim());
            //创建端口号
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(this.txt_Port.Text.Trim()));
            //绑定IP地址和端口号
            socketWatch.Bind(point);
            this.txt_Log.AppendText("监听成功" + " \r \n");
            //开始监听:设置最大可以同时连接多少个请求
            socketWatch.Listen(20);

            //实例化回调
            setCallBack = new SetTextValueCallBack(SetTextValue);
            receiveCallBack = new ReceiveMsgCallBack(ReceiveMsg);
            ipCallBack = new IPCallBack(IpChangeValue);
            setCmbCallBack = new SetCmbCallBack(AddCmbItem);
            sendCallBack = new SendFileCallBack(SendFile);

            //创建线程
            AcceptSocketThread = new Thread(new ParameterizedThreadStart(StartListen));
            AcceptSocketThread.IsBackground = true;
            AcceptSocketThread.Start(socketWatch);
        }
      
        /// <summary>
        /// 等待客户端的连接，并且创建与之通信用的Socket
        /// </summary>
        /// <param name="obj"></param>
        private void StartListen(object obj)
        {
            Socket socketWatch = obj as Socket;
            while (true)
            {
                //等待客户端的连接，并且创建一个用于通信的Socket
                socketSend = socketWatch.Accept();
                //获取远程主机的ip地址和端口号
                string strIp = socketSend.RemoteEndPoint.ToString();
                dicSocket.Add(strIp, socketSend);
                this.cmb_Socket.Invoke(setCmbCallBack, strIp);
                string strMsg = "远程主机：" + socketSend.RemoteEndPoint + "连接成功";
                
                //使用回调
                txt_Log.Invoke(setCallBack, strMsg);

                //定义接收客户端消息的线程
                threadReceive = new Thread(new ParameterizedThreadStart(Receive));
                threadReceive.IsBackground = true;
                threadReceive.Start(socketSend);
                if(!_isDMYTcp)
                    Test(strIp);
                Send();
                Console.Read();

            }
        }



        /// <summary>
        /// 服务器端不停的接收客户端发送的消息
        /// </summary>
        /// <param name="obj"></param>
        private void Receive(object obj)
        {
            Socket socketSend = obj as Socket;
            while (true)
            {
                try
                {
                    //客户端连接成功后，服务器接收客户端发送的消息
                    byte[] buffer = new byte[36];
                    //实际接收到的有效字节数
                    int count = socketSend.Receive(buffer);
                    if (count == 0)//count 表示客户端关闭，要退出循环
                    {
                        break;
                    }
                    else
                    {
                        if (sp.IsOpen)
                            sp.Write(buffer, 0, buffer.Length);
                        var s = "";
                        foreach (var c in buffer)
                            s += c.ToString("X2");
                        string str = Encoding.ASCII.GetString(buffer, 0, count);
                        string strReceiveMsg = $"接收：{s}发送的消息：{ str}";
                        if (!_isDMYTcp)
                        {
                            if (s.ToLower().IndexOf("ff0807") >= 0)
                            {
                                _isDMYTcp = true;
                                ip = socketSend.RemoteEndPoint.ToString();
                                ChangeClientIP();
                                txt_Log.Invoke(receiveCallBack, $"\n测试成功:当前客户端IP:{ip}");
                            }
                        }
                            txt_Log.Invoke(receiveCallBack, strReceiveMsg);
                    }
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 回调委托需要执行的方法
        /// </summary>
        /// <param name="strValue"></param>
        private void SetTextValue(string strValue)
        {
            this.txt_Log.AppendText(strValue + " \r \n");
        }


        private void ReceiveMsg(string strMsg)
        {
            this.txt_Log.AppendText(strMsg + " \r \n");
        }

        private void IpChangeValue(string strMsg)
        {
            lClientIP.Text = strMsg;
        }

        private void AddCmbItem(string strItem)
        {
            this.cmb_Socket.Items.Add(strItem);
        }

        /// <summary>
        /// 服务器给客户端发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                txt_Log.AppendText("测试链接");
                var mess = new byte[] { 0xff, 0x08, 0x07 };
                dicSocket[ip].Send(mess);
            }
            catch (Exception ex)
            {
                txt_Log.AppendText("给客户端发送消息出错:" + ex.Message);
            }
            //socketSend.Send(buffer);
        }

        private void Test(string ip)
        {
            try
            {
                var mess = new byte[] { 0xff, 0x08, 0x07 };
                txt_Log.Invoke(receiveCallBack, "测试链接");
                int i = 0;
                while(i++<5 && !_isDMYTcp)
                    dicSocket[ip].Send(mess);
            }
            catch (Exception ex)
            {
                txt_Log.Invoke(receiveCallBack, "给客户端发送消息出错:" + ex.Message);
                //MessageBox.Show("给客户端发送消息出错:" );
            }
            //socketSend.Send(buffer);
        }

        private void Send()
        {
            while (true)
            {
                if (ByteQueue.Any())
                {
                    try
                    {
                        string strMsg = this.txt_Msg.Text.Trim();
                        byte[] buffer = Encoding.Default.GetBytes(strMsg);
                        List<byte> list = new List<byte>();
                        list.Add(0);
                        list.AddRange(buffer);
                        //将泛型集合转换为数组
                        byte[] newBuffer = list.ToArray();
                        //获得用户选择的IP地址
                        
                        //var mess = new byte[] { 0xff, 0x08, 0x07 };
                        //= @"FF 08 07";
                        var mess = DivisionHEX(txt_Msg.Text).TrimEnd();

                        //var strings = mess.Split(' ');
                        //var bytes = Array.ConvertAll(strings, input => Convert.ToByte(input, 16));

                        dicSocket[ip].Send(ByteQueue.Dequeue());
                        txt_Log.Invoke(receiveCallBack, $"发送成功");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("给客户端发送消息出错:" + ex.Message);
                    }
                }
                else
                    Thread.Sleep(1);
            }
        }

        SerialPort sp = new SerialPort();
        //public static byte[] transmitByte = null;
        public static Queue<byte[]> ByteQueue = new Queue<byte[]>();
        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int len = sp.BytesToRead;
            byte[] data = new byte[len];
            sp.Read(data, 0, data.Length);
            ByteQueue.Enqueue(data);
            string temp = "";
            for (int i = 0; i < data.Length; i++)
            {
                temp += data[i].ToString("X2") + " ";
            }
            txt_Log.Invoke(receiveCallBack, $"接收数据{temp}\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!sp.IsOpen)
            {
                try
                {
                    sp.PortName = comboBox1.Text;
                    sp.Open();
                    txt_Log.AppendText($"打开串口{sp.PortName}:{sp.BaudRate}成功");
                }
                catch { }
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
        }

        //声明

        /// <summary>
        /// 选择要发送的文件
        /// </summary>
        /// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btn_Select_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog dia = new OpenFileDialog();
        //    //设置初始目录
        //    dia.InitialDirectory = @"";
        //    dia.Title = "请选择要发送的文件";
        //    //过滤文件类型
        //    dia.Filter = "所有文件|*.*";
        //    dia.ShowDialog();
        //    //将选择的文件的全路径赋值给文本框
        //    this.txt_FilePath.Text = dia.FileName;
        //}

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btn_SendFile_Click(object sender, EventArgs e)
        //{
        //    List<byte> list = new List<byte>();
        //    //获取要发送的文件的路径
        //    string strPath = this.txt_FilePath.Text.Trim();
        //    using (FileStream sw = new FileStream(strPath, FileMode.Open, FileAccess.Read))
        //    {
        //        byte[] buffer = new byte[2048];
        //        int r = sw.Read(buffer, 0, buffer.Length);
        //        list.Add(1);
        //        list.AddRange(buffer);

        //        byte[] newBuffer = list.ToArray();
        //        //发送
        //        //dicSocket[cmb_Socket.SelectedItem.ToString()].Send(newBuffer, 0, r+1, SocketFlags.None);
        //        btn_SendFile.Invoke(sendCallBack, newBuffer);


        //    }

        //}

        private void SendFile(byte[] sendBuffer)
        {

            try
            {
                dicSocket[cmb_Socket.SelectedItem.ToString()].Send(sendBuffer, SocketFlags.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送文件出错:" + ex.Message);
            }
        }

        private void btn_Shock_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[1] { 2 };
            dicSocket[cmb_Socket.SelectedItem.ToString()].Send(buffer);
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_StopListen_Click(object sender, EventArgs e)
        {
            _isDMYTcp = false;
            cmb_Socket.Items.Clear();
            socketWatch.Close();
            socketSend.Close();
            //终止线程
            AcceptSocketThread.Abort();
            threadReceive.Abort();
        }

        private void StringToHex()
        {
            string sPortInfo = txt_Msg.Text;
            DivisionHEX(txt_Msg.Text);
        }

        /// <summary>
        /// 将一段字符串每一个字节存入数组中
        /// </summary>
        /// <param name="DivisionText"></param>
        /// <returns></returns>
        private string DivisionHEX(string DivisionText)
        {
            string txt = DivisionText;
            string x = "";
            for (int i = 0; i < txt.Length; i++)
            {
                x = x + txt[i].ToString() + txt[++i].ToString();
                x += " ";
            }
            return x;
        }

        private void TCPServer_Load(object sender, EventArgs e)
        {
            txt_IP.Text = "192.168.1.128";
            txt_Port.Text = "9000";
        }
        string ip = "";

        private void cmb_Socket_SelectedIndexChanged(object sender, EventArgs e)
        {
            ip = this.cmb_Socket.SelectedItem?.ToString();
            ChangeClientIP();
        }

        private void ChangeClientIP()
        {
            lClientIP.Invoke(ipCallBack, ip);
        }
    }
}

