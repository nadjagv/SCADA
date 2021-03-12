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
    public class DI
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
        public List<Alarm> Alarms { get; set; }
        [DataMember]
        public bool OnOffScan { get; set; }


        public DI() { }
        public DI(string id, string description, string address, int scantime, bool onoffscan,string driver)
        {
            Id = id;
            Description = description;
            Address = address;
            ScanTime = scantime;
            OnOffScan = onoffscan;
            Driver = driver;
        }

        public void addAlarm(Alarm a)
        {
            Alarms.Add(a);
        }

        public override string ToString()
        {
            return $"{Id} | {Description} | {Address} | {ScanTime} | {OnOffScan} | {Driver}";
        }
    }
}