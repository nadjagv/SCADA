using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADACore.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAlarmDisplayService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IAlarmCallback))]
    public interface IAlarmDisplayService
    {
        [OperationContract(IsOneWay = true)]
        void initAlarmService();
    }

    public interface IAlarmCallback
    {
        [OperationContract(IsOneWay = true)]
        void writeToConsole(string tagstr);
    }
}
