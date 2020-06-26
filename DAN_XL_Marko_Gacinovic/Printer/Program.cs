using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Printer
{
    class Program
    {        
        static readonly object locker = new object();        
       
        static string[] formats = new string[] { "A3", "A4" };
        static string[] orientations = new string[] { "Portrait", "Landscape" };
        static string[] fileColors = File.ReadAllLines("../../Colors.txt");

        static List<string> users = new List<string>(10);
        
        static Random rnd = new Random();               

        static void Main(string[] args)
        {
            Colors();

            for (int i = 0; i < 10; i++)
            {
                Thread computer = new Thread(() => Print(Thread.CurrentThread.Name));

                computer.Name = "Computer_" + (i + 1);

                computer.Start();                
            }            

            Console.ReadLine();
        }

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

        static void Print(string name)
        { 
            while (users.Count < 10)
            {
                if (users.Count == 10)
                {
                    return;
                }
               
                Thread.Sleep(100);

                string color = fileColors[rnd.Next(fileColors.Length)];
                string format = formats[rnd.Next(2)];
                string orientation = orientations[rnd.Next(2)];

                Console.WriteLine("{0} send the request for printing document {1} format. Color: {2}. Orientation: {3}", name, format, color, orientation);

                Thread request = new Thread(() => Request(name, color, format, orientation));
                request.Start();                                
            }
        }

        static void Request(string threadName, string color, string format, string orientation)
        {
            if (format == "A3")
            {                
                lock (locker)
                {
                    if (users.Count == 10)
                    {
                        return;
                    }
                    Thread.Sleep(1000);

                    Console.WriteLine("\nDocument -> format: {0}, color: {1}, orientation: {2} is printed.", format, color, orientation);
                    Console.WriteLine("User of {0} can take over document {1} format.\n", threadName, format);

                    
                    if (!users.Contains(threadName))
                    {
                        users.Add(threadName);
                    }
                }
            }
            else
            {                
                lock (locker)
                {
                    if (users.Count == 10)
                    {
                        return;
                    }
                    Thread.Sleep(1000);

                    Console.WriteLine("\nDocument -> format: {0}, color: {1}, orientation: {2} is printed.", format, color, orientation);
                    Console.WriteLine("User of {0} can take over document {1} format.\n", threadName, format);

                    
                    if (!users.Contains(threadName))
                    {
                        users.Add(threadName);
                    }
                }
            }            
        }
    }
}
