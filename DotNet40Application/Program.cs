using System;
using System.Threading;
using DotNet40Library;

namespace DotNet40Application
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Usage: DotNet40Application.exe http://path/to/queue");
                    return;
                }

                var waitHandle = new AutoResetEvent(false);

                var messageSender = new MessageSender(args[0]);
                messageSender.SendAsync("Hello, world!").ContinueWith(task =>
                {
                    try
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine("Error: {0}", task.Exception);
                        }
                        else if (task.IsCompleted)
                        {
                            Console.WriteLine("Sent!");
                        }
                    }
                    finally
                    {
                        waitHandle.Set();
                    }
                });

                if (!waitHandle.WaitOne(10000))
                {
                    Console.WriteLine("Timeout!");
                }
            }
            finally
            {
                Console.Write("Press any key to exit.");
                Console.ReadKey(true);
            }
        }
    }
}
