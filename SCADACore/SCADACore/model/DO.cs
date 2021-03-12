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
    public class DO
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public int InitValue { get; set; }


        public DO() { }
        public DO(string id, string description, string address, int initvalue)
        {
            Id = id;
            Description = description;
            Address = address;
            InitValue = initvalue;
        }

        public override string ToString()
        {
            return $"{Id} {Description} {Address} {InitValue}";
        }
    }
}