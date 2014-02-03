using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;


namespace Client
{
    public class Class1
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "SampleQueue";
        private const string ExchangeName = "";


        public static void Main(
            string[] args)
        {
            Console.WriteLine("Starting RabbitMQ Message Sender");
            Console.WriteLine();
            Console.WriteLine();

            var connectionFactory = new ConnectionFactory { HostName = HostName, UserName = UserName, Password = Password };
            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();

            var properties = model.CreateBasicProperties();
            properties.SetPersistent(false);

            //Serialize
            byte[] messageBuffer = Encoding.Default.GetBytes("this is my message");

            //Send message
            model.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);

            Console.WriteLine("Message sent");
            Console.ReadLine();
        }
    }
}
