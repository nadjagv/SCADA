using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmDisplay
{
    public class Callback : ServiceReference.IAlarmDisplayServiceCallback
    {
        public void writeToConsole(string message)
        {
            Console.WriteLine(message);
        }
    }
    class Program
    {
        static ServiceReference.AlarmDisplayServiceClient ad;
        static void Main(string[] args)
        {
            InstanceContext ic = new InstanceContext(new Callback());
            ad = new ServiceReference.AlarmDisplayServiceClient(ic);
            ad.initAlarmService();

            Console.ReadKey();
        }
    }
}
