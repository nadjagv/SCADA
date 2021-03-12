using SCADACore.model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SCADACore
{
    public static class DataManipulator
    {
        public static SCADAContext db = new SCADAContext();
        private static readonly object locker = new object();
        public static SCADAContext loadData()
        {
            //load from xml
            lock (locker)
            {
                XElement xmlData = XElement.Load("C:\\Users\\Nadja\\Documents\\TRECA GODINA\\SNUS\\SW_10_2018_PROJEKAT\\SCADACore\\SCADAConfig.xml");

                foreach (var r in db.AIs)
                    db.AIs.Remove(r);
                foreach (var r in db.AOs)
                    db.AOs.Remove(r);
                foreach (var r in db.DIs)
                    db.DIs.Remove(r);
                foreach (var r in db.DOs)
                    db.DOs.Remove(r);
                foreach (var r in db.Alarms)
                    db.Alarms.Remove(r);
                foreach (var r in db.Users)
                    db.Users.Remove(r);

                db.SaveChanges();

                var allAIsXml = xmlData.Descendants("AI");
                List<AI> listAIs = new List<AI>();
                foreach (XElement node in allAIsXml)
                {
                    try
                    {
                        string id = (string)node;
                        string description = (string)node.Attribute("Description");
                        string address = (string)node.Attribute("Address");
                        int scantime = (int)node.Attribute("ScanTime");
                        bool onoffscan = (bool)node.Attribute("OnOffScan");
                        double lowlimit = (double)node.Attribute("LowLimit");
                        double highlimit = (double)node.Attribute("HighLimit");
                        string units = (string)node.Attribute("Units");
                        string driver = (string)node.Attribute("Driver");

                        listAIs.Add(new AI(id, description, address, scantime, onoffscan, lowlimit, highlimit, units, driver));
                    }
                    catch
                    {
                        continue;
                    }


                }

                var allAOsXml = xmlData.Descendants("AO");
                List<AO> listAOs = new List<AO>();
                foreach (XElement node in allAOsXml)
                {
                    try
                    {
                        string id = (string)node;
                        string description = (string)node.Attribute("Description");
                        string address = (string)node.Attribute("Address");
                        double initvalue = (double)node.Attribute("InitValue");
                        double lowlimit = (double)node.Attribute("LowLimit");
                        double highlimit = (double)node.Attribute("HighLimit");

                        listAOs.Add(new AO(id, description, address, initvalue, lowlimit, highlimit));
                    }
                    catch
                    {
                        continue;
                    }



                }

                var allDIsXml = xmlData.Descendants("DI");
                List<DI> listDIs = new List<DI>();
                foreach (XElement node in allDIsXml)
                {
                    try
                    {
                        string id = (string)node;
                        string description = (string)node.Attribute("Description");
                        string address = (string)node.Attribute("Address");
                        int scantime = (int)node.Attribute("ScanTime");
                        bool onoffscan = (bool)node.Attribute("OnOffScan");
                        string driver = (string)node.Attribute("Driver");

                        listDIs.Add(new DI(id, description, address, scantime, onoffscan, driver));
                    }
                    catch
                    {
                        continue;
                    }



                }

                var allDOsXml = xmlData.Descendants("DO");
                List<DO> listDOs = new List<DO>();
                foreach (XElement node in allDOsXml)
                {
                    try
                    {
                        string id = (string)node;
                        string description = (string)node.Attribute("Description");
                        string address = (string)node.Attribute("Address");
                        int initvalue = (int)node.Attribute("InitValue");

                        listDOs.Add(new DO(id, description, address, initvalue));
                    }
                    catch
                    {
                        continue;
                    }

                }

                var allAlarmsXml = xmlData.Descendants("Alarm");
                List<Alarm> listAlarms = new List<Alarm>();
                foreach (XElement node in allAlarmsXml)
                {
                    try
                    {
                        string id = (string)node;
                        string type = (string)node.Attribute("Type");
                        bool activated = (bool)node.Attribute("Activated");
                        double value = (double)node.Attribute("CriticalValue");

                        string timestr = (string)node.Attribute("ActivationTime");
                        DateTime? time = null;
                        if (!timestr.Equals(""))
                            time = DateTime.ParseExact(timestr, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);

                        string analogtagid = (string)node.Attribute("AnalogTagId");
                        string unit = (string)node.Attribute("Unit");
                        int priority = (int)node.Attribute("Priority");

                        Alarm alarm = new Alarm(id, type, value, unit, priority, analogtagid);
                        alarm.ActivationTime = time;
                        listAlarms.Add(alarm);
                    }
                    catch
                    {
                        continue;
                    }

                }

                var allUsersXml = xmlData.Descendants("User");
                List<User> listUsers = new List<User>();
                foreach (XElement node in allUsersXml)
                {
                    try
                    {
                        string username = (string)node;
                        string password = (string)node.Attribute("Password");
                        string role = (string)node.Attribute("Role");

                        listUsers.Add(new User(username, password, role));
                    }
                    catch
                    {
                        continue;
                    }

                }


                foreach (AI a in listAIs)
                {
                    db.AIs.Add(a);
                }
                db.SaveChanges();

                foreach (AO a in listAOs)
                {
                    db.AOs.Add(a);
                }
                db.SaveChanges();
                foreach (DI a in listDIs)
                {
                    db.DIs.Add(a);
                }
                db.SaveChanges();
                foreach (DO a in listDOs)
                {
                    db.DOs.Add(a);
                }
                db.SaveChanges();

                foreach (Alarm a in listAlarms)
                {
                    db.Alarms.Add(a);
                }
                db.SaveChanges();

                foreach (User a in listUsers)
                {
                    db.Users.Add(a);
                }
                db.SaveChanges();

                Console.WriteLine("gotov load");
                return db;
            }
        }


        public static SCADAContext saveData()
        {
            XElement AIs = new XElement("Database");
            AIs.SetAttributeValue("name", "AIs");
            foreach (AI tag in db.AIs)
            {
                XElement el = new XElement("AI", tag);
                el.Value = tag.Id;
                el.SetAttributeValue("Description", tag.Description);
                el.SetAttributeValue("Address", tag.Address);
                el.SetAttributeValue("ScanTime", tag.ScanTime);
                el.SetAttributeValue("OnOffScan", tag.OnOffScan);
                el.SetAttributeValue("LowLimit", tag.LowLimit);
                el.SetAttributeValue("HighLimit", tag.HighLimit);
                el.SetAttributeValue("Units", tag.Units);
                el.SetAttributeValue("Driver", tag.Driver);

                AIs.Add(el);
            }

            XElement DIs = new XElement("Database");
            DIs.SetAttributeValue("name", "DIs");
            foreach (DI tag in db.DIs)
            {
                XElement el = new XElement("DI", tag);
                el.Value = tag.Id;
                el.SetAttributeValue("Description", tag.Description);
                el.SetAttributeValue("Address", tag.Address);
                el.SetAttributeValue("ScanTime", tag.ScanTime);
                el.SetAttributeValue("OnOffScan", tag.OnOffScan);
                el.SetAttributeValue("Driver", tag.Driver);

                DIs.Add(el);
            }

            XElement AOs = new XElement("Database");
            AOs.SetAttributeValue("name", "AOs");
            foreach (AO tag in db.AOs)
            {
                XElement el = new XElement("AO", tag);
                el.Value = tag.Id;
                el.SetAttributeValue("Description", tag.Description);
                el.SetAttributeValue("Address", tag.Address);
                el.SetAttributeValue("LowLimit", tag.LowLimit);
                el.SetAttributeValue("HighLimit", tag.HighLimit);
                el.SetAttributeValue("InitValue", tag.InitValue);

                AOs.Add(el);
            }

            XElement DOs = new XElement("Database");
            DOs.SetAttributeValue("name", "DOs");
            foreach (DO tag in db.DOs)
            {
                XElement el = new XElement("DO", tag);
                el.Value = tag.Id;
                el.SetAttributeValue("Description", tag.Description);
                el.SetAttributeValue("Address", tag.Address);
                el.SetAttributeValue("InitValue", tag.InitValue);

                DOs.Add(el);
            }

            XElement Alarms = new XElement("Database");
            Alarms.SetAttributeValue("name", "Alarms");
            foreach (Alarm a in db.Alarms)
            {
                XElement el = new XElement("Alarm", a);
                el.Value = a.Id;
                el.SetAttributeValue("Type", a.Type.ToString());
                el.SetAttributeValue("Activated", a.Activated);
                el.SetAttributeValue("CriticalValue", a.CriticalValue);
                el.SetAttributeValue("ActivationTime", a.ActivationTime.ToString());
                el.SetAttributeValue("AnalogTagId", a.AnalogTagId);
                el.SetAttributeValue("Priority", a.Priority);
                el.SetAttributeValue("Unit", a.Unit);

                Alarms.Add(el);
            }

            XElement Users = new XElement("Database");
            Users.SetAttributeValue("name", "Users");
            foreach (User a in db.Users)
            {
                XElement el = new XElement("User", a);
                el.Value = a.Username;
                el.SetAttributeValue("Password", a.Password);
                el.SetAttributeValue("Role", a.Role.ToString());

                Users.Add(el);

            }

            XElement root = new XElement("Databases");
            root.Add(AIs);
            root.Add(AOs);
            root.Add(DIs);
            root.Add(DOs);
            root.Add(Alarms);
            root.Add(Users);
            root.Save("C:\\Users\\Nadja\\Documents\\TRECA GODINA\\SNUS\\SW_10_2018_PROJEKAT\\SCADACore\\SCADAConfig.xml");

            return db;

        }
    }
}