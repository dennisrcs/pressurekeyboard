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
        private string _participant_id;

        // constructor
        public DataHandler(string root_dir, string participant_id)
        {
            this._root = root_dir;
            this._participant_id = participant_id;
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

            for (int i = 0; i < Constants.NUM_SESSIONS; i++)
            {
                string[] pressure_content = DataReader.ReadText(Path.Combine(_root, "Pressure", "session" + (i + 1) + ".txt"));
                string[] keystrokes_content = DataReader.ReadText(Path.Combine(_root, "Keystrokes", "session" + (i + 1) + ".txt"));

                DateTime sessionDay = DateTime.Parse(pressure_content[1]);
                keystrokes_splitter.Split(tasks_info[i], keystrokes_content, sessionDay);
                
                // Pressures computed using the MergeMax method. Temporary comment, but probably will not be ever invoked anymore
                //pressure_splitter.Split(tasks_info[i], pressure_content);
            }
        }

        // Creates a file with all the pressures, including when there's no keystroke event
        public void MergeKeepAllPressures(string order_filepath)
        {
            string[] lines = DataReader.ReadText(order_filepath);
            List<TaskInfo> tasks_info = _loadParticipantsTaskInfo(lines);
            // variables
            for (int i = 0; i < Constants.NUM_SESSIONS; i++)
            {
                string[] pressure_content = DataReader.ReadText(Path.Combine(_root, "Pressure", "session" + (i + 1) + ".txt"));
                string[] keystrokes_content = DataReader.ReadText(Path.Combine(_root, "Keystrokes", "session" + (i + 1) + ".txt"));

                DataSynchronizer syncer = new DataSynchronizer(_root);
                syncer.MergePressure(pressure_content, keystrokes_content, i, tasks_info[i].ParticipantId);
            }
        }

        // Creates files with the next pressure after a keydown event
        public void Merge(string order_filepath)
        {
            // variables
            string[] lines = DataReader.ReadText(order_filepath);
            List<TaskInfo> tasks_info = _loadParticipantsTaskInfo(lines);
            
            for (int i = 0; i < Constants.NUM_SESSIONS; i++)
            {
                string[] pressure_content = DataReader.ReadText(Path.Combine(_root, "Pressure", "session" + (i + 1) + ".txt"));
                string[] keystrokes_content = DataReader.ReadText(Path.Combine(_root, "Keystrokes", "session" + (i + 1) + ".txt"));

                DataSynchronizer syncer = new DataSynchronizer(_root);
                syncer.Merge(pressure_content, keystrokes_content, tasks_info[i]);
            }
        }

        // Creates files with the maximum pressure between two keydown events
        public void MergeMax(string order_filepath)
        {
            // variables
            string[] lines = DataReader.ReadText(order_filepath);
            List<TaskInfo> tasks_info = _loadParticipantsTaskInfo(lines);

            for (int i = 0; i < Constants.NUM_SESSIONS; i++)
            {
                string[] pressure_content = DataReader.ReadText(Path.Combine(_root, "Pressure", "session" + (i + 1) + ".txt"));
                string[] keystrokes_content = DataReader.ReadText(Path.Combine(_root, "Keystrokes", "session" + (i + 1) + ".txt"));

                // Computing norm and saving it to disk
                string norm_dest_path = Path.Combine(_root, "PressureCentered", "session" + (i + 1) + ".txt");
                List<List<double>> centeredValues = DataSynchronizer.ComputeNorm(pressure_content);

                double[] norms = centeredValues[1].ToArray();
                List<string> norms_string = DataSynchronizer.FromDoubleArraytoList(norms);
                File.WriteAllLines(norm_dest_path, norms_string);

                DataSynchronizer syncer = new DataSynchronizer(_root);
                syncer.MergeMax(pressure_content, centeredValues, keystrokes_content, tasks_info[i]);
            }
        }

        // Removes unformatted pressure values from the dataset
        public void Preprocess()
        {
            for (int i = 0; i < Constants.NUM_SESSIONS; i++)
            {
                string path = Path.Combine(_root, "Pressure", "session" + (i + 1) + ".txt");
                string[] pressure_content = DataReader.ReadText(path);
                string[] content = Preprocessor.RemoveUnformattedPressuresAndFilter(pressure_content);

                File.WriteAllLines(path, content.ToArray());
            }
        }

        // returns the task order for each participant
        private List<TaskInfo> _loadParticipantsTaskInfo(string[] lines)
        {
            List<TaskInfo> result = new List<TaskInfo>();

            if (lines.Length != Constants.NUM_SESSIONS)
                throw new ArgumentOutOfRangeException("Lines in task info file should be the same as the number of NUM_SESSIONS");

            for (int i = 0; i < lines.Length; i++)
            {
                TaskInfo task_info = new TaskInfo(i+1, lines[i], this._participant_id);
                result.Add(task_info);
            }

            return result;
        }
    }
}
