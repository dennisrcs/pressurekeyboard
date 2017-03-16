using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Model;

namespace DataSplitter
{
    // keystroke data splliter
    public class KeystrokesDataSplitter : IDataSplitter
    {
        // members
        private string _root;

        // constructor
        public KeystrokesDataSplitter(string root)
        {
            this._root = root;
        }

        // split content into appropriate keystroke files
        public void Split(TaskInfo taskInfo, string[] content)
        {
            int j = 0;
            while (j < taskInfo.TaskOrder.Count)
            {
                // setting up output file
                List<string> task_lines = new List<string>();
                string dest_path = Path.Combine(_root, "Parsed", "Keystrokes", "Task" + taskInfo.TaskOrder[j],
                                                "Participant" + taskInfo.ParticipantId + ".txt");

                // getting task's start and end time
                DateTime start_time = taskInfo.TaskStart[j];
                DateTime end_time = taskInfo.TaskEnd[j];

                // printing keystrokes collected during the task interval to a file
                for (int i = 0; i < content.Length; i++)
                {
                    Keystroke keystroke = new Keystroke(content[i]);
                    
                    if (keystroke.Timestamp < start_time)
                        continue;
                    else
                        if (keystroke.Timestamp < end_time)
                            task_lines.Add(content[i]);
                        else
                        {
                            j += 1;
                            break;
                        }                               
                }

                // printing to file
                File.WriteAllLines(dest_path, task_lines);
            }
        }
    }
}
