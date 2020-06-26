using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Printer
{
    class Program
    {        
        // lockers for printing threads
        static readonly object locker = new object();
        static readonly object locker2 = new object();        
       
        // arrays for document's characteristics
        static string[] formats = new string[] { "A3", "A4" };
        static string[] orientations = new string[] { "Portrait", "Landscape" };
        static string[] fileColors = File.ReadAllLines("../../Colors.txt");

        // list of 10 thread names
        static List<string> users = new List<string>(10);
        
        static Random rnd = new Random();

        public delegate bool Finish();
        public static event Finish Finishing;

        static void Main(string[] args)
        {
            // writing colors to the file
            Colors();

            // loop for creating and starting threads for computers
            for (int i = 0; i < 10; i++)
            {
                Thread computer = new Thread(() => Print(Thread.CurrentThread.Name));

                computer.Name = "Computer_" + (i + 1);

                computer.Start();                
            }            

            Console.ReadLine();
        }

        /// <summary>
        /// method for writing colors to the file
        /// </summary>
        static void Colors()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("../../Colors.txt"))
                {
                    List<string> colors = new List<string>() { "Red", "White", "Blue", "Black", "Yellow" };

                    for (int i = 0; i < colors.Count; i++)
                    {
                        sw.WriteLine(colors[i]);
                    }  
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// method for sending requests
        /// </summary>
        /// <param name="name"></param>
        static void Print(string name)
        { 
            // loop for sending requests while each computer gets printed document
            while (users.Count < 10)
            {
                // finishing application work
                if (users.Count == 10)
                {
                    return;
                }
               
                Thread.Sleep(100);

                // random document's characteristics
                string color = fileColors[rnd.Next(fileColors.Length)];
                string format = formats[rnd.Next(2)];
                string orientation = orientations[rnd.Next(2)];

                Console.WriteLine("{0} send the request for printing document {1} format. Color: {2}. Orientation: {3}", name, format, color, orientation);

                // creating and starting threads for requests
                Thread request = new Thread(() => Request(name, color, format, orientation));
                request.Start();                                
            }
        }

        /// <summary>
        /// method for printing documents
        /// </summary>
        /// <param name="threadName"></param>
        /// <param name="color"></param>
        /// <param name="format"></param>
        /// <param name="orientation"></param>
        static void Request(string threadName, string color, string format, string orientation)
        {
            // checking document's format
            if (format == "A3")
            {     
                // lock for one thread printing
                lock (locker)
                {
                    // finishing application work
                    if (users.Count == 10)
                    {
                        return;
                    }
                    Thread.Sleep(1000);
                    
                    Console.WriteLine("\nDocument -> format: {0}, color: {1}, orientation: {2} is printed.", format, color, orientation);
                    Console.WriteLine("User of {0} can take over document {1} format.\n", threadName, format);

                    // adding thread name to the list to ensure that each thread printed at least once
                    if (!users.Contains(threadName))
                    {
                        users.Add(threadName);
                    }
                }
            }
            else
            {     
                // lock for one thread printing
                lock (locker2)
                {
                    // finishin aplication work when each thread printed at least once
                    if (users.Count == 10)
                    {
                        return;
                    }
                    Thread.Sleep(1000);

                    Console.WriteLine("\nDocument -> format: {0}, color: {1}, orientation: {2} is printed.", format, color, orientation);
                    Console.WriteLine("User of {0} can take over document {1} format.\n", threadName, format);

                    // adding threads name to the list to ensure that each thread printed at least once
                    if (!users.Contains(threadName))
                    {
                        users.Add(threadName);
                    }
                }
            }            
        }        
    }
}
