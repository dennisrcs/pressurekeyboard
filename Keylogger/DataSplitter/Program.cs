using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSplitter
{
    // class program
    class Program
    {
        // main method
        static void Main(string[] args)
        {
            // settings paths
            string data_dir = "C:/Users/Dennis/Source/Repos/pressurekeyboard/Keylogger/Data/Raw";
            string order_filepath = "C:/Users/Dennis/Source/Repos/pressurekeyboard/Keylogger/Data/Raw/orders.txt";

            System.Console.WriteLine("Splitting files...");

            // splits data into folders task1, task2, and task3
            DataHandler handler = new DataHandler(data_dir);
            handler.Split(order_filepath);

            System.Console.WriteLine("Files splitted in the Parsed folder in " + data_dir);
        }
    }
}
