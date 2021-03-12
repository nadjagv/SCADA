using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADACore.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReportManagerService" in both code and config file together.
    [ServiceContract]
    public interface IReportManagerService
    {
        [OperationContract(IsOneWay = false)]
        string getReport1(DateTime start, DateTime end);
        [OperationContract(IsOneWay = false)]
        string getReport2(int priority);
        [OperationContract(IsOneWay = false)]
        string getReport3(DateTime start, DateTime end);
        [OperationContract(IsOneWay = false)]
        string getReport4();
        [OperationContract(IsOneWay = false)]
        string getReport5();
        [OperationContract(IsOneWay = false)]
        string getReport6(string id);

    }
}
