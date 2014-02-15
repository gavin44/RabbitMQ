using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WinClient
{
    public partial class Form1 : Form
    {
        private int c_messageCount;


        public Form1()
        {
            InitializeComponent();
        }


        private void sendButton_Click(
            object sender, 
            EventArgs e)
        {
            var _topics = new List<string>();
            var _messageSender = new RabbitSender();

            _topics.Add(GetComboItem(customerTypeComboBox));
            _topics.Add(GetComboItem(orderSizeComboBox));
            _topics.Add(GetComboItem(productComboBox));

            var _message = string.Format("Message: {0}", c_messageCount);            

            var _routingkey = _messageSender.Send(_message, _topics);

            MessageBox.Show(string.Format("Sending Message - {0}, Routing Key - {1}", _message, _routingkey), "Message sent");

            this.c_messageCount++;
        }


        private static string GetComboItem(
            ComboBox comboBox)
        {
            if (string.IsNullOrEmpty(comboBox.Text))
                return string.Empty;

            return comboBox.Text;
        }
    }
}
