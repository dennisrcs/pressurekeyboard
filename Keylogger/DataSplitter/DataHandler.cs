using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer;

namespace DataSplitter
{
    // handles raw data
    public class DataHandler
    {
        // members
        private string _root;

        // constructor
        public DataHandler(string root_dir)
        {
            this._root = root_dir;
        }

        // splits raw data into folders task1, task2..
        public void Split(string order_filepath)
        {
            // variables
            string[] lines = DataReader.ReadText(order_filepath);
            List<TaskInfo> tasks_info = _loadParticipantsTaskInfo(lines);

            // initializing splitters
            KeystrokesDataSplitter keystrokes_splitter = new KeystrokesDataSplitter(_root);
            PressureDataSplitter pressure_splitter = new PressureDataSplitter(_root);

            for (int i = 0; i < Constants.NUM_PARTICIPANTS; i++)
            {
                string[] pressure_content = DataReader.ReadText(Path.Combine(_root, "Pressure", "Participant" + (i+1)));
                string[] keystrokes_content = DataReader.ReadText(Path.Combine(_root, "Keystrokes", "Participant" + (i + 1)));

                keystrokes_splitter.Split(tasks_info[i], keystrokes_content);
                pressure_splitter.Split(tasks_info[i], pressure_content);
            }
        }

        // returns the task order for each participant
        private List<TaskInfo> _loadParticipantsTaskInfo(string[] lines)
        {
            List<TaskInfo> result = new List<TaskInfo>();

            if (lines.Length != Constants.NUM_PARTICIPANTS)
                throw new ArgumentOutOfRangeException("Lines in task info file should be the same as the number of NUM_PARTICIPANTS");

            for (int i = 0; i < lines.Length; i++)
            {
                TaskInfo task_info = new TaskInfo(i+1, lines[i]);
                result.Add(task_info);
            }

            return result;
        }
    }
}
