using DriversLib;
using SCADACore.model;
using SCADACore.services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace SCADACore
{
    
    public static class TagProcessing
    {
        public static Dictionary<string, AI> AItags { get; set; }
        public static Dictionary<string, DI> DItags { get; set; }

        public static Dictionary<string, Thread> AIthreads { get; set; }
        public static Dictionary<string, Thread> DIthreads { get; set; }

        public static SCADAContext db;
        public static TrendingService trending;
        public static AlarmDisplayService alarming;
        private static int dataKeyId;
        readonly static object locker = new object();

        public static string alarmLogPath = "C:\\Users\\Nadja\\Documents\\TRECA GODINA\\SNUS\\SW_10_2018_PROJEKAT\\alarmsLog.txt";
        static TagProcessing()
        {
            dataKeyId = 0;
            db = DataManipulator.loadData();
            //vidi sta se desava kada korisnik doda tag
            AItags = new Dictionary<string, AI>();
            DItags = new Dictionary<string, DI>();

            AIthreads = new Dictionary<string, Thread>();
            DIthreads = new Dictionary<string, Thread>();

            trending = new TrendingService();
            alarming = new AlarmDisplayService();

            foreach (AI ai in db.AIs) {
                AItags.Add(ai.Id, ai);
                AIthreads.Add(ai.Id, new Thread(()=> readAI(ai.Id)));

            }

            foreach (DI di in db.DIs)
            {
                DItags.Add(di.Id, di);
                DIthreads.Add(di.Id, new Thread(() => readDI(di.Id)));
            }

            //start threads
            foreach(Thread t in AIthreads.Values)
            {
                t.Start();
            }

            foreach (Thread t in DIthreads.Values)
            {
                t.Start();
            }

            Thread saveThread = new Thread(() =>
            {
                while (true)
                {
                    lock (db)
                    {
                        db.SaveChanges();
                    }
                    Thread.Sleep(10000);
                }
            });

            saveThread.Start();
        }

        public static void readAI(string id) 
        {

            double oldVal = -10000;
            while (true)
            {
                AI tag;
                lock (db.AIs)
                {
                    try
                    {
                        tag = AItags[id];
                    }
                    catch
                    {
                        return;
                    }
                    
                    double value;
                    if (tag.OnOffScan)
                    {
                        if (tag.Driver == "SD")
                            value = SimulationDriver.ReturnValue(tag.Address);
                        else
                            value = RealTimeDriver.ReturnValue(tag.Address);

                        if (value > tag.HighLimit)
                            value = tag.HighLimit;
                        else if (value < tag.LowLimit)
                            value = tag.LowLimit;
                        lock (locker)
                        {
                            TagValue tv = new TagValue("AI", tag.Id, dataKeyId, DateTime.Now, value);
                            dataKeyId++;
                            db.TagValues.Add(tv);
                        }

                        foreach(Alarm a in tag.Alarms)
                        {
                            if (a.checkAlarmActivated(value))
                            {
                                lock (locker)
                                {
                                    string alarmLogStr = $"AI tag  ID: {tag.Id} is {a.Type.ToString()},\t Time>{DateTime.Now}";
                                    StreamWriter file = new StreamWriter(alarmLogPath, append: true);
                                    file.WriteLine(alarmLogStr);
                                    file.Close();

                                    for (int i = 0; i < a.Priority; i++)
                                    {
                                        lock (locker)
                                        {
                                            if (alarming != null)
                                                alarming.write($"WARNING! AI tag  ID: {tag.Id} is {a.Type.ToString()}");
                                        }

                                    }
                                }
                                

                            }
                        }

                        if (trending != null && oldVal != value)
                            trending.write($"AI tag\t ID: {tag.Id}\t VALUE: {value} ");
                        oldVal = value;

                    }

                }
                if (tag != null)
                {
                    Thread.Sleep(tag.ScanTime * 1000);
                }
            }

            
        }

        public static void readDI(string id)
        {
            while (true)
            {
                DI tag;
                lock (db.DIs)
                {
                    try
                    {
                        tag = DItags[id];
                    }
                    catch
                    {
                        return;
                    }
                    
                    double value;
                    if (tag.OnOffScan)
                    {
                        if (tag.Driver == "SD")
                            value = SimulationDriver.ReturnValue(tag.Address);
                        else
                            value = RealTimeDriver.ReturnValue(tag.Address);

                        if (value > 0.5)
                            value = 1;
                        else 
                            value = 0;

                        lock (locker)
                        {
                            TagValue tv = new TagValue("DI", tag.Id, dataKeyId, DateTime.Now, value);
                            dataKeyId++;
                            db.TagValues.Add(tv);
                            db.SaveChanges();
                        }


                        trending.write($"DI tag\t ID: {tag.Id}\t VALUE: {value} ");


                    }

                }
                if (tag != null)
                {
                    Thread.Sleep(tag.ScanTime * 1000);
                }
            }
        }


    }
}