using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    class Program
    {
        static ServiceReference.ReportManagerServiceClient rm = new ServiceReference.ReportManagerServiceClient();
        
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n -----REPORTS MENU-----");
                Console.WriteLine("1. Alarms by date report");
                Console.WriteLine("2. Alarms by priority report");
                Console.WriteLine("3. All tags values report");
                Console.WriteLine("4. AI values report");
                Console.WriteLine("5. DI values report");
                Console.WriteLine("6. Tag by id values report");

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
                        Console.ReadLine();
                    }

                }
                switch (option)
                {
                    case 1:
                        {
                            try
                            {
                                Console.WriteLine("Start date (input numbers)");
                                Console.WriteLine("Day: ");
                                int d = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Month: ");
                                int m = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Year: ");
                                int y = Convert.ToInt32(Console.ReadLine());

                                Console.WriteLine("End date (input numbers)");
                                Console.WriteLine("Day: ");
                                int d2 = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Month: ");
                                int m2 = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Year: ");
                                int y2 = Convert.ToInt32(Console.ReadLine());

                                DateTime start = new DateTime(y, m, d);
                                DateTime end = new DateTime(y2, m2, d2);
                                Console.WriteLine(rm.getReport1(start, end));
                                Console.ReadKey();
                                break;

                            }
                            catch
                            {
                                Console.WriteLine("Bad date input.");
                                Console.ReadKey();
                                break;
                            }
                            
                        }
                    case 2:
                        {
                            try
                            {
                                int priority;
                                while (true)
                                {
                                    Console.WriteLine("Priority: (1, 2 or 3)");
                                    priority = Convert.ToInt32(Console.ReadLine());
                                    if (priority >= 1 && priority <= 3)
                                        break;
                                    Console.WriteLine("Bad input. Priority must be 1, 2 or 3.");
                                }
                                Console.WriteLine(rm.getReport2(priority)); 
                                Console.ReadKey(); 
                                break;

                            }
                            catch
                            {
                                Console.WriteLine("Bad input. Priority must be 1, 2 or 3.");
                                Console.ReadKey();
                                break;
                            }
                        }
                    case 3:
                        {

                            try
                            {
                                Console.WriteLine("Start date (input numbers)");
                                Console.WriteLine("Day: ");
                                int d = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Month: ");
                                int m = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Year: ");
                                int y = Convert.ToInt32(Console.ReadLine());

                                Console.WriteLine("End date (input numbers)");
                                Console.WriteLine("Day: ");
                                int d2 = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Month: ");
                                int m2 = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Year: ");
                                int y2 = Convert.ToInt32(Console.ReadLine());

                                DateTime start = new DateTime(y, m, d);
                                DateTime end = new DateTime(y2, m2, d2);
                                Console.WriteLine(rm.getReport3(start, end));
                                Console.ReadKey();
                                break;

                            }
                            catch
                            {
                                Console.WriteLine("Bad date input.");
                                Console.ReadKey();
                                break;
                            }
                        }
                    case 4: Console.WriteLine(rm.getReport4()); Console.ReadKey();break;
                    case 5: Console.WriteLine(rm.getReport5()); Console.ReadKey(); break;
                        
                    case 6:
                        {
                            Console.WriteLine("Id: ");
                            string id = Console.ReadLine();
                            Console.WriteLine(rm.getReport6(id)); 
                            Console.ReadKey();
                            break;
                        }
                }


            }
        }
    
    }
}
