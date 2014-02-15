using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;


namespace WinClient
{
    public class RabbitSender : IDisposable
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string ExchangeName = "Module2.Sample5.Exchange";
        private const bool IsDurable = true;
        private const string VirtualHost = "";
        private int Port = 0;

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
            Console.WriteLine("Port: {0}", Port);
            Console.WriteLine("Is Durable: {0}", IsDurable);
        }
        

        private void SetupRabbitMq()
        {
            c_connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            if (string.IsNullOrEmpty(VirtualHost) == false)
                this.c_connectionFactory.VirtualHost = VirtualHost;
            if (Port > 0)
                this.c_connectionFactory.Port = Port;

            this.c_connection = c_connectionFactory.CreateConnection();
            this.c_model = this.c_connection.CreateModel();
        }

        public string Send(
            string message, 
            List<string> topics)
        {
            //Setup properties
            var properties = c_model.CreateBasicProperties();
            properties.SetPersistent(true);

            //Serialize
            byte[] messageBuffer = Encoding.Default.GetBytes(message);

            //Create routing key from topics
            var routingKey = topics.Aggregate(string.Empty, (current, key) => current + (key.ToLower() + "."));
            if (routingKey.Length > 1)
                routingKey = routingKey.Remove(routingKey.Length - 1, 1);

            Console.WriteLine("Routing key from topics: {0}", routingKey);

            //Send message
            c_model.BasicPublish(ExchangeName, routingKey, properties, messageBuffer);

            return routingKey;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (c_connection != null)
                c_connection.Close();
            
            if (c_model != null && c_model.IsOpen)
                c_model.Abort();

            c_connectionFactory = null;

            GC.SuppressFinalize(this);
        }
    }
}
