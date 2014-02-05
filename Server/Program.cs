using System;


namespace Server
{
    class Program
    {
        static void Main(
            string[] args)
        {
            Console.WriteLine("Starting RabbitMQ queue processor");
            Console.WriteLine();
            Console.WriteLine();

            var _queueProcessor = new RabbitConsumer() { Enabled = true };
            _queueProcessor.Start();
            Console.ReadLine();
        }
    }
}
