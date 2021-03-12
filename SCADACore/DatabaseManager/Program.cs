using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace DatabaseManager
{
    public class Callback : ServiceReference.IDatabaseManagerServiceCallback
    {
        public void notifyClient(string message)
        {
            Console.WriteLine(message);
            Thread.Sleep(2000);
        }
    }

    class Program
    {
        static ServiceReference.DatabaseManagerServiceClient dbm;
        static string role;
        static void Main(string[] args)
        {
            InstanceContext ic = new InstanceContext(new Callback());
            dbm = new ServiceReference.DatabaseManagerServiceClient(ic);
            dbm.initService();

            logIn();

        }

        static void logIn()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("LOGIN");
                Console.WriteLine("Username: ");
                string username = Console.ReadLine();
                Console.WriteLine("Password: ");
                string password = Console.ReadLine();
                role = dbm.logIn(username, password);
                if (role == "ADMIN" || role == "REGULAR")
                {
                    mainMenu();
                }
                else
                {
                    Console.WriteLine(role);
                    Console.ReadLine();
                }
            }

            
        }

        static void mainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n -----MAIN MENU-----");
                Console.WriteLine("1. Manage Analog Input (AI)");
                Console.WriteLine("2. Manage Analog Output (AO)");
                Console.WriteLine("3. Manage Digital Input (DI)");
                Console.WriteLine("4. Manage Digital Output (DO)");
                if (role == "ADMIN")
                {

                    Console.WriteLine("5. Register user.");
                    Console.WriteLine("6. Log out.");
                }
                else
                    Console.WriteLine("5. Log out.");

                int option=0;
                while (true)
                {
                    try
                    {
                        Console.WriteLine("\nOption number:  ");
                        option = Convert.ToInt32(Console.ReadLine());
                        if (option > 0 && option <= 5)
                            break;
                        else if  (option == 6 && role == "ADMIN")
                            break;
                        else
                            Console.WriteLine("Bad input.\n");
                    }
                    catch
                    {
                        Console.WriteLine("Bad input.\n");
                        Console.ReadLine();
                    }
                    
                }
                switch (option)
                {
                    case 1: AIMenu();break;
                    case 2: AOMenu(); break;
                    case 3: DIMenu(); break;
                    case 4: DOMenu(); break;
                    case 5:
                        {
                            if (role == "ADMIN")
                            {
                                registerUser(); break;
                            }   
                            else
                                return; //log out
                        }
                    case 6:
                        {
                            return; //log out (povratak na log in)
                        }
                }

                
            }
        }

        static void AIMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n---Manage AI---");
                Console.WriteLine("1. Add tag");
                Console.WriteLine("2. Edit tag");
                Console.WriteLine("3. Turn scan on/off");
                Console.WriteLine("4. Get tags");
                Console.WriteLine("5. Delete tag");
                Console.WriteLine("6. Add alarm");
                Console.WriteLine("7. Delete alarm");
                Console.WriteLine("8. Back to Main Menu");
                int option = 0;
                while (true)
                {
                    try
                    {
                        Console.WriteLine("\nOption number:  ");
                        option = Convert.ToInt32(Console.ReadLine());
                        if (option > 0 && option <= 8)
                            break;
                        else
                            Console.WriteLine("Bad input.\n");
                    }
                    catch
                    {
                        Console.WriteLine("Bad input.\n");
                    }

                }
                switch (option)
                {
                    case 1:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();
                            Console.WriteLine("Description: ");
                            string description = Console.ReadLine();
                            Console.WriteLine("Address: ");
                            string address = Console.ReadLine();
                            Console.WriteLine("Driver: ");
                            string driver = Console.ReadLine();
                            int scantime;
                            double lowlimit, highlimit;
                            bool scanonoff;
                            try
                            {
                                Console.WriteLine("Scan time: ");
                                scantime = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("On/Off scan (true = \"ON\"    false = \"OFF\"): ");
                                scanonoff = Convert.ToBoolean(Console.ReadLine());
                                Console.WriteLine("Low limit: ");
                                lowlimit = Convert.ToDouble(Console.ReadLine());
                                Console.WriteLine("High limit: ");
                                highlimit = Convert.ToDouble(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Bad argument input. Scan time is int, limits are double and on/off scan is bool.\n");
                                break;
                            }
                            
                            Console.WriteLine("Units: ");
                            string units = Console.ReadLine();
                            dbm.addUpdateAI("add", id, description, address, scantime, scanonoff, lowlimit, highlimit, units, driver);
                            //alarms
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();

                            string tagstr = dbm.getStrAI(id);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }
                            Console.WriteLine(tagstr);


                            Console.WriteLine("Description: ");
                            string description = Console.ReadLine();
                            Console.WriteLine("Address: ");
                            string address = Console.ReadLine();
                            Console.WriteLine("Driver: ");
                            string driver = Console.ReadLine();
                            int scantime;
                            double lowlimit, highlimit;
                            bool scanonoff;
                            try
                            {
                                Console.WriteLine("Scan time: ");
                                scantime = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("On/Off scan (true = \"ON\"    false = \"OFF\"): ");
                                scanonoff = Convert.ToBoolean(Console.ReadLine());
                                Console.WriteLine("Low limit: ");
                                lowlimit = Convert.ToDouble(Console.ReadLine());
                                Console.WriteLine("High limit: ");
                                highlimit = Convert.ToDouble(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Bad argument input. Scan time is int, limits are double and on/off scan is bool.\n");
                                Console.ReadLine();
                                break;
                            }
                            Console.WriteLine("Units: ");
                            string units = Console.ReadLine();
                            dbm.addUpdateAI("update", id, description, address, scantime, scanonoff, lowlimit, highlimit, units, driver);
                            //alarms
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();

                            string tagstr = dbm.getStrAI(id);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }
                            Console.WriteLine(tagstr);


                            Console.WriteLine("On/Off scan (true = \"ON\"    false = \"OFF\"): ");
                            bool scanonoff = Convert.ToBoolean(Console.ReadLine());
                            dbm.TurnScanOnOff("AI", id, scanonoff);
                            Console.Clear();
                            break;
                        }
                    case 4: Console.WriteLine(dbm.getAIs()); Console.ReadLine(); break;
                    case 5:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();

                            string tagstr = dbm.getStrAI(id);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }

                            dbm.deleteAI(id);


                            break;
                        }
                    case 6:
                        {
                            Console.WriteLine("Tag id: ");
                            string tagId = Console.ReadLine();

                            string tagstr = dbm.getStrAI(tagId);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }
                            Console.WriteLine("Alarm id: ");
                            string id = Console.ReadLine();
                            Console.WriteLine("Type: (enter HIGH or LOW)");
                            string type = Console.ReadLine();
                            int priority;
                            double value;
                            try
                            {
                                while (true)
                                {
                                    Console.WriteLine("Priority: (1, 2 or 3)");
                                    priority = Convert.ToInt32(Console.ReadLine());
                                    if (priority >= 1 && priority <= 3)
                                        break;
                                    Console.WriteLine("Bad input. Priority must be 1, 2 or 3");
                                }
                                
                                Console.WriteLine("Critical value: ");
                                value = Convert.ToDouble(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Bad argument input. Priority is int nad critical value is double.\n");
                                break;
                            }

                            Console.WriteLine("Units: ");
                            string units = Console.ReadLine();

                            dbm.addTagAlarm(tagId, id, type.ToUpper(), value, units, priority);

                            break;
                        }
                    case 7:
                        {
                            Console.WriteLine("Tag id: ");
                            string tagId = Console.ReadLine();

                            string tagstr = dbm.getStrAI(tagId);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }
                            Console.WriteLine("Alarm id: ");
                            string id = Console.ReadLine();
                            

                            dbm.removeTagAlarm(tagId, id);

                            break;
                        }
                    case 8: return;

                }
            }
            
        }
        static void AOMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n---Manage AO---");
                Console.WriteLine("1. Add tag");
                Console.WriteLine("2. Edit tag");
                Console.WriteLine("3. Get tags");
                Console.WriteLine("4. Set value");
                Console.WriteLine("5. Remove tag");
                Console.WriteLine("6. Back to Main Menu");
                int option = 0;
                while (true)
                {
                    try
                    {
                        Console.WriteLine("\nOption number:  ");
                        option = Convert.ToInt32(Console.ReadLine());
                        if (option > 0 && option <= 6)
                            break;
                        else
                            Console.WriteLine("Bad input.\n");
                    }
                    catch
                    {
                        Console.WriteLine("Bad input.\n");
                    }

                }
                switch (option)
                {
                    case 1:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();
                            Console.WriteLine("Description: ");
                            string description = Console.ReadLine();
                            Console.WriteLine("Address: ");
                            string address = Console.ReadLine();
                            double lowlimit, highlimit, initvalue;
                            try
                            {
                                Console.WriteLine("Initial value: ");
                                initvalue = Convert.ToDouble(Console.ReadLine());
                                Console.WriteLine("Low limit: ");
                                lowlimit = Convert.ToDouble(Console.ReadLine());
                                Console.WriteLine("High limit: ");
                                highlimit = Convert.ToDouble(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Bad argument input. Limits and initial value are double.\n");
                                break;
                            }
                            dbm.addUpdateAO("add", id, description, address, initvalue, lowlimit, highlimit);
                            
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();

                            string tagstr = dbm.getStrAO(id);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }
                            Console.WriteLine(tagstr);




                            Console.WriteLine("Description: ");
                            string description = Console.ReadLine();
                            Console.WriteLine("Address: ");
                            string address = Console.ReadLine();
                            double lowlimit, highlimit, initvalue;
                            try
                            {
                                Console.WriteLine("Initial value: ");
                                initvalue = Convert.ToDouble(Console.ReadLine());
                                Console.WriteLine("Low limit: ");
                                lowlimit = Convert.ToDouble(Console.ReadLine());
                                Console.WriteLine("High limit: ");
                                highlimit = Convert.ToDouble(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Bad argument input. Limits and initial value are double.\n");
                                break;
                            }
                            dbm.addUpdateAO("update", id, description, address, initvalue, lowlimit, highlimit);
                            
                            break;
                        }
                    case 3: Console.WriteLine(dbm.getAOs()); Console.ReadLine(); break;
                    case 4:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();
                            string tagstr = dbm.getStrAO(id);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }
                            Console.WriteLine(tagstr);

                            double initvalue;
                            try
                            {
                                Console.WriteLine("Initial value: ");
                                initvalue = Convert.ToDouble(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Bad argument input. Initial value is double.\n");
                                break;
                            }

                            dbm.changeValueAO(id, initvalue);

                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();

                            string tagstr = dbm.getStrAO(id);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }

                            dbm.deleteAO(id);


                            break;
                        }
                    case 6: return;

                }
            }
        }
        static void DIMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n---Manage DI---");
                Console.WriteLine("1. Add tag");
                Console.WriteLine("2. Edit tag");
                Console.WriteLine("3. Turn scan on/off");
                Console.WriteLine("4. Get tags");
                Console.WriteLine("5. Remove tag");
                Console.WriteLine("6. Back to Main Menu");
                int option = 0;
                while (true)
                {
                    try
                    {
                        Console.WriteLine("\nOption number:  ");
                        option = Convert.ToInt32(Console.ReadLine());
                        if (option > 0 && option <= 6)
                            break;
                        else
                            Console.WriteLine("Bad input.\n");
                    }
                    catch
                    {
                        Console.WriteLine("Bad input.\n");
                    }

                }
                switch (option)
                {
                    case 1:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();
                            Console.WriteLine("Description: ");
                            string description = Console.ReadLine();
                            Console.WriteLine("Address: ");
                            string address = Console.ReadLine();
                            Console.WriteLine("Driver: ");
                            string driver = Console.ReadLine();
                            int scantime;
                            bool scanonoff;
                            try
                            {
                                Console.WriteLine("Scan time: ");
                                scantime = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("On/Off scan (true = \"ON\"    false = \"OFF\"): ");
                                scanonoff = Convert.ToBoolean(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Bad argument input. Scan time is int and on/off scan is bool.\n");
                                break;
                            }
                            dbm.addUpdateDI("add", id, description, address, scantime, scanonoff, driver);
                            //alarms
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();
                            string tagstr = dbm.getStrDI(id);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }
                            Console.WriteLine(tagstr);



                            Console.WriteLine("Description: ");
                            string description = Console.ReadLine();
                            Console.WriteLine("Address: ");
                            string address = Console.ReadLine();
                            Console.WriteLine("Driver: ");
                            string driver = Console.ReadLine();
                            int scantime;
                            bool scanonoff;
                            try
                            {
                                Console.WriteLine("Scan time: ");
                                scantime = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("On/Off scan (true = \"ON\"    false = \"OFF\"): ");
                                scanonoff = Convert.ToBoolean(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("Bad argument input. Scan time is int and on/off scan is bool.\n");
                                Console.ReadLine();
                                break;
                            }

                            dbm.addUpdateDI("update", id, description, address, scantime, scanonoff, driver);
                            //alarms
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();
                            string tagstr = dbm.getStrDI(id);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }
                            Console.WriteLine(tagstr);

                            Console.WriteLine("On/Off scan (true = \"ON\"    false = \"OFF\"): ");
                            bool scanonoff = Convert.ToBoolean(Console.ReadLine());
                            dbm.TurnScanOnOff("DI", id, scanonoff);
                            break;
                        }
                    case 4: Console.WriteLine(dbm.getDIs()); Console.ReadLine(); break;
                    case 5:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();

                            string tagstr = dbm.getStrDI(id);
                            if (tagstr == null)
                            {
                                Console.WriteLine("Tag not found.\n");
                                Console.ReadLine();
                                break;
                            }

                            dbm.deleteDI(id);


                            break;
                        }
                    case 6: return;

                }
            }

        }
        static void DOMenu()
        {
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("\n---Manage DO---");
                    Console.WriteLine("1. Add tag");
                    Console.WriteLine("2. Edit tag");
                    Console.WriteLine("3. Get tags");
                    Console.WriteLine("4. Set value");
                    Console.WriteLine("5. Remove tag");
                    Console.WriteLine("6. Back to Main Menu");
                    int option = 0;
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("\nOption number:  ");
                            option = Convert.ToInt32(Console.ReadLine());
                            if (option > 0 && option <= 6)
                                break;
                            else
                                Console.WriteLine("Bad input.\n");
                        }
                        catch
                        {
                            Console.WriteLine("Bad input.\n");
                        }

                    }
                    switch (option)
                    {
                        case 1:
                            {
                                Console.WriteLine("Id: ");
                                string id = Console.ReadLine();
                                Console.WriteLine("Description: ");
                                string description = Console.ReadLine();
                                Console.WriteLine("Address: ");
                                string address = Console.ReadLine();
                                double initvalue;
                                try
                                {
                                    Console.WriteLine("Initial value: ");
                                    initvalue = Convert.ToDouble(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("Bad argument input. Initial value is int.\n");
                                    break;
                                }
                                dbm.addUpdateDO("add", id, description, address, initvalue);
                                
                                break;
                            }
                        case 2:
                            {
                                Console.WriteLine("Id: ");
                                string id = Console.ReadLine();
                                string tagstr = dbm.getStrDO(id);
                                if (tagstr == null)
                                {
                                    Console.WriteLine("Tag not found.\n");
                                    Console.ReadLine();
                                    break;
                                }
                                Console.WriteLine(tagstr);



                                Console.WriteLine("Description: ");
                                string description = Console.ReadLine();
                                Console.WriteLine("Address: ");
                                string address = Console.ReadLine();
                                double initvalue;
                                try
                                {
                                    Console.WriteLine("Initial value: ");
                                    initvalue = Convert.ToDouble(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("Bad argument input. Initial value is double.\n");
                                    break;
                                }
                                dbm.addUpdateDO("update", id, description, address, initvalue);
                                
                                break;
                            }
                        case 3: Console.WriteLine(dbm.getDOs()); Console.ReadLine(); break;
                        case 4:
                            {
                                Console.WriteLine("Id: ");
                                string id = Console.ReadLine();
                                string tagstr = dbm.getStrDO(id);
                                if (tagstr == null)
                                {
                                    Console.WriteLine("Tag not found.\n");
                                    Console.ReadLine();
                                    break;
                                }
                                Console.WriteLine(tagstr);

                                double initvalue;
                                try
                                {
                                    Console.WriteLine("Initial value: ");
                                    initvalue = Convert.ToDouble(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("Bad argument input. Initial value is double.\n");
                                    break;
                                }

                                dbm.changeValueDO(id, initvalue);

                                break;
                            }
                        case 5:
                            {
                                Console.WriteLine("Id: ");
                                string id = Console.ReadLine();

                                string tagstr = dbm.getStrDO(id);
                                if (tagstr == null)
                                {
                                    Console.WriteLine("Tag not found.\n");
                                    Console.ReadLine();
                                    break;
                                }

                                dbm.deleteDO(id);


                                break;
                            }
                        case 6: return;

                    }
                }

            }
        }
        static void registerUser()
        {
            while (true)
            {
                Console.WriteLine("New user username:");
                string username = Console.ReadLine();
                Console.WriteLine("New user password:");
                string password = Console.ReadLine();
                Console.WriteLine("New user role: 1 - REGULAR\t 2 - ADMIN\n");
                string userRole = "REGULAR";
                try
                {
                    int roleNum = Convert.ToInt32(Console.ReadLine());
                    if (roleNum == 1)
                        userRole = "REGULAR";
                    else if (roleNum == 2)
                        userRole = "ADMIN";
                    else
                    {
                        Console.WriteLine("Bad input. Valid input is 1 or 2");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Bad input. Valid input is 1 or 2");
                    continue;
                }

                bool succes = dbm.register(username, password, userRole);
                if (succes)
                {
                    Console.WriteLine("User registration succesful.");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("Failed to register user. This username already exists in database.");
                Console.ReadLine();

            }
        }
    }
}
