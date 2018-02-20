using DataAnalyzer;
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
            int num_participants = 9;
            for (int i = 0; i < num_participants; i++)
            {
                // settings paths
                string participant_id = "Participant" + (i+1);

                string data_dir = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\Raw\\" + participant_id;
                string order_filepath = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\Raw\\" + participant_id + "\\orders.txt";

                System.Console.WriteLine("Splitting files for participant " + i);

                // splits data into folders task1, task2, and task3
                DataHandler handler = new DataHandler(data_dir, participant_id);
                handler.MergeMax(order_filepath);
                handler.Split(order_filepath);
                handler.MergeKeepAllPressures(order_filepath);

                // System.Console.WriteLine("Files splitted for participant " + i + " in the Parsed folder in " + data_dir);
            }
        }
    }
}
