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
            this.c_connectionFactory = new ConnectionFactory
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
            var _properties = this.c_model.CreateBasicProperties();
            _properties.SetPersistent(true);

            //Serialize
            byte[] _messageBuffer = Encoding.Default.GetBytes(message);

            //Create routing key from topics
            var _routingKey = topics.Aggregate(string.Empty, (current, key) => current + (key.ToLower() + "."));
            if (_routingKey.Length > 1)
                _routingKey = _routingKey.Remove(_routingKey.Length - 1, 1);

            Console.WriteLine("Routing key from topics: {0}", _routingKey);

            //Send message
            this.c_model.BasicPublish(ExchangeName, _routingKey, _properties, _messageBuffer);

            return _routingKey;
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
