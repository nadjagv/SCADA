using SCADACore.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADACore
{
    [DataContract]
    public class AI
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Driver { get; set; } //RTD ili SD
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public int ScanTime { get; set; }
        [DataMember]
        public virtual List<Alarm> Alarms { get; set; }
        [DataMember]
        public bool OnOffScan { get; set; }
        [DataMember]
        public double LowLimit { get; set; }
        [DataMember]
        public double HighLimit { get; set; }
        [DataMember]
        public string Units { get; set; }


        public AI() { }
        public AI(string id, string description, string address, int scantime, bool onoffscan, double lowlimit, double highlimit, string units, string driver) 
        {
            Id = id;
            Description = description;
            Address = address;
            ScanTime = scantime;
            OnOffScan = onoffscan;
            LowLimit = lowlimit;
            HighLimit = highlimit;
            Units = units;
            Driver = driver;
            Alarms = new List<Alarm>();
        }

        public void addAlarm(Alarm a)
        {
            Alarms.Add(a);
        }

        public override string ToString()
        {
            string alarmIds = "";
            foreach (Alarm a in Alarms)
                alarmIds += a.Id;
            return $"{Id} | {Description} | {Address} | {ScanTime} | {OnOffScan} | {LowLimit} | {HighLimit} | {Units} | {Driver} | {alarmIds}";
        }

    }
}