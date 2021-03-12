using SCADACore.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace SCADACore.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DatabaseManagerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DatabaseManagerService.svc or DatabaseManagerService.svc.cs at the Solution Explorer and start debugging.
    public class DatabaseManagerService : IDatabaseManagerService
    {
        public SCADAContext db;
        delegate void notifyDelegate(string message);

        static event notifyDelegate onChangeNotification;

        IDatabaseCallback proxy;


        private static string EncryptData(string valueToEncrypt)
        {
            string GenerateSalt()
            {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
            string EncryptValue(string strValue)
            {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strValue);
                using (SHA256Managed sha = new SHA256Managed())
                {
                    byte[] hash = sha.ComputeHash(saltedPassword);
                    return $"{Convert.ToBase64String(hash)}:{saltValue}";
                }
            }
            return EncryptValue(valueToEncrypt);
        }


        private static bool ValidateEncryptedData(string valueToValidate, string valueFromDatabase)
        {
            string[] arrValues = valueFromDatabase.Split(':');
            string encryptedDbValue = arrValues[0];
            string salt = arrValues[1];
            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + valueToValidate);
            using (var sha = new SHA256Managed())
            {
                byte[] hash = sha.ComputeHash(saltedValue);
                string enteredValueToValidate = Convert.ToBase64String(hash);
                return encryptedDbValue.Equals(enteredValueToValidate);
            }
        }


        public void initService()
        {
            proxy = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();
            onChangeNotification += proxy.notifyClient;
            db = DataManipulator.loadData();
        }

        public void TurnScanOnOff(string tagType, string tagId, bool onOff)
        {
            lock (db)
            {
                if (tagType == "AI")
                {
                    AI tag = getByIdAI(tagId);
                    tag.OnOffScan = onOff;
                }
                else
                {
                    DI tag = getByIdDI(tagId);
                    tag.OnOffScan = onOff;
                }
                
                db.SaveChanges();
                DataManipulator.saveData();
                if (onOff)
                    onChangeNotification($"Scan turned on.");
                else
                    onChangeNotification($"Scan turned off.");

            }
        }
        public void addTagAlarm(string analogtagid, string id, string type, double value, string unit, int priority)
        {
            lock (db)
            {
                if (getAlarmById(id) != null)
                {
                    onChangeNotification.Invoke("ID already exists. Choose different id.");
                    return;
                }
                Alarm novi = new Alarm(id, type, value, unit, priority, analogtagid);
                db.Alarms.Add(novi);
                AI tag = getByIdAI(analogtagid);
                tag.Alarms.Add(novi);
                db.SaveChanges();
                DataManipulator.saveData();
                onChangeNotification.Invoke("Alarm added succesfully.");
            }
        }

        public Alarm getAlarmById (string id){
            foreach (Alarm a in db.Alarms)
            {
                if (a.Id == id)
                    return a;
            }
            return null;
        }

        public void removeTagAlarm(string tagid, string id)
        {
            lock (db)
            {
                Alarm a = getAlarmById(id);
                if (a == null)
                {
                    onChangeNotification.Invoke("This alarm does not exist");
                    return;
                }
                AI tag = getByIdAI(a.AnalogTagId);
                if (tagid != tag.Id)
                {
                    onChangeNotification.Invoke("This alarm does not belong to chosen tag.");
                    return;
                }
                    
                tag.Alarms.Remove(a);
                db.Alarms.Remove(a);

                db.SaveChanges();
                DataManipulator.saveData();
                onChangeNotification.Invoke("Alarm removed succesfully.");
            }

        }

        public void addUpdateAI(string operation, string id, string description, string address, int scantime, bool onoffscan, double lowlimit, double highlimit, string units, string driver)
        {
            lock (db.AIs)
            {
                if (operation == "add")
                {
                    AI novi = new AI(id, description, address, scantime, onoffscan, lowlimit, highlimit, units, driver);
                    db.AIs.Add(novi);

                    TagProcessing.AItags.Add(novi.Id, novi);
                    TagProcessing.AIthreads.Add(novi.Id, new Thread(() => TagProcessing.readAI(novi.Id)));
                    TagProcessing.AIthreads[novi.Id].Start();

                    db.SaveChanges();
                    DataManipulator.saveData();
                    return;
                }
                AI tag = getByIdAI(id);
                tag.Description = description;
                tag.Address = address;
                tag.ScanTime = scantime;
                tag.OnOffScan = onoffscan;
                tag.LowLimit = lowlimit;
                tag.HighLimit = highlimit;
                tag.Units = units;
                tag.Driver = driver;
                db.SaveChanges();
                DataManipulator.saveData();
                TagProcessing.AItags.Remove(tag.Id);
                TagProcessing.AItags.Add(tag.Id, tag);
                TagProcessing.AIthreads.Remove(tag.Id);
                TagProcessing.AIthreads.Add(tag.Id, new Thread(() => TagProcessing.readAI(tag.Id)));
                TagProcessing.AIthreads[tag.Id].Start();
                onChangeNotification.Invoke("Operation succesful.");
            }
            
        }

        public void addUpdateAO(string operation, string id, string description, string address, double initvalue, double lowlimit, double highlimit)
        {
            lock (db.AOs)
            {
                if (operation == "add")
                {
                    AO novi = new AO(id, description, address, initvalue, lowlimit, highlimit);
                    db.AOs.Add(novi);
                    db.SaveChanges();
                    DataManipulator.saveData();
                    return;
                }

                AO tag = getByIdAO(id);
                tag.Description = description;
                tag.Address = address;
                tag.InitValue = initvalue;
                tag.LowLimit = lowlimit;
                tag.HighLimit = highlimit;
                db.SaveChanges();
                DataManipulator.saveData();
                onChangeNotification.Invoke("Operation succesful.");
            }
        }

        public void addUpdateDI(string operation, string id, string description, string address, int scantime, bool onoffscan, string driver)
        {
            lock (db.DIs)
            {
                if (operation == "add")
                {
                    DI novi = new DI(id, description, address, scantime, onoffscan, driver);
                    db.DIs.Add(novi);

                    TagProcessing.DItags.Add(novi.Id, novi);
                    TagProcessing.DIthreads.Add(novi.Id, new Thread(() => TagProcessing.readDI(novi.Id)));
                    TagProcessing.DIthreads[novi.Id].Start();

                    db.SaveChanges();
                    DataManipulator.saveData();
                    return;
                }
                DI tag = getByIdDI(id);
                tag.Description = description;
                tag.Address = address;
                tag.ScanTime = scantime;
                tag.OnOffScan = onoffscan;
                tag.Driver = driver;
                db.SaveChanges();
                DataManipulator.saveData();

                TagProcessing.DItags.Remove(tag.Id);
                TagProcessing.DItags.Add(tag.Id, tag);
                TagProcessing.DIthreads.Remove(tag.Id);
                TagProcessing.DIthreads.Add(tag.Id, new Thread(() => TagProcessing.readAI(tag.Id)));
                TagProcessing.DIthreads[tag.Id].Start();

                onChangeNotification.Invoke("Operation succesful.");
            }
        }

        public void addUpdateDO(string operation, string id, string description, string address, double initval)
        {
            int initvalue = 0;
            if (initval > 0.5)
                initvalue = 1;
            lock (db.DOs)
            {
                if (operation == "add")
                {
                    DO novi = new DO(id, description, address, initvalue);
                    db.DOs.Add(novi);
                    db.SaveChanges();
                    DataManipulator.saveData();
                    return;
                }
                DO tag = getByIdDO(id);
                tag.Description = description;
                tag.Address = address;
                tag.InitValue = initvalue;
                db.SaveChanges();
                DataManipulator.saveData();
                onChangeNotification.Invoke("Operation succesful.");
            }
        }


        public AI getByIdAI(string id)
        {
            foreach (AI tag in db.AIs)
            {
                if (tag.Id == id)
                {
                    return tag;
                }
            }
            return null;
        }

        public AO getByIdAO(string id)
        {
           
            foreach (AO tag in db.AOs)
            {
                if (tag.Id == id)
                {
                    return tag;
                }
            }
            return null;
            
        }

        public DI getByIdDI(string id)
        {
            foreach (DI tag in db.DIs)
            {
                if (tag.Id == id)
                {
                    return tag;
                }
            }
            return null;
            
        }

        public DO getByIdDO(string id)
        {
            
            foreach (DO tag in db.DOs)
            {
                if (tag.Id == id)
                {
                    return tag;
                }
            }
            return null;
            
        }

        public string getAIs()
        {
            lock (db.AIs)
            {
                string result = "";
                foreach (AI tag in db.AIs)
                {
                    result += tag.ToString() + "\n";
                }
                return result;
            }
        }

        public string getAOs()
        {
            lock (db.AOs)
            {
                string result = "";
                foreach (AO tag in db.AOs)
                {
                    result += tag.ToString() + "\n";
                }
                return result;
            }
        }

        public string getDIs()
        {
            lock (db.DIs)
            {
                string result = "";
                foreach (DI tag in db.DIs)
                {
                    result += tag.ToString() + "\n";
                }
                return result;
            }
        }

        public string getDOs()
        {
            lock (db.DOs)
            {
                string result = "";
                foreach (DO tag in db.DOs)
                {
                    result += tag.ToString() + "\n";
                }
                return result;
            }
        }

        string IDatabaseManagerService.getStrAO(string id)
        {
            AO tag = getByIdAO(id);
            if (tag == null)
                return null;
            return tag.ToString();
        }

        string IDatabaseManagerService.getStrAI(string id)
        {
            AI tag = getByIdAI(id);
            if (tag == null)
                return null;
            return tag.ToString();
        }

        string IDatabaseManagerService.getStrDO(string id)
        {
            DO tag = getByIdDO(id);
            if (tag == null)
                return null;
            return tag.ToString();
        }

        string IDatabaseManagerService.getStrDI(string id)
        {
            DI tag = getByIdDI(id);
            if (tag == null)
                return null;
            return tag.ToString();
        }

        public string logIn(string username, string password)
        {
            lock (db.Users)
            {
                if (db.Users.Count() == 0)
                {
                    bool succes = register(username, password, "ADMIN");
                    if (succes)
                        return "User registration succesful.";
                    else
                        return "Failed to register user. This username already exists in database.";
                }

                foreach (User u in db.Users)
                {
                    if (username == u.Username && ValidateEncryptedData(password, u.Password))
                    {
                        onChangeNotification.Invoke("Log in succesful.");
                        return u.Role.ToString();
                    }
                }
                return "Failed to log in. Check username and password.\n";
            }
           
        }

        public bool register(string username, string password, string role="REGULAR")
        {
            lock (db.Users)
            {
                foreach (User u in db.Users)
                {
                    if (username == u.Username)
                    {
                        return false;
                    }
                }
                string encryptedPassword = EncryptData(password);
                User novi = new User(username, encryptedPassword, role);
                db.Users.Add(novi);
                db.SaveChanges();
                DataManipulator.saveData();
                return true;

            }
        }

        public void changeValueAO(string id, double value)
        {
            lock (db.AOs)
            {
                AO tag = getByIdAO(id);
                foreach(AO t in db.AOs)
                {
                    if (t.Address == tag.Address)

                        t.InitValue = value;
                }
                db.SaveChanges();
                DataManipulator.saveData();
                onChangeNotification.Invoke("Value changed.");
            }
            
        }

        public void changeValueDO(string id, double value)
        {
            lock (db.DOs)
            {
                DO tag = getByIdDO(id);
                int val = 0;
                if (value > 0.5)
                    val = 1;
                foreach (DO t in db.DOs)
                {
                    if (t.Address == tag.Address)

                        t.InitValue = val;
                }
                db.SaveChanges();
                DataManipulator.saveData();
                onChangeNotification.Invoke("Value changed.");
            }
            
        }

       

        public void deleteAO(string id)
        {
            lock (db.AOs)
            {
                AO tag = getByIdAO(id);
                db.AOs.Remove(tag);
                
                db.SaveChanges();
                DataManipulator.saveData();
                onChangeNotification.Invoke("Operation succesful.");
            }
        }

        public void deleteDO(string id)
        {
            lock (db.DOs)
            {
                DO tag = getByIdDO(id);
                db.DOs.Remove(tag);
                db.SaveChanges();
                DataManipulator.saveData();
                onChangeNotification.Invoke("Operation succesful.");
            }
        }

        public void deleteAI(string id)
        {
            lock (db.AIs)
            {
                AI tag = getByIdAI(id);
                db.AIs.Remove(tag);
                TagProcessing.AItags.Remove(id);
                TagProcessing.AIthreads.Remove(id);
                db.SaveChanges();
                DataManipulator.saveData();
                onChangeNotification.Invoke("Operation succesful.");
            }
        }

        public void deleteDI(string id)
        {
            lock (db.DIs)
            {
                DI tag = getByIdDI(id);
                db.DIs.Remove(tag);
                TagProcessing.AItags.Remove(id);
                TagProcessing.AIthreads.Remove(id);
                db.SaveChanges();
                DataManipulator.saveData();
                onChangeNotification.Invoke("Operation succesful.");
            }
        }
    }
    }
