using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static TelemetryClient client = new TelemetryClient()
        {
            InstrumentationKey = "269032bd-5e2c-42c8-b231-aea41ec4ca37"
        };

        public static object Delay { get; private set; }

        static void Main(string[] args)
        {
            for (int i = 0; i < 10000; ++i)
            {
                client.TrackEvent("Hello Buffer");
                Thread.Sleep(100);
            }

            client.Flush();
            Thread.Sleep(20000);
        }
    }
}
