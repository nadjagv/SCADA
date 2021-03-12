using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADACore.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrendingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TrendingService.svc or TrendingService.svc.cs at the Solution Explorer and start debugging.
    public class TrendingService : ITrendingService
    {
        public SCADAContext db = new SCADAContext();
        delegate void trendingDelegate(string message);

        static event trendingDelegate onScan;

        ITrendingCallback proxy;
        public void initTrendingService()
        {
            proxy = OperationContext.Current.GetCallbackChannel<ITrendingCallback>();
            onScan += proxy.writeToConsole;
            TagProcessing.trending = this;
        }

        public void write(string message)
        {
            try
            {
                if (onScan != null)
                    onScan.Invoke(message);
            }
            catch
            {
                return;
            }
            
        }
    }
}
