using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSplitter
{
    // splits pressure data into appropriate files
    public class PressureDataSplitter : IDataSplitter
    {
        // members
        private string _root;

        // constructor
        public PressureDataSplitter(string root)
        {
            this._root = root;
        }

        // splits content into appropriate files
        public void Split(TaskInfo taskInfo, string[] content, DateTime pressureDate)
        {
            // initializing variables
            int j = 0;
            bool is_after_startime = false;
            bool is_before_endtime = false;

            // run taskInfo.TaskOrder.Count times (3 for pressure-sensitive experiments)
            while (j < taskInfo.TaskOrder.Count)
            {
                // setting up output file
                List<string> task_lines = new List<string>();
                string dest_path = Path.Combine(_root, @"..\", "Parsed", "Pressure", "Task" + taskInfo.TaskOrder[j],
                                                "Participant" + taskInfo.SessionId + ".txt");

                // retrieving start and end time for each task
                DateTime start_time = taskInfo.TaskStart[j];
                DateTime end_time = taskInfo.TaskEnd[j];

                // iterate over content array
                for (int i = 0; i < content.Length; i++)
                {
                    string current = content[i];

                    // some lines in the pressure file have a timestamp in the following format:
                    // // ex: time:20170315122319036
                    if (current.Contains("time"))
                    {
                        string timestamp_str = current.Substring(5, current.Length-5);
                        DateTime timestamp = DateTime.Parse(timestamp_str);

                        // checking if the pressure's timestamp lies within the task interval
                        is_after_startime = (timestamp > start_time);
                        is_before_endtime = (timestamp < end_time);
                    }

                    // if start_time < timestamp < end_time, then read file
                    if (is_after_startime && is_before_endtime)
                    {
                        string[] pressures = current.Split(';');
                        if (pressures.Length == 4)
                            task_lines.Add(current);
                    }

                    // if startt_ime < end_time < timestamp, then stop reading (task is finished)
                    else if (is_after_startime && !is_before_endtime)
                    {
                        is_after_startime = is_before_endtime = false;
                        j = j + 1;
                        break;
                    }
                }

                // print lines to file
                File.WriteAllLines(dest_path, task_lines);
            }
        }
    }
}
