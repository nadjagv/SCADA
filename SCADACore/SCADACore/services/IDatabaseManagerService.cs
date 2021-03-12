using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADACore.services
{
    [ServiceContract(CallbackContract = typeof(IDatabaseCallback))]
    public interface IDatabaseManagerService
    {
        [OperationContract(IsOneWay = true)]
        void initService();

        [OperationContract (IsOneWay =false)]
        string logIn(string username, string password);
        [OperationContract (IsOneWay = false)]
        bool register(string username, string password, string role);

        //operation == add/update
        [OperationContract (IsOneWay = true)]
        void addUpdateAO(string operation, string id, string description, string address, double initvalue, double lowlimit, double highlimit);
        [OperationContract (IsOneWay = true)]
        void addUpdateAI(string operation, string id, string description, string address, int scantime, bool onoffscan, double lowlimit, double highlimit, string units, string driver);
        [OperationContract (IsOneWay = true)]
        void addUpdateDO(string operation, string id, string description, string address, double initvalue);
        [OperationContract (IsOneWay = true)]
        void addUpdateDI(string operation, string id, string description, string address, int scantime, bool onoffscan, string driver);


        [OperationContract(IsOneWay = true)]
        void changeValueAO(string id, double value);

        [OperationContract(IsOneWay = true)]
        void changeValueDO(string id, double value);


        [OperationContract(IsOneWay = true)]
        void deleteAO(string id);

        [OperationContract(IsOneWay = true)]
        void deleteDO(string id);

        [OperationContract(IsOneWay = true)]
        void deleteAI(string id);

        [OperationContract(IsOneWay = true)]
        void deleteDI(string id);


        [OperationContract (IsOneWay = true)]
        void addTagAlarm(string analogtagid, string id, string type, double value, string unit, int priority);
        [OperationContract(IsOneWay = true)]
        void removeTagAlarm(string tagid, string id);


        [OperationContract(IsOneWay = true)]
        void TurnScanOnOff(string tagType, string tagId, bool onOff);


        AO getByIdAO(string id);
        AI getByIdAI(string id);
        DO getByIdDO(string id);
        DI getByIdDI(string id);

        //return all in string
        [OperationContract (IsOneWay = false)]
        string getAOs();
        [OperationContract (IsOneWay = false)]
        string getAIs();
        [OperationContract (IsOneWay = false)]
        string getDOs();
        [OperationContract (IsOneWay = false)]
        string getDIs();

        //find one by id and return strin
        [OperationContract(IsOneWay = false)]
        string getStrAO(string id);
        [OperationContract(IsOneWay = false)]
        string getStrAI(string id);
        [OperationContract(IsOneWay = false)]
        string getStrDO(string id);
        [OperationContract(IsOneWay = false)]
        string getStrDI(string id);
    }

    public interface IDatabaseCallback
    {
        [OperationContract(IsOneWay = true)]
        void notifyClient(string message);
    }
}
