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

namespace ARMSocketClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static int port = 8005; // порт сервера
        //static string address = "127.0.0.1"; // адрес сервера
        static string address = "192.168.13.7"; // адрес сервера
        Socket socket;
        IPEndPoint ipPoint;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = textBox1.Text;
            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);

            // получаем ответ
            data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket.Available > 0);
            label3.Text = builder.ToString();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
