using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Client
{
    public class RabbitSender
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string ExchangeName = "Sample.Exchange";
        private const bool IsDurable = true;
        
        private const string VirtualHost = "";
        private int c_port = 0;

        private ConnectionFactory c_connectionFactory;
        private IConnection c_connection;
        private IModel c_model;


        public RabbitSender()
        {
            this.DisplaySettings();
            this.SetupRabbitMq();
        }


        private void DisplaySettings()
        {
            Console.WriteLine("Host: {0}", HostName);
            Console.WriteLine("Username: {0}", UserName);
            Console.WriteLine("Password: {0}", Password);
            Console.WriteLine("ExchangeName: {0}", ExchangeName);
            Console.WriteLine("VirtualHost: {0}", VirtualHost);
            Console.WriteLine("Port: {0}", c_port);
            Console.WriteLine("Is Durable: {0}", IsDurable);
        }
        

        private void SetupRabbitMq()
        {
            this.c_connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            if (string.IsNullOrEmpty(VirtualHost) == false)
                this.c_connectionFactory.VirtualHost = VirtualHost;

            if (this.c_port > 0)
                this.c_connectionFactory.Port = this.c_port;

            this.c_connection = this.c_connectionFactory.CreateConnection();
            this.c_model = this.c_connection.CreateModel();
        }

        public void Send(
            string message)
        {
            //Setup properties
            var properties = c_model.CreateBasicProperties();
            properties.SetPersistent(true);

            //Serialize
            byte[] messageBuffer = Encoding.Default.GetBytes(message);

            //Send message
            c_model.BasicPublish(ExchangeName, "", properties, messageBuffer);
        }

        
        public void Dispose()
        {
            if (this.c_connection != null)
                this.c_connection.Close();
            
            if (this.c_model != null && this.c_model.IsOpen)
                this.c_model.Abort();

            this.c_connectionFactory = null;

            GC.SuppressFinalize(this);
        }
    }
}
