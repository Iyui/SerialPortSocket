using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
namespace SerialPortTCP.SerialPorCommunication
{
    public partial class SerialPortReceive : Form
    {
        public SerialPortReceive()
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
            textBox1.Invoke(receiveCallBack, $"接收数据{temp}\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!sp.IsOpen)
            {
                try
                {
                    sp.PortName = comboBox1.Text;
                    sp.Open();
                    textBox1.AppendText($"打开串口{sp.PortName}:{sp.BaudRate}成功");
                }
                catch { }
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
        }

        private void ReceiveMsg(string strMsg)
        {
            this.textBox1.AppendText(strMsg + " \r \n");
        }
        private delegate void ReceiveMsgCallBack(string strReceive);
        //声明
        private ReceiveMsgCallBack receiveCallBack;
    }
}
