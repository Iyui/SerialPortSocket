namespace SerialPortTCP
{
    partial class TCPServer
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.txt_Port = new System.Windows.Forms.TextBox();
            this.txt_Msg = new System.Windows.Forms.TextBox();
            this.cmb_Socket = new System.Windows.Forms.ListBox();
            this.txt_Log = new System.Windows.Forms.TextBox();
            this.btn_Start = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(44, 65);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(100, 21);
            this.txt_IP.TabIndex = 0;
            // 
            // txt_Port
            // 
            this.txt_Port.Location = new System.Drawing.Point(193, 65);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Size = new System.Drawing.Size(100, 21);
            this.txt_Port.TabIndex = 1;
            // 
            // txt_Msg
            // 
            this.txt_Msg.Location = new System.Drawing.Point(50, 331);
            this.txt_Msg.Name = "txt_Msg";
            this.txt_Msg.Size = new System.Drawing.Size(280, 21);
            this.txt_Msg.TabIndex = 2;
            // 
            // cmb_Socket
            // 
            this.cmb_Socket.FormattingEnabled = true;
            this.cmb_Socket.ItemHeight = 12;
            this.cmb_Socket.Location = new System.Drawing.Point(44, 204);
            this.cmb_Socket.Name = "cmb_Socket";
            this.cmb_Socket.Size = new System.Drawing.Size(280, 88);
            this.cmb_Socket.TabIndex = 3;
            this.cmb_Socket.SelectedIndexChanged += new System.EventHandler(this.cmb_Socket_SelectedIndexChanged);
            // 
            // txt_Log
            // 
            this.txt_Log.Location = new System.Drawing.Point(458, 4);
            this.txt_Log.Multiline = true;
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_Log.Size = new System.Drawing.Size(330, 434);
            this.txt_Log.TabIndex = 5;
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(50, 110);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_Start.TabIndex = 6;
            this.btn_Start.Text = "监听";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(353, 329);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(202, 110);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "停止监听";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btn_StopListen_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(202, 155);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "打开串口";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(44, 155);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            // 
            // TCPServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.txt_Log);
            this.Controls.Add(this.cmb_Socket);
            this.Controls.Add(this.txt_Msg);
            this.Controls.Add(this.txt_Port);
            this.Controls.Add(this.txt_IP);
            this.Name = "TCPServer";
            this.Text = "FrmServer";
            this.Load += new System.EventHandler(this.TCPServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_IP;
        private System.Windows.Forms.TextBox txt_Port;
        private System.Windows.Forms.TextBox txt_Msg;
        private System.Windows.Forms.ListBox cmb_Socket;
        private System.Windows.Forms.TextBox txt_Log;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

