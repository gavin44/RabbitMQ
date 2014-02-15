using System;
using System.Collections.Generic;


namespace Server3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting RabbitMQ queue processor");
            Console.WriteLine();
            
            var _queueProcessor = new RabbitConsumer(){Enabled = true};
            _queueProcessor.Start();

            Console.ReadLine();
        }
    }
}
