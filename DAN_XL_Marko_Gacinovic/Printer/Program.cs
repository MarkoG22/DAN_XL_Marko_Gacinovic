using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Printer
{
    class Program
    {        
        static readonly AutoResetEvent auto = new AutoResetEvent(true);
        static readonly AutoResetEvent auto2 = new AutoResetEvent(true);

        static string[] formats = new string[] { "A3", "A4" };
        static string[] orientations = new string[] { "Portrait", "Landscape" };

        static Random rnd = new Random();               

        static void Main(string[] args)
        {
            Colors();

            for (int i = 0; i < 10; i++)
            {
                Thread computer = new Thread(() => Print(formats[rnd.Next(2)], orientations[rnd.Next(2)]));

                computer.Name = "Computer_" + (i + 1);

                computer.Start();
                Thread.Sleep(100);
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

        static void Print(string format, string orientation)
        {
            string color = null;
            

            if (format == "A3")
            {
                auto.WaitOne();                

                try
                {
                    string[] fileColors = File.ReadAllLines("../../Colors.txt");

                    color = fileColors[rnd.Next(fileColors.Length)];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("{0} send the request for printing document {1} format. Color: {2}. Orientation: {3}", Thread.CurrentThread.Name, format, color, orientation);

                Thread.Sleep(1000);

                Console.WriteLine("Document -> format: {0}, color: {1}, orientation: {2} is printed.", format, color, orientation);
                Console.WriteLine("\nUser of {0} can take over document {1} format.\n", Thread.CurrentThread.Name, format);

                auto.Set();
            }
            else
            {
                auto2.WaitOne();

                try
                {
                    string[] fileColors = File.ReadAllLines("../../Colors.txt");

                    color = fileColors[rnd.Next(fileColors.Length)];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("{0} send the request for printing document {1} format. Color: {2}. Orientation: {3}", Thread.CurrentThread.Name, format, color, orientation);

                Thread.Sleep(1000);

                Console.WriteLine("Document -> format: {0}, color: {1}, orientation: {2} is printed.", format, color, orientation);
                Console.WriteLine("\nUser of {0} can take over document {1} format.\n", Thread.CurrentThread.Name, format);

                auto2.Set();
            }            
        }
    }
}
