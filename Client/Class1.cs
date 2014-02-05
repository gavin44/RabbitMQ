using System;
using System.Text;
using RabbitMQ.Client;


namespace Client
{
    public class Class1
    {        
        public static void Main(
            string[] args)
        {
            Console.WriteLine("Starting RabbitMQ Message Sender");
            Console.WriteLine();

            var _messageCount = 0;
            var _rabbitSender = new RabbitSender();

            Console.WriteLine("Press enter key to send a message");
            while (true)
            {
                var _key = Console.ReadKey();
                if (_key.Key == ConsoleKey.Q)
                    break;

                if (_key.Key == ConsoleKey.Enter)
                {
                    var _message = string.Format("Message: {0}", _messageCount);
                    Console.WriteLine(string.Format("Sending - {0}", _message));

                    _rabbitSender.Send(_message);
                    _messageCount++;
                }
            }

            Console.ReadLine();
        }
    }
}
