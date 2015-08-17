using System;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace TestApi
{
    class Program
    {
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        static void Main()
        {
            Console.WriteLine("Press CTRL+C to exit");

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                ExitEvent.Set();
            };

            using (WebApp.Start<Startup>("http://+:9000"))
            {
                ExitEvent.WaitOne();
            }
        }
    }
}