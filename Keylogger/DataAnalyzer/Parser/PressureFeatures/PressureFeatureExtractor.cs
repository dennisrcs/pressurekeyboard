using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Model;

namespace DataAnalyzer.Parser.PressureFeatures
{
    // Repo with pressure feature functions
    public class PressureFeatureExtractor : IPressureFeatureExtractor
    {
        // calculates mean value
        public double CalculateMean(List<double> pressures)
        {
            double result = 0;
            double sum = 0;

            for (int i = 0; i < pressures.Count; i++)
                sum += pressures[i];
            result = sum / pressures.Count;

            return result;
        }

        // calculates standard deviation
        public double CalculateStdDev(List<double> pressures)
        {
            double result = 0;
            double sum = 0;

            // calculating mean
            double mean = CalculateMean(pressures);
            
            for (int i = 0; i < pressures.Count; i++)
                sum += Math.Pow(pressures[i] - mean, 2);

            result = Math.Sqrt(sum / pressures.Count);
            
            return result;
        }

        // finds minimum value
        public double FindMinValue(List<double> pressures)
        {
            double min = Int32.MaxValue;
            int index = -1;

            for (int i = 0; i < pressures.Count; i++)
                if (pressures[i] < min)
                {
                    min = pressures[i];
                    index = i;
                }

            return min;
        }

        // finds maximum value
        public double FindMaxValue(List<double> pressures)
        {
            double max = Int32.MinValue;

            for (int i = 0; i < pressures.Count; i++)
                if (max < pressures[i])
                    max = pressures[i];

            return max;
        }

        // calculate positive energy center
        public double CalculatePositiveEnergyCenter(List<double> pressures)
        {
            double result = 0;
            double num = 0;
            double denum = 0;

            for (int i = 0; i < pressures.Count; i++)
            {
                if (pressures[i] > 0)
                {
                    num += i * Math.Pow(pressures[i], 2);
                    denum += Math.Pow(pressures[i], 2);
                }
            }

            result = num / denum;
            return result;
        }

        // calculate negative energy center
        public double CalculateNegativeEnergyCenter(List<double> pressures)
        {
            double result = 0;
            double num = 0;
            double denum = 0;

            for (int i = 0; i < pressures.Count; i++)
            {
                if (pressures[i] < 0)
                {
                    num += i * Math.Pow(pressures[i], 2);
                    denum += Math.Pow(pressures[i], 2);
                }
            }

            result = num / denum;
            return result;
        }

        // Normalize data
        public List<double> NormalizeData(List<double> pressures)
        {
            List<double> result = new List<double>();
            double mean = CalculateMean(pressures);
            double stddev = CalculateStdDev(pressures);

            for (int i = 0; i < pressures.Count; i++)
                result.Add((pressures[i] - mean) / stddev);

            return result;
        }
    }
}
