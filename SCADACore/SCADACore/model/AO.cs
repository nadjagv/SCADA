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
    public class AO
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public double InitValue { get; set; }
        [DataMember]
        public double LowLimit { get; set; }
        [DataMember]
        public double HighLimit { get; set; }


        public AO() { }
        public AO(string id, string description, string address, double initvalue, double lowlimit, double highlimit)
        {
            Id = id;
            Description = description;
            Address = address;

            if (initvalue < lowlimit)
                InitValue = lowlimit;
            else if (initvalue > highlimit)
                InitValue = highlimit;
            else
                InitValue = initvalue;

            LowLimit = lowlimit;
            HighLimit = highlimit;
        }

        public override string ToString()
        {
            return $"{Id} | {Description} | {Address} | {InitValue} | {LowLimit} | {HighLimit}";
        }
    }
}