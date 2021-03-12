using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCADACore.model
{
    public class TagValue
    {
        public string TagType { get; set; }
        public string TagId { get; set; }

        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public double Value { get; set; }

        public TagValue() { }
        public TagValue(string tagtype, string tagid, int id, DateTime time, double value )
        {
            TagType = tagtype;
            TagId = tagid;
            Id = id;
            Time = time;
            Value = value;
        }

        public override string ToString()
        {
            return $"Tag: {TagType} | Id: {TagId} | Value: {Value} | Time: {Time}";
        }

    }
}