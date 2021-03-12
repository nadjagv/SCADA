using SCADACore.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADACore
{
    public enum AlarmType { LOW, HIGH };
    [DataContract]
    public class Alarm
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public AlarmType Type { get; set; }
        [DataMember]
        public bool Activated { get; set; }

        [DataMember]
        public double CriticalValue { get; set; }

        [DataMember]
        public DateTime? ActivationTime { get; set; }
        [DataMember]
        public string Unit { get; set; }
        [DataMember]
        public int Priority { get; set; }

        [ForeignKey("AI")]
        [DataMember]
        public string AnalogTagId { get; set; }
        public virtual AI AI { get; set; }


        public Alarm() { }
        public Alarm(string id, string type, double value, string unit, int priority, string analogtagid)
        {
            Id = id;
            Type = (AlarmType)Enum.Parse(typeof(AlarmType), type);
            CriticalValue = value;
            Unit = unit;
            Priority = priority;
            Activated = false;
            ActivationTime = null;

            AnalogTagId = analogtagid;
        }

        public bool checkAlarmActivated(double value)
        {
            Activated = false;
            if (Type == AlarmType.HIGH && value > CriticalValue)
            {
                Activated = true;
                ActivationTime = DateTime.Now;
            }
            else if (Type == AlarmType.LOW && value < CriticalValue)
            {
                Activated = true;
                ActivationTime = DateTime.Now;
            }

            return Activated;
        }
    }
}