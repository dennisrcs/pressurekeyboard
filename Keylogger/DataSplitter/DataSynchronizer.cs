using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAnalyzer.Model;
using DataAnalyzer;
using System.Globalization;

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
                string dest_path = Path.Combine(_root, @"..\..\", "Parsed", taskInfo.ParticipantId, "Pressure", "Task" + taskInfo.TaskOrder[j],
                                                "session" + taskInfo.SessionId + ".txt");
                
                // get data from current task only
                List<string> keystrokeTaskLines = _getDataFromTask(keystrokesContent, j, taskInfo);
                List<string> pressureKeystroke = new List<string>();

                for (int i = 0; i < keystrokeTaskLines.Count; i++)
                {
                    Keystroke keystroke = new Keystroke(keystrokeTaskLines[i], i);
                    DateTime timestamp = keystroke.Timestamp;

                    // FIX THAT WHEN NEEDED
                    PressureIndex keystrokePressure = null;
                    //PressureIndex keystrokePressure = _getKeystrokePressure(pressureContent, timestamp);
                    //keystrokeTaskLines[i] = keystrokeTaskLines[i] + ";" + keystrokePressure;
                    if (!keystroke.IsKeyUp)
                        pressureKeystroke.Add(keystrokePressure.Pressure);
                }

                // printing to file
                File.WriteAllLines(dest_path, pressureKeystroke);
            }
        }

        // merges the data streams (keystrokes and pressures)
        public void MergeMax(string[] pressureContent, List<List<double>> norms, string[] keystrokesContent, TaskInfo taskInfo)
        {
            int indexClosestCurrent = 0;
            int indexClosestNext = 0;

            NearestSensorRepo repo = new NearestSensorRepo();

            for (int j = 0; j < taskInfo.TaskOrder.Count; j++)
            {
                // setting up output files
                string dest_path = Path.Combine(_root, @"..\..\", "Parsed", taskInfo.ParticipantId, "Pressure", "Task" + taskInfo.TaskOrder[j],
                                                "session" + taskInfo.SessionId + ".txt");
                
                // get data from current task only
                List<string> keystrokeTaskLines = _getDataFromTask(keystrokesContent, j, taskInfo);
                List<string> pressureKeystroke = new List<string>();

                // The second line in the pressure file contains the datetime's info session
                DateTime sessionDate = DateTime.Parse(pressureContent[1]);

                indexClosestCurrent = 0;
                for (int i = 0; i < keystrokeTaskLines.Count - 1; i++)
                {
                    Keystroke keystroke = new Keystroke(keystrokeTaskLines[i], i, sessionDate);
                    if (!keystroke.IsKeyUp)
                    {
                        Keystroke next_keystroke = null;
                        for (int k = i+1; k < keystrokeTaskLines.Count - 1; k++)
                        {
                            next_keystroke = new Keystroke(keystrokeTaskLines[k], k, sessionDate);
                            if (!next_keystroke.IsKeyUp)
                                break;
                        }

                        if (next_keystroke != null)
                        {
                            DateTime timestamp = keystroke.Timestamp;
                            DateTime timestamp_next = next_keystroke.Timestamp;

                            indexClosestCurrent = _getClosestIndex(pressureContent, timestamp, indexClosestCurrent);
                            indexClosestNext = _getClosestIndex(pressureContent, timestamp_next, indexClosestCurrent);

                            //PressureIndex keystrokePressure = _getMaxPressureFromNearestSensor(pressureContent, keystroke, repo, norms, indexClosestCurrent, indexClosestNext);
                            //PressureIndex keystrokePressure = _getDiffMaxMinValueInterval(pressureContent, norms[1].ToArray(), indexClosestCurrent, indexClosestNext);
                            PressureIndex keystrokePressure = _getMaximumValueInterval(pressureContent, norms[1].ToArray(), indexClosestCurrent, indexClosestNext);
                            //PressureIndex keystrokePressure = _getAverageValueInterval(pressureContent, norms[1].ToArray(), indexClosestCurrent, indexClosestNext);

                            //keystrokeTaskLines[i] = keystrokeTaskLines[i] + ";" + keystrokePressure;
                            pressureKeystroke.Add(keystrokePressure.Pressure);
                        }
                    }
                }

                // printing to file
                File.WriteAllLines(dest_path, pressureKeystroke);
            }
        }

        private PressureIndex _getMaxPressureFromNearestSensor(string[] pressureContent, Keystroke key, NearestSensorRepo repo, List<List<double>> norms, int indexClosestCurrent, int indexClosestNext)
        {
            int nearestIndex = repo.getNearestSensorFromKey(key.Character);
            PressureIndex keystrokePressure = _getMaximumValueInterval(pressureContent, norms[nearestIndex].ToArray(), indexClosestCurrent, indexClosestNext);

            return keystrokePressure;
        }

        private PressureIndex _getDiffMaxMinValueInterval(string[] pressureContent, double[] norms, int indexClosestCurrent, int indexClosestNext)
        {
            PressureIndex result = null;
            double max = Double.MinValue;
            double min = Double.MaxValue;

            int finalIndex = -1;

            for (int i = indexClosestCurrent; i <= indexClosestNext; i++)
            {
                double norm = norms[i];
                if (norm > max)
                {
                    max = norm;
                    finalIndex = i;
                }

                if (norm < min)
                    min = norm;
            }

            string finalPressureContent = pressureContent[finalIndex];
            string[] finalPressureElems = finalPressureContent.Split('-');
            //result = new PressureIndex(finalPressureElems[finalPressureElems.Length-1], finalIndex);

            // returning the max norm instead of the four pressure values
            result = new PressureIndex((max-min) + "", finalIndex);
            return result;
        }

        public static List<string> FromDoubleArraytoList(double[] norms)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < norms.Length; i++)
                result.Add(norms[i] + "");

            return result;
        }

        // returns an array containing the pressure norm
        public static List<List<double>> ComputeNorm(string[] pressureContent)
        {
            List<double> averages = new List<double>();
            double[] norms = new double[pressureContent.Length]; norms[0] = norms[1] = 0;

            // Initializing allValues array
            List<List<double>> allValues = new List<List<double>>();
            for (int i = 0; i < Constants.NUM_SENSORS; i++)
                allValues.Add(new List<double>());

            // Retrieving all values
            for (int i = 2; i < pressureContent.Length; i++)
            {
                double[] pressureValues = _parsePressure(pressureContent[i]);
                for (int j = 0; j < pressureValues.Length; j++)
                    allValues[j].Add(pressureValues[j]);
            }

            // Compute each sensor's average value
            for (int i = 0; i < Constants.NUM_SENSORS; i++)
                averages.Add(allValues[i].Average());

            // Computing moving average
            int windowSize = 500;
            List<List<double>> allValues_movingAverage = MathUtils.ComputeMovingAverage(allValues, windowSize);

            int maxStdDevIndex = _FindIndexMaxStandDeviation(allValues, averages);
            maxStdDevIndex = 1;

            // Centering values
            List<List<double>> allValues_centered = MathUtils.SubtractMovingAverage(allValues, allValues_movingAverage, Constants.NUM_SENSORS);

            /*
            // Subtracting mean and computing norm of each pressure value
            for (int i = 2; i < allValues[0].Count; i++)
            {
                double sumAux = 0;
                for (int j = 0; j < allValues.Count; j++) // most likely allValues.Count == 4
                    sumAux += Math.Pow(allValues[j][i] - averages[j], 2);
                norms[i] = Math.Sqrt(sumAux);
            }


            for (int i = 0; i < allValues[maxStdDevIndex].Count; i++)
                norms[i] = allValues[maxStdDevIndex][i] - averages[maxStdDevIndex]; 
            */

            return allValues_centered;
        }

        // Returns array's index with highest standard deviation
        private static int _FindIndexMaxStandDeviation(List<List<double>> allValues, List<double> averages)
        {
            int result = -1;
            double[] stdDevs = new double[Constants.NUM_SENSORS];

            // Computing standard deviation
            for (int i = 0; i < stdDevs.Length; i++)
            {
                double avg = averages[i];
                double sum = allValues[i].Sum(d => Math.Pow(d - avg, 2));
                stdDevs[i] = Math.Sqrt((sum) / (allValues[i].Count() - 1));
            }

            // Find index with maximum standard deviation
            double maxValue = stdDevs.Max();
            result = stdDevs.ToList().IndexOf(maxValue);

            return result;
        }

        // receives a string (line from file) parses pressures and returns array containing pressure values
        private static double[] _parsePressure(string currentPressure)
        {
            double[] result = new double[Constants.NUM_SENSORS];

            // splitting value and returning pressure only
            string[] pressureElems = currentPressure.Split('-');
            string pressureValueOnly = pressureElems[pressureElems.Length - 1];
            string[] values_str = pressureValueOnly.Split(';');

            // transforming string array into double array
            for (int i = 0; i < values_str.Length; i++)
                result[i] = Double.Parse(values_str[i]);

            return result;
        }

        // Returns the maximum norm within the interval passed as argument
        private PressureIndex _getMaximumValueInterval(string[] pressureContent, double[] norms, int indexClosestCurrent, int indexClosestNext)
        {
            PressureIndex result = null;
            double max = Double.MinValue;
            int finalIndex = -1;

            for (int i = indexClosestCurrent; i <= indexClosestNext; i++)
            {
                double norm = norms[i];
                if (norm > max)
                {
                    max = norm;
                    finalIndex = i;
                }
            }

            string finalPressureContent = pressureContent[finalIndex];
            string[] finalPressureElems = finalPressureContent.Split('-');
            //result = new PressureIndex(finalPressureElems[finalPressureElems.Length-1], finalIndex);

            // returning the max norm instead of the four pressure values
            result = new PressureIndex(max + "", finalIndex);
            return result;
        }

        // Returns the maximum norm within the interval passed as argument
        private PressureIndex _getAverageValueInterval(string[] pressureContent, double[] norms, int indexClosestCurrent, int indexClosestNext)
        {
            PressureIndex result = null;
            
            int size = (indexClosestNext - indexClosestCurrent) + 1;
            double[] smallerArray = new double[size];
            Array.Copy(norms, indexClosestCurrent, smallerArray, 0, size);
            
            // returning the average norm
            result = new PressureIndex(smallerArray.Average() + "", indexClosestNext);
            return result;
        }

        // returns the closest index for the corresponding keystroke timestamp
        private int _getClosestIndex(string[] pressureContent, DateTime timestamp, int startingIndex)
        {
            int resultIndex = -1;

            startingIndex = (startingIndex < 2) ? 2 : startingIndex;
            for (int i = startingIndex; i < pressureContent.Length; i++)
            {
                string current = pressureContent[i];
                string[] currentElems = current.Split('-');
                string pressureTimeString = currentElems[0] + "-" + currentElems[1] + "-" + currentElems[2];

                DateTime pressureTime = DateTime.ParseExact(pressureTimeString, "yyyy-MM-dd HH:mm:ss.fff", new CultureInfo("en-US"));

                if (timestamp > pressureTime)
                    continue;
                else
                {
                    //resultIndex = i - 1;
                    resultIndex = i - 4;
                    break;
                }
            }

            return resultIndex;
        }

        // Merges entire pressure stream with the pressures sampled according to the keystrokes
        public void MergePressure(string[] pressure_content, string[] keystrokes_content, int sessionId, string participant_id)
        {
            // setting up output file
            string dest_path = Path.Combine(_root, "PressureSampled", "session" + (sessionId + 1) + ".txt");

            List<PressureIndex> pressureKeystroke = new List<PressureIndex>();

            DateTime sessionDate = DateTime.Parse(pressure_content[1]);
            int index = 0;
            for (int i = 0; i < keystrokes_content.Length; i++)
            {
                Keystroke keystroke = new Keystroke(keystrokes_content[i], i, sessionDate);
                DateTime timestamp = keystroke.Timestamp;

                // getting closest pressure
                index = _getClosestIndex(pressure_content, timestamp, index);
                PressureIndex keystrokePressure = new PressureIndex(pressure_content[index], index);
                keystrokePressure.Pressure = keystrokes_content[i] + ";" + keystrokePressure.Pressure;

                pressureKeystroke.Add(keystrokePressure);
            }

            string[] pressureSampled = new string[pressure_content.Length];
            for (int i = 0; i < pressure_content.Length; i++)
                pressureSampled[i] = "0;0;0;0";                

            for (int i = 0; i < pressureKeystroke.Count; i++)
            {
                index = pressureKeystroke[i].Index;
                pressureSampled[index] = pressureKeystroke[i].Pressure;
            }

            // printing to file
            File.WriteAllLines(dest_path, pressureSampled);
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
                Keystroke keystroke = new Keystroke(keystrokesContent[i], i, end_time);

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
