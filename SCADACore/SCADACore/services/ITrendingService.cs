using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADACore.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITrendingService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(ITrendingCallback))]
    public interface ITrendingService
    {
        [OperationContract (IsOneWay = true)]
        void initTrendingService();
    }

    public interface ITrendingCallback
    {
        [OperationContract(IsOneWay = true)]
        void writeToConsole(string tagstr);
    }
}
