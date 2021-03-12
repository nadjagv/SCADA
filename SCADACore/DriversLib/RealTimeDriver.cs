using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriversLib
{
    public static class RealTimeDriver
    {
        static Dictionary<string, double> addressData = new Dictionary<string, double>();
        public static double ReturnValue(string address)
        {
            if (addressData.ContainsKey(address))
                return addressData[address];
            
            return -1000;
        }

        public static void WriteValue(string address, double data)
        {
            if (addressData.ContainsKey(address))
                addressData[address] = data;
            else
                addressData.Add(address, data);
        }
    }
}
