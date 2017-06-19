using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataSplitter
{
    public static class Preprocessor
    {
        // Filters the data by removing unformatted pressures
        public static string[] RemoveUnformattedPressuresAndFilter(string[] input)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < input.Length; i++)
            {
                string pressure = input[i];

                if (pressure.Contains("time"))
                    result.Add(pressure);
                else
                {
                    string[] values = pressure.Split(';');
                    Regex regex = new Regex(@"^\d+$");
                    bool valid = true;

                    if (values.Length == 4)
                    {
                        // validating
                        for (int j = 0; j < 4; j++)
                            if (values[j] == "")
                                valid &= false;

                        if (valid)
                            result.Add(pressure);
                    }
                }
            }

            result = _FilterPressures(result);

            return result.ToArray();
        }

        // Filter pressures by substituting apparent erroneus measures from the corresponding average value
        private static List<string> _FilterPressures(List<string> pressures)
        {
            List<string> result = new List<string>();
            double[] averages = _ComputeAverages(pressures);

            double value1;
            double value2;
            double value3;
            double value4;

            double previous1 = 0;
            double previous2 = 0;
            double previous3 = 0;
            double previous4 = 0;

            string newline;
            bool first = true;

            for (int i = 0; i < pressures.Count; i++)
            {
                string pressure = pressures[i];
                if (!pressure.Contains("time"))
                {
                    string[] values = pressure.Split(';');

                    value1 = Int32.Parse(values[0]);
                    value2 = Int32.Parse(values[1]);
                    value3 = Int32.Parse(values[2]);
                    value4 = Int32.Parse(values[3]);

                    if (first)
                    {
                        previous1 = value1;
                        previous2 = value2;
                        previous3 = value3;
                        previous4 = value4;
                        first = false;
                    }

                    if (Math.Abs(Int32.Parse(values[0]) - averages[0]) > 300)
                        value1 = previous1;

                    if (Math.Abs(Int32.Parse(values[1]) - averages[1]) > 300)
                        value2 = previous2;

                    if (Math.Abs(Int32.Parse(values[2]) - averages[2]) > 300)
                        value3 = previous3;

                    if (Math.Abs(Int32.Parse(values[3]) - averages[3]) > 300)
                        value4 = previous4;

                    newline = (value1) + ";" + (value2) + ";" + (value3) + ";" + (value4);
                    result.Add(newline);

                    previous1 = value1;
                    previous2 = value2;
                    previous3 = value3;
                    previous4 = value4;
                }
                else
                    result.Add(pressure);
            }
            
            return result;   
        }

        //Computes the average pressure for each sensor
        private static double[] _ComputeAverages(List<string> pressures)
        {
            double[] averages = new double[4];
            double[] sums = new double[4] { 0, 0, 0, 0 };

            for (int i = 0; i < pressures.Count; i++)
            {
                string pressure = pressures[i];
                if (!pressure.Contains("time"))
                {
                    string[] values = pressure.Split(';');
                    for (int j = 0; j < 4; j++)
                        sums[j] += Int32.Parse(values[j]);
                }
                
            }

            for (int i = 0; i < 4; i++)
                averages[i] = sums[i] / pressures.Count;

            return averages;
        }
    }
}
