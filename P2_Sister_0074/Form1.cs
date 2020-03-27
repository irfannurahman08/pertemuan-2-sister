using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace P2_Sister_0074
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string received;
        public string textSend;

        public Form1()
        {
            InitializeComponent();

            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    txtIpC.Text = address.ToString();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonStarServer_Click(object sender, EventArgs e)
        {

            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(txtPortS.Text));
            listener.Start();
            client = listener.AcceptTcpClient();
            STR = new StreamReader(client.GetStream());
            STW = new StreamWriter(client.GetStream());
            STW.AutoFlush = true;

            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.WorkerSupportsCancellation = true;

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            IPEndPoint IP_END = new IPEndPoint(IPAddress.Parse(txtIpC.Text), 
                int.Parse(txtPortC.Text));

            try
            {
                client.Connect(IP_END);
                if (client.Connected)
                {
                    txtChat.AppendText("Connected to Server " + "\n");
                    STW = new StreamWriter(client.GetStream());
                    STR = new StreamReader(client.GetStream());
                    STW.AutoFlush = true;

                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker2.WorkerSupportsCancellation = true;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message.ToString());
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (txtPesan.Text != "")
            {
                textSend = txtPesan.Text;
                backgroundWorker2.RunWorkerAsync();

                txtPesan.Text = "";
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    received = STR.ReadLine();
                    this.txtChat.Invoke(new MethodInvoker(delegate()
                    { txtChat.AppendText("Anda : " + received + "\n"); }));
                    received = "";

                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (client.Connected)
            {
                STW.WriteLine(textSend);
                this.txtChat.Invoke(new MethodInvoker(delegate()
                { txtChat.AppendText("Saya : " + textSend + "\n"); }));
            }
            else
            {
                MessageBox.Show("Send faild!!");
            }

            backgroundWorker2.CancelAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Visible = false;
        }
    }
}
