using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace P2_Sister_0074
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string[] port = SerialPort.GetPortNames();
            cboPort.Items.AddRange(port);
            cboPort.SelectedIndex = 0;
            buttonClose.Enabled = false;

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            buttonOpen.Enabled=false;
            buttonClose.Enabled=true;
            try
            {
                serialPort1.PortName = cboPort.Text;
                serialPort1.Open();
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message, 
                    "Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            buttonOpen.Enabled = true;
            buttonClose.Enabled = false;
            try
            {
                serialPort1.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, 
                    "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine(txtSend.Text + Environment.NewLine);
                    txtSend.Clear();
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Mesage", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonReceive_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    txtReceive.Text = serialPort1.ReadExisting();
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Message", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Form2_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();

        }
    }
}
