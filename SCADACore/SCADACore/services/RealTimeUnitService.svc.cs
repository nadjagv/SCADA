using DriversLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace SCADACore.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RealTimeUnitService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RealTimeUnitService.svc or RealTimeUnitService.svc.cs at the Solution Explorer and start debugging.
    public class RealTimeUnitService : IRealTimeUnitService
    {
        static private CspParameters csp;
        static private RSACryptoServiceProvider rsa;
        static private string pubPath;

        readonly object locker = new object();


        //const string PUBLIC_KEY_FILE = @"rsaPublicKey.txt";
        private static void ImportPublicKey(string PUBLIC_KEY_FILE)
        {
            string path = PUBLIC_KEY_FILE;
            //Provera da li fajl sa javnim ključem postoji na prosleđenoj lokaciji
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    csp = new CspParameters();
                    rsa = new RSACryptoServiceProvider(csp);
                    string publicKeyText = reader.ReadToEnd();
                    rsa.FromXmlString(publicKeyText);
                }
            }
        }

        private static bool VerifySignedMessage(string message, byte[] signature)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                var deformatter = new RSAPKCS1SignatureDeformatter(rsa);
                deformatter.SetHashAlgorithm("SHA256");
                return deformatter.VerifySignature(hashValue, signature);
            }
        }
        


        public void sendData(string address, string datastr, byte[] signature)
        {
            ImportPublicKey(pubPath);
            lock (locker)
            {
                Console.WriteLine("Validating...");
                bool ok = VerifySignedMessage(datastr, signature);
                if (!ok)
                {
                    Console.WriteLine("Validation failed. ");
                    return;
                }
            }

            double data = Convert.ToDouble(datastr);
            RealTimeDriver.WriteValue(address, data);
        }

        public void pubInit(string keyPath)
        {
            pubPath = keyPath;
        }

        
    }
}
