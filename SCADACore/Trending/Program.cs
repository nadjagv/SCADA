using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Trending
{
    public class Callback : ServiceReference.ITrendingServiceCallback
    {
        public void writeToConsole(string message)
        {
            Console.WriteLine(message);
        }
    }

    class Program
    {
        static ServiceReference.TrendingServiceClient ts;
        static void Main(string[] args)
        {
            InstanceContext ic = new InstanceContext(new Callback());
            ts = new ServiceReference.TrendingServiceClient(ic);
            ts.initTrendingService();

            Console.ReadKey();

        }
    }

}
