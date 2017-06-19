using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAnalyzer.Model;

namespace DataSplitter
{
    public class DataSynchronizer
    {
        // root directory
        private string _root;
        public string Root {
            get { return _root; }
        }

        // constructor
        public DataSynchronizer(string root)
        {
            this._root = root;
        }

        // merges the data streams (keystrokes and pressures)
        public void Merge(string[] pressureContent, string[] keystrokesContent, TaskInfo taskInfo)
        {
            for (int j = 0; j < taskInfo.TaskOrder.Count; j++)
            {
                // setting up output file
                string dest_path = Path.Combine(_root, @"..\", "Parsed", "KeystrokePressure", "Task" + taskInfo.TaskOrder[j],
                                                "Participant" + taskInfo.ParticipantId + ".txt");
                
                // get data from current task only
                List<string> keystrokeTaskLines = _getDataFromTask(keystrokesContent, j, taskInfo);
                List<string> pressureKeystroke = new List<string>();

                for (int i = 0; i < keystrokeTaskLines.Count; i++)
                {
                    Keystroke keystroke = new Keystroke(keystrokeTaskLines[i], i);
                    DateTime timestamp = keystroke.Timestamp;
                    PressureIndex keystrokePressure = _getKeystrokePressure(pressureContent, timestamp);
                    //keystrokeTaskLines[i] = keystrokeTaskLines[i] + ";" + keystrokePressure;
                    if (!keystroke.IsKeyUp)
                        pressureKeystroke.Add(keystrokePressure.Pressure);
                }

                // printing to file
                File.WriteAllLines(dest_path, pressureKeystroke);
            }
        }

        // merges the data streams (keystrokes and pressures)
        public void MergeMax(string[] pressureContent, string[] keystrokesContent, TaskInfo taskInfo)
        {
            for (int j = 0; j < taskInfo.TaskOrder.Count; j++)
            {
                // setting up output file
                string dest_path = Path.Combine(_root, @"..\", "Parsed", "KeystrokePressure", "Task" + taskInfo.TaskOrder[j],
                                                "Participant" + taskInfo.ParticipantId + ".txt");

                // get data from current task only
                List<string> keystrokeTaskLines = _getDataFromTask(keystrokesContent, j, taskInfo);
                List<string> pressureKeystroke = new List<string>();

                for (int i = 0; i < keystrokeTaskLines.Count - 1; i++)
                {
                    Keystroke keystroke = new Keystroke(keystrokeTaskLines[i], i);
                    if (!keystroke.IsKeyUp)
                    {
                        Keystroke next_keystroke = null;
                        for (int k = i+1; k < keystrokeTaskLines.Count - 1; k++)
                        {
                            next_keystroke = new Keystroke(keystrokeTaskLines[k], k);
                            if (!next_keystroke.IsKeyUp)
                                break;
                        }

                        if (next_keystroke != null)
                        {
                            DateTime timestamp = keystroke.Timestamp;
                            DateTime timestamp_next = next_keystroke.Timestamp;

                            int indexClosestCurrent = _getClosestIndex(pressureContent, timestamp);
                            int indexClosestNext = _getClosestIndex(pressureContent, timestamp);

                            PressureIndex keystrokePressure = _getMaximumValueInterval(pressureContent, indexClosestCurrent, indexClosestNext);

                            //keystrokeTaskLines[i] = keystrokeTaskLines[i] + ";" + keystrokePressure;
                            pressureKeystroke.Add(keystrokePressure.Pressure);
                        }
                    }
                }

                // printing to file
                File.WriteAllLines(dest_path, pressureKeystroke);
            }
        }

        // Returns the maximum norm within the interval passed as argument
        private PressureIndex _getMaximumValueInterval(string[] pressureContent, int indexClosestCurrent, int indexClosestNext)
        {
            PressureIndex result = null;
            double max = Double.MinValue;
            int finalIndex = -1;

            for (int i = indexClosestCurrent; i <= indexClosestNext; i++)
            {
                string currentPressure = pressureContent[i];
                string[] values_str = currentPressure.Split(';');
                double aux;

                double norm = 0;
                for (int j = 0; j < 4; j++)
                {
                    aux = Double.Parse(values_str[j]);
                    norm += Math.Pow(aux,2);
                }

                if (norm > max)
                {
                    max = norm;
                    finalIndex = i;
                }
            }

            result = new PressureIndex(pressureContent[finalIndex], finalIndex);
            return result;
        }

        // returns the closest index for the corresponding keystroke timestamp
        private int _getClosestIndex(string[] pressureContent, DateTime timestamp)
        {
            int resultIndex = -1;

            int previousTimeIndex = 0;
            int currentTimeIndex = 0;
            DateTime currentDateTime = DateTime.MinValue;
            DateTime previousDateTime = DateTime.MinValue;

            for (int i = 1; i < pressureContent.Length; i++)
            {
                string current = pressureContent[i];
                if (current.Contains("time"))
                {
                    string timestamp_str = current.Substring(5, current.Length - 5);
                    DateTime pressure_timestamp = DateTime.Parse(timestamp_str);

                    if (timestamp > pressure_timestamp)
                    {
                        previousTimeIndex = i;
                        previousDateTime = pressure_timestamp;
                        continue;
                    }
                    else
                    {
                        currentTimeIndex = i;
                        currentDateTime = pressure_timestamp;

                        for (int j = currentTimeIndex + 1; j < pressureContent.Length; j++)
                        {
                            current = pressureContent[j];
                            if (current.Contains("time"))
                            {
                                int sizeInterval = (j - 1) - currentTimeIndex;
                                string[] pressureInterval = new string[sizeInterval];
                                Array.Copy(pressureContent, currentTimeIndex + 1, pressureInterval, 0, sizeInterval);

                                TimeSpan diffPressureInterval = currentDateTime - previousDateTime;
                                TimeSpan diffKeystrokeInitPressureInterval = timestamp - previousDateTime;
                                double ratio = (double)diffKeystrokeInitPressureInterval.TotalMilliseconds / diffPressureInterval.TotalMilliseconds;
                                resultIndex = (int)Math.Round(pressureInterval.Length * ratio) - 1;
                                resultIndex = (resultIndex < 0) ? 0 : resultIndex;
                                resultIndex = (resultIndex == sizeInterval) ? resultIndex - 1 : resultIndex;

                                int finalIndex = currentTimeIndex + 1 + resultIndex;
                                return finalIndex;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        // Merges entire pressure stream with the pressures sampled according to the keystrokes
        public void MergePressure(string[] pressure_content, string[] keystrokes_content, int participantId)
        {
            // setting up output file
            string dest_path = Path.Combine(_root, "PressureSampled", "Participant" + (participantId + 1) + ".txt");

            // get data from current task only
            List<PressureIndex> pressureKeystroke = new List<PressureIndex>();
                
            for (int i = 0; i < keystrokes_content.Length; i++)
            {
                Keystroke keystroke = new Keystroke(keystrokes_content[i], i);
                DateTime timestamp = keystroke.Timestamp;
                PressureIndex keystrokePressure = _getKeystrokePressure(pressure_content, timestamp);
                keystrokePressure.Pressure = keystrokes_content[i] + ";" + keystrokePressure.Pressure;
                pressureKeystroke.Add(keystrokePressure);
            }

            string[] pressureSampled = new string[pressure_content.Length];
            for (int i = 0; i < pressure_content.Length; i++)
            {
                if (!pressure_content[i].Contains("time"))
                    pressureSampled[i] = "0;0;0;0";
                else
                    pressureSampled[i] = pressure_content[i];
            }
                

            for (int i = 0; i < pressureKeystroke.Count; i++)
            {
                int index = pressureKeystroke[i].Index;
                pressureSampled[index] = pressureKeystroke[i].Pressure;
            }

            // printing to file
            File.WriteAllLines(dest_path, pressureSampled);
        }

        private PressureIndex _getKeystrokePressure(string[] pressureContent, DateTime timestamp)
        {
            PressureIndex result = null;
            int resultIndex = -1;

            int previousTimeIndex = 0;
            int currentTimeIndex = 0;
            DateTime currentDateTime = DateTime.MinValue;
            DateTime previousDateTime = DateTime.MinValue;

            for (int i = 1; i < pressureContent.Length; i++)
            {
                string current = pressureContent[i];
                if (current.Contains("time"))
                {
                    string timestamp_str = current.Substring(5, current.Length - 5);
                    DateTime pressure_timestamp = DateTime.Parse(timestamp_str);

                    if (timestamp > pressure_timestamp)
                    {
                        previousTimeIndex = i;
                        previousDateTime = pressure_timestamp;
                        continue;
                    }
                    else
                    {
                        currentTimeIndex = i;
                        currentDateTime = pressure_timestamp;

                        for (int j = currentTimeIndex+1; j < pressureContent.Length; j++)
                        {
                            current = pressureContent[j];
                            if (current.Contains("time"))
                            {
                                int sizeInterval = (j - 1) - currentTimeIndex;
                                string[] pressureInterval = new string[sizeInterval];
                                Array.Copy(pressureContent, currentTimeIndex + 1, pressureInterval, 0, sizeInterval);
                                
                                TimeSpan diffPressureInterval = currentDateTime - previousDateTime;
                                TimeSpan diffKeystrokeInitPressureInterval = timestamp - previousDateTime;
                                double ratio = (double)diffKeystrokeInitPressureInterval.TotalMilliseconds/ diffPressureInterval.TotalMilliseconds;
                                //resultIndex = (int)Math.Round(pressureInterval.Length * ratio) - 1;
                                resultIndex = (int)Math.Round(pressureInterval.Length * ratio);
                                resultIndex = (resultIndex < 0) ? 0 : resultIndex;
                                resultIndex = (resultIndex == sizeInterval) ? resultIndex - 1 : resultIndex;

                                int finalIndex = currentTimeIndex + 1 + resultIndex;
                                result = new PressureIndex(pressureInterval[resultIndex], finalIndex);
                                return result;
                            }
                        }
                    }
                }
            }

            return result;
        }
        
        // returns the keystroke data for the corresponding task
        private List<string> _getDataFromTask(string[] keystrokesContent, int currentTask, TaskInfo taskInfo)
        {
            List<string> result = new List<string>();

            // getting task's start and end time
            DateTime start_time = taskInfo.TaskStart[currentTask];
            DateTime end_time = taskInfo.TaskEnd[currentTask];

            // printing keystrokes collected during the task interval to a file
            for (int i = 0; i < keystrokesContent.Length; i++)
            {
                Keystroke keystroke = new Keystroke(keystrokesContent[i], i);

                if (keystroke.Timestamp < start_time)
                    continue;
                else if (keystroke.Timestamp < end_time)
                    result.Add(keystrokesContent[i]);
                else
                    break;
            }

            return result;
        }
    }
}
