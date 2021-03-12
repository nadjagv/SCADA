using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealTimeUnit
{
    class Program
    {
        static ServiceReference.RealTimeUnitServiceClient pub = new ServiceReference.RealTimeUnitServiceClient();
        static private CspParameters csp;
        static private RSACryptoServiceProvider rsa;

        public static void CreateAsmKeys()
        {
            csp = new CspParameters();
            rsa = new RSACryptoServiceProvider(csp);
        }

        private static byte[] SignMessage(string message)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                var formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("SHA256");
                return formatter.CreateSignature(hashValue);
            }
        }


        const string PUBLIC_KEY_FILE = "C:\\Users\\Nadja\\Documents\\TRECA GODINA\\SNUS\\SW_10_2018_PROJEKAT\\publicKey.txt";
        private static void ExportPublicKey()
        {
            //Kreiranje foldera za eksport ukoliko on ne postoji

            string path = PUBLIC_KEY_FILE;
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(rsa.ToXmlString(false));
            }
        }
        static void Main(string[] args)
        {
            CreateAsmKeys();
            ExportPublicKey();
            pub.pubInit(PUBLIC_KEY_FILE);

            while (true)
            {
                Console.WriteLine("Id: ");
                string id = Console.ReadLine();
                Console.WriteLine("Address: ");
                string address = Console.ReadLine();
                
                int lowlimit, highlimit;
                try
                {
                    
                    Console.WriteLine("Low limit: ");
                    lowlimit = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("High limit: ");
                    highlimit = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Bad argument input. Limits are int.\n");
                    break;
                }

                //generate and send data

                while (true)
                {

                    Random rand = new Random();
                    int r = rand.Next(lowlimit, highlimit);
                    string data = Convert.ToString(r);
                    pub.sendData(address, data, SignMessage(data));
                    Thread.Sleep(1000);
                }
            }

        }

        
    }
}
