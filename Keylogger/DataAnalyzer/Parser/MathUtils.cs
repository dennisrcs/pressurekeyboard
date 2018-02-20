using DataAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    public static class MathUtils
    {
        public static List<List<double>> ComputeMovingAverage(List<List<double>> allValues, int windowSize, int numSensors)
        {
            List<List<double>> result = new List<List<double>>();

            for (int i = 0; i < numSensors; i++)
            {
                result.Add(new List<double>());
                double[] valuesPerSensor = allValues[i].ToArray();
                double[] valuesMovingAverage = _MovingAveragePerSensor(valuesPerSensor, windowSize);
                result[i].AddRange(valuesMovingAverage);
            }

            return result;
        }

        private static double[] _MovingAveragePerSensor(double[] data, int period)
        {
            double[] buffer = new double[period];
            double[] output = new double[data.Length];
            int current_index = 0;

            for (int i = 0; i < data.Length; i++)
            {
                buffer[current_index] = data[i] / period;
                double ma = 0;
                for (int j = 0; j < period; j++)
                    ma += buffer[j];
                output[i] = ma;
                current_index = (current_index + 1) % period;
            }

            return output;
        }

        public static List<List<double>> SubtractMovingAverage(List<List<double>> allValues, List<List<double>> allValues_movingAverage, int numSensors)
        {
            List<List<double>> allValues_centered = new List<List<double>>();
            for (int i = 0; i < numSensors; i++)
            {
                allValues_centered.Add(new List<double>());

                List<double> valuesPerSensor = allValues[i];
                List<double> movingAveragePerSensor = allValues_movingAverage[i];

                for (int j = 0; j < valuesPerSensor.Count; j++)
                    allValues_centered[i].Add(valuesPerSensor[j] - movingAveragePerSensor[j]);
            }

            return allValues_centered;
        }
    }
}
