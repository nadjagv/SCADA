using SCADACore.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADACore.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportManagerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReportManagerService.svc or ReportManagerService.svc.cs at the Solution Explorer and start debugging.
    public class ReportManagerService : IReportManagerService
    {
        public SCADAContext db = new SCADAContext();

        public string alarmLogPath = "C:\\Users\\Nadja\\Documents\\TRECA GODINA\\SNUS\\SW_10_2018_PROJEKAT\\alarmsLog.txt";
        public string getReport1(DateTime start, DateTime end)
        {
            lock (db)
            {
                string report = "";
                string[] lines = System.IO.File.ReadAllLines(alarmLogPath);

                foreach (string line in lines)
                {
                    string[] tokens = line.Split(',');
                    string[] timeTokens = tokens[1].Split('>');
                    string timeStr = timeTokens[1];
                    DateTime time = DateTime.Parse(timeStr);
                    if (time > start && time < end)
                        report += line + "\n";
                }
                if (report == "")
                    return "No data found.\n";
                return report;
            }
        }

        

        public string getReport2(int priority)
        {
            lock (db)
            {
                string report = "";
                var res = from alarm in db.Alarms where alarm.Priority == priority select alarm;
                var resOrdered = res.OrderBy(alarm => alarm.ActivationTime);
                foreach (Alarm a in res)
                {
                    report += $"Id: {a.Id}\tPriority: {a.Priority}\t Type: {a.Type.ToString()}\t Time: {a.ActivationTime.ToString()}\n";
                }
                if (report == "")
                    return "No data found.\n";
                return report;
            }
        }

        

        public string getReport3(DateTime start, DateTime end)
        {
            lock (db)
            {
                string report = "";
                var res = from v in db.TagValues where v.Time > start && v.Time < end select v;
                var resOrdered = res.OrderBy(v => v.Time);
                foreach (TagValue a in res)
                {
                    report += a.ToString() + "\n";
                }
                if (report == "")
                    return "No data found.\n";
                return report;
            }
        }

        

        public string getReport4()
        {
            lock (db)
            {
                string report = "";
                var res = from v in db.TagValues where v.TagType == "AI" select v;
                var resOrdered = res.OrderBy(v => v.Time);
                foreach (TagValue a in res)
                {
                    report += a.ToString() + "\n";
                }
                if (report == "")
                    return "No data found.\n";
                return report;
            }
        }

        public string getReport5()
        {
            lock (db)
            {
                string report = "";
                var res = from v in db.TagValues where v.TagType == "DI" select v;
                var resOrdered = res.OrderBy(v => v.Time);
                foreach (TagValue a in res)
                {
                    report += a.ToString() + "\n";
                }
                if (report == "")
                    return "No data found.\n";
                return report;
            }
        }

        public string getReport6(string id)
        {
            lock (db)
            {
                string report = "";
                var res = from v in db.TagValues where v.TagId == id select v;
                var resOrdered = res.OrderBy(v => v.Value);
                foreach (TagValue a in res)
                {
                    report += a.ToString() + "\n";
                }
                if (report == "")
                    return "No data found.\n";
                return report;
            }
        }
    }
}
