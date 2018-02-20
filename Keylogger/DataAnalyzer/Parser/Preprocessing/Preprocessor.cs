using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAnalyzer.Model;
using DataAnalyzer.Parser.PressureFeatures;

namespace DataAnalyzer.Parser.Preprocessing
{
    // preprocessing methods: filtering, computing norm
    public class Preprocessor
    {
        // root directory
        private string _root;
        public string Root { get { return this._root; } }

        private string _participant_id;
        public string ParticipantId { get { return this._participant_id;  } }

        // constructor
        public Preprocessor(string root, string participant_id)
        {
            this._root = root;
            this._participant_id = participant_id;
        }

        // Computes the norm for each set of pressures and updates the file with them
        public void ComputeNorm()
        {
            for (int j = 0; j < Constants.NUM_SESSIONS; j++)
            {
                for (int i = 0; i < Constants.NUM_TASKS; i++)
                {
                    string output_path = Path.Combine(_root, _participant_id, "PressureNorm", "Task" + (i + 1), "session" + (j + 1) + ".txt");
                    string pressure_path = Path.Combine(_root, _participant_id, "PressureCentralized", "Task" + (i + 1), "session" + (j + 1) + ".txt");
                    string[] lines = DataReader.ReadText(pressure_path);
                    List<string> contents = new List<string>();

                    for (int it = 0; it < lines.Length; it++)
                    {
                        string line = lines[it];
                        string[] values = line.Split(';');

                        Regex regex = new Regex(@"^\d+$");
                        bool valid = true;

                        if (values.Length == Constants.NUM_SENSORS)
                        {
                            string newline;
                            Decimal dummy;
                            // validating
                            for (int k = 0; k < Constants.NUM_SENSORS; k++)
                                if (!Decimal.TryParse(values[k], out dummy))
                                    valid &= false;

                            if (valid)
                            {
                                double val1 = Double.Parse(values[0]);
                                double val2 = Double.Parse(values[1]);
                                double val3 = Double.Parse(values[2]);
                                double val4 = Double.Parse(values[3]);
                                double norm = Math.Sqrt(Math.Pow(val1, 2) + Math.Pow(val2, 2) + Math.Pow(val3, 2) + Math.Pow(val4, 2));

                                //double[] temp_values = new double[4] { val1, val2, val3, val4 };
                                //double norm = temp_values.Max();
                                newline = norm + "";
                                contents.Add(newline);
                            }
                        }
                    }
                    File.WriteAllLines(output_path, contents.ToArray());
                }
            }
        }

        internal void SubtractMovingAverage()
        {
            for (int j = 0; j < Constants.NUM_SESSIONS; j++)
            {
                for (int i = 0; i < Constants.NUM_TASKS; i++)
                {
                    string pressure_path = Path.Combine(_root, _participant_id, "Pressure", "Task" + (i + 1), "session" + (j + 1) + ".txt");
                    string output_path = Path.Combine(_root, _participant_id, "PressureCentralized", "Task" + (i + 1), "session" + (j + 1) + ".txt");
                    _subtractMovingAverage(pressure_path, output_path);
                }
            }
        }

        private void _subtractMovingAverage(string pressure_path, string output_path)
        {
            string[] pressureContent = DataReader.ReadText(pressure_path);
            // Initializing allValues array
            List<List<double>> allValues = new List<List<double>>();
            allValues.Add(new List<double>()); // value from 1 sensor only

            // Retrieving all values
            for (int i = 0; i < pressureContent.Length; i++)
            {
                double[] pressureValues = _parsePressure(pressureContent[i]);
                for (int j = 0; j < pressureValues.Length; j++)
                    allValues[j].Add(pressureValues[j]);
            }

            // Computing moving average
            int windowSize = 500;
            List<List<double>> allValues_movingAverage = MathUtils.ComputeMovingAverage(allValues, windowSize, 1);
            List<List<double>> allValues_centered = MathUtils.SubtractMovingAverage(allValues, allValues_movingAverage, 1);

            List<string> centeredContent = new List<string>();
            for (int i = 0; i < allValues_centered[0].Count; i++)
            {
                for (int j = 0; j < allValues_centered.Count; j++)
                {
                    if (j == 0)
                        centeredContent.Add(allValues_centered[j][i] + "");
                    else
                        centeredContent[j] = centeredContent[i] + ";" + allValues_centered[j][i];
                }
            }

            File.WriteAllLines(output_path, centeredContent.ToArray());
        }

        private double[] _parsePressure(string line)
        {
            double[] result = new double[1];
            result[0] = 0;
            if (Double.TryParse(line, out double output))
                result[0] = output;

            return result;
        }

        // Normalizes the dataset by subtracting the global mean
        public void SubtractMean(List<List<double>> averages)
        {
            for (int j = 0; j < Constants.NUM_SESSIONS; j++)
            {
                for (int i = 0; i < Constants.NUM_TASKS; i++)
                {
                    string pressure_path = Path.Combine(_root, _participant_id, "Pressure", "Task" + (i + 1), "session" + (j + 1) + ".txt");
                    string output_path = Path.Combine(_root, _participant_id, "PressureCentralized", "Task" + (i + 1), "session" + (j + 1) + ".txt");
                    _subtractMean(pressure_path, output_path, averages[j]);
                }
            }
        }

        // Normalizes the participant's data by subtracting the global mean
        private void _subtractMean(string pressure_path, string output_path, List<double> avgs)
        {
            string[] lines = DataReader.ReadText(pressure_path);
            string[] contents = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] values = line.Split(';');

                Regex regex = new Regex(@"^\d+$");
                bool valid = true;

                //if (values.Length == Constants.NUM_SENSORS)
                if (values.Length == 1)
                {
                    Decimal dummy;
                    // validating
                    //for (int j = 0; j < Constants.NUM_SENSORS; j++)
                    for (int j = 0; j < 1; j++)
                        if (!Decimal.TryParse(values[j], out dummy))
                            valid &= false;

                    // if valid
                    if (valid)
                    {
                        string newline = (Double.Parse(values[0]) - avgs[0]) + "";
                        //string newline = (Double.Parse(values[0]) - avgs[0]) + ";" + (Double.Parse(values[1]) - avgs[1]) + ";" + (Double.Parse(values[2]) - avgs[2]) + ";" + (Double.Parse(values[3]) - avgs[3]);
                        //string newline = (Int32.Parse(values[0]) - avgs[0]) + "";
                        contents[i] = newline;
                    }
                }
                else
                    contents[i] = lines[i];

                if (!valid)
                    contents[i] = lines[i];
            }

            File.WriteAllLines(output_path, contents);
        }

        // Computes the average pressure set for all participants
        public List<List<double>> ComputeAverages()
        {
            List<List<double>> averages = new List<List<double>>();

            for (int j = 0; j < Constants.NUM_SESSIONS; j++)
            {
                List<Model.FRS> all_pressures = new List<Model.FRS>();
                for (int i = 0; i < Constants.NUM_TASKS; i++)
                {
                    string pressure_path = Path.Combine(_root, _participant_id, "Pressure", "Task" + (i + 1), "session" + (j + 1) + ".txt");
                    PressureFeaturesParser pressure_parser = new PressureFeaturesParser();

                    //pressure_parser.Parse(pressure_path, false);
                    pressure_parser.Parse(pressure_path, true);

                    all_pressures.AddRange(pressure_parser.Pressures);
                }
                List<double> avg = _compute_average(all_pressures);
                averages.Add(avg);
            }

            return averages;
        }

        // Computes the mean pressure for each sensor
        private List<double> _compute_average(List<FRS> all_pressures)
        {
            List<double> res = new List<double>();

            //for (int j = 0; j < Constants.NUM_SENSORS; j++)
            for (int j = 0; j < 1; j++)
            {
                double sum = 0;
                for (int i = 0; i < all_pressures.Count; i++)
                    sum += all_pressures[i].Values[j];
                res.Add(sum / all_pressures.Count);
            }

            return res;
        }
    }
}
