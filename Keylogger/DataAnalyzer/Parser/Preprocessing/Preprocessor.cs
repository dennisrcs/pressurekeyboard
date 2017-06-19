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

        // constructor
        public Preprocessor(string root)
        {
            this._root = root;
        }

        // Computes the norm for each set of pressures and updates the file with them
        public void ComputeNorm()
        {
            for (int j = 0; j < Constants.NUM_PARTICIPANTS; j++)
            {
                for (int i = 0; i < Constants.NUM_TASKS; i++)
                {
                    string pressure_path = Path.Combine(_root, "KeystrokePressure", "Task" + (i + 1), "Participant" + (j + 1) + ".txt");
                    string[] lines = DataReader.ReadText(pressure_path);
                    List<string> contents = new List<string>();

                    for (int it = 0; it < lines.Length; it++)
                    {
                        string line = lines[it];
                        string[] values = line.Split(';');

                        Regex regex = new Regex(@"^\d+$");
                        bool valid = true;

                        if (values.Length == 4)
                        {
                            string newline;
                            Decimal dummy;
                            // validating
                            for (int k = 0; k < 4; k++)
                                if (!Decimal.TryParse(values[k], out dummy))
                                    valid &= false;

                            if (valid)
                            {
                                double val1 = Double.Parse(values[0]);
                                double val2 = Double.Parse(values[1]);
                                double val3 = Double.Parse(values[2]);
                                double val4 = Double.Parse(values[3]);
                                //double norm = Math.Sqrt(Math.Pow(val1, 2) + Math.Pow(val2, 2) + Math.Pow(val3, 2) + Math.Pow(val4, 2));

                                double[] temp_values = new double[4] { val1, val2, val3, val4 };
                                double norm = temp_values.Max();
                                newline = norm + "";
                                contents.Add(newline);
                            }
                        }
                    }
                    File.WriteAllLines(pressure_path, contents.ToArray());
                }
            }
        }

        // Filter the pressure stream with a mean value
        public void Filter(List<List<double>> averages)
        {
            for (int j = 0; j < Constants.NUM_PARTICIPANTS; j++)
            {
                for (int i = 0; i < Constants.NUM_TASKS; i++)
                {
                    string pressure_path = Path.Combine(_root, "KeystrokePressure", "Task" + (i + 1), "Participant" + (j + 1) + ".txt");
                    string[] lines = DataReader.ReadText(pressure_path);
                    string[] contents = new string[lines.Length];

                    double[] previous = new double[4];
                    previous[0] = averages[j][0];
                    previous[1] = averages[j][1];
                    previous[2] = averages[j][2];
                    previous[3] = averages[j][3];

                    for (int it = 0; it < lines.Length; it++)
                    {
                        string line = lines[it];
                        string[] values = line.Split(';');

                        Regex regex = new Regex(@"^\d+$");
                        bool valid = true;

                        if (values.Length == 4)
                        {
                            string newline;
                            // validating
                            for (int k = 0; k < 4; k++)
                                if (!regex.IsMatch(values[k]))
                                    valid &= false;

                            if (it == 0)
                            {
                                contents[it] = line;
                            }
                            else
                            {
                                // if valid
                                if (valid)
                                {
                                    double value1 = Int32.Parse(values[0]);
                                    double value2 = Int32.Parse(values[1]);
                                    double value3 = Int32.Parse(values[2]);
                                    double value4 = Int32.Parse(values[3]);

                                    if (Math.Abs(Int32.Parse(values[0]) - previous[0]) > 300)
                                        value1 = previous[0];

                                    if (Math.Abs(Int32.Parse(values[1]) - previous[1]) > 300)
                                        value2 = previous[1];

                                    if (Math.Abs(Int32.Parse(values[2]) - previous[2]) > 300)
                                        value3 = previous[2];

                                    if (Math.Abs(Int32.Parse(values[3]) - previous[3]) > 300)
                                        value4 = previous[3];

                                    newline = (value1) + ";" + (value2) + ";" + (value3) + ";" + (value4);
                                    contents[it] = newline;
                                }
                                else
                                    contents[it] = lines[it];
                            }
                        }
                        else
                            contents[it] = lines[it];

                        if (!valid)
                            contents[it] = lines[it];
                    }
                    System.IO.File.WriteAllLines(pressure_path, contents);
                }
            }
        }

        // Normalizes the dataset by subtracting the global mean
        public void Normalize(List<List<double>> averages)
        {
            for (int j = 0; j < Constants.NUM_PARTICIPANTS; j++)
            {
                for (int i = 0; i < Constants.NUM_TASKS; i++)
                {
                    string pressure_path = Path.Combine(_root, "KeystrokePressure", "Task" + (i + 1), "Participant" + (j + 1) + ".txt");
                    _normalize(pressure_path, averages[j]);
                }
            }
        }

        // Normalizes the participant's data by subtracting the global mean
        private void _normalize(string pressure_path, List<double> avgs)
        {
            string[] lines = DataReader.ReadText(pressure_path);
            string[] contents = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] values = line.Split(';');

                Regex regex = new Regex(@"^\d+$");
                bool valid = true;

                if (values.Length == Constants.NUM_SENSORS)
                {
                    Decimal dummy;
                    // validating
                    for (int j = 0; j < Constants.NUM_SENSORS; j++)
                        if (!Decimal.TryParse(values[j], out dummy))
                            valid &= false;

                    // if valid
                    if (valid)
                    {
                        string newline = (Double.Parse(values[0]) - avgs[0]) + ";" + (Double.Parse(values[1]) - avgs[1]) + ";" + (Double.Parse(values[2]) - avgs[2]) + ";" + (Double.Parse(values[3]) - avgs[3]);
                        //string newline = (Int32.Parse(values[0]) - avgs[0]) + "";
                        contents[i] = newline;
                    }
                }
                else
                    contents[i] = lines[i];

                if (!valid)
                    contents[i] = lines[i];
            }

            System.IO.File.WriteAllLines(pressure_path, contents);
        }

        // Computes the average pressure set for all participants
        public List<List<double>> ComputeAverages()
        {
            List<List<double>> averages = new List<List<double>>();

            for (int j = 0; j < Constants.NUM_PARTICIPANTS; j++)
            {
                List<Model.FRS> all_pressures = new List<Model.FRS>();
                for (int i = 0; i < Constants.NUM_TASKS; i++)
                {
                    string pressure_path = Path.Combine(_root, "KeystrokePressure", "Task" + (i + 1), "Participant" + (j + 1) + ".txt");
                    PressureFeaturesParser pressure_parser = new PressureFeaturesParser();
                    pressure_parser.Parse(pressure_path);
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

            for (int j = 0; j < 4; j++)
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
