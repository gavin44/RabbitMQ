using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;


namespace Server
{
    public class RabbitConsumer
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "SampleQueue3";
        private const bool IsDurable = true;
        
        private const string VirtualHost = "";
        private int c_port = 0;

        public delegate void OnReceiveMessage(string message);

        public bool Enabled { get; set; }

        private ConnectionFactory c_connectionFactory;
        private IConnection c_connection;
        private IModel c_model;
        private Subscription _subscription;


        /// <summary>
        /// Ctor with a key to lookup the configuration
        /// </summary>
        public RabbitConsumer()
        {
            DisplaySettings();
            this.c_connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            if (string.IsNullOrEmpty(VirtualHost) == false)
                this.c_connectionFactory.VirtualHost = VirtualHost;
            if (c_port > 0)
                this.c_connectionFactory.Port = this.c_port;

            this.c_connection = this.c_connectionFactory.CreateConnection();
            this.c_model = this.c_connection.CreateModel();
            this.c_model.BasicQos(0, 1, false);
        }
        

        private void DisplaySettings()
        {
            Console.WriteLine("Host: {0}", HostName);
            Console.WriteLine("Username: {0}", UserName);
            Console.WriteLine("Password: {0}", Password);
            Console.WriteLine("QueueName: {0}", QueueName);
            Console.WriteLine("VirtualHost: {0}", VirtualHost);
            Console.WriteLine("Port: {0}", c_port);
            Console.WriteLine("Is Durable: {0}", IsDurable);
        }
        

        public void Start()
        {
            _subscription = new Subscription(c_model, QueueName, false);

            var consumer = new ConsumeDelegate(Poll);
            consumer.Invoke();
        }


        private delegate void ConsumeDelegate();


        private void Poll()
        {
            while (Enabled)
            {
                //Get next message
                var _deliveryArgs = _subscription.Next();
                //Deserialize message
                var _message = Encoding.Default.GetString(_deliveryArgs.Body);

                //Handle Message
                Console.WriteLine("Message Recieved - {0}", _message);

                //Acknowledge message is processed
                _subscription.Ack(_deliveryArgs);
            }
        }
        

        public void Dispose()
        {
            if (this.c_model != null)
                this.c_model.Dispose();
            if (this.c_connection != null)
                this.c_connection.Dispose();

            this.c_connectionFactory = null;

            GC.SuppressFinalize(this);
        }
    }
}
