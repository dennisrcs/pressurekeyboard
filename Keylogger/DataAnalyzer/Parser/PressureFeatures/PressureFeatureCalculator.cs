using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Model;

namespace DataAnalyzer.Parser.PressureFeatures
{
    // Pressure feature calculator
    public class PressureFeatureCalculator
    {
        public double[] CalculateFeatures(List<FRS> pressures)
        {
            // variables
            int num_pressures = pressures.Count;
            double[] means = new double[Constants.NUM_SENSORS];
            double[] stddevs = new double[Constants.NUM_SENSORS];
            double[] mins = new double[Constants.NUM_SENSORS];
            double[] maxs = new double[Constants.NUM_SENSORS];
            double[] pecs = new double[Constants.NUM_SENSORS];
            double[] necs = new double[Constants.NUM_SENSORS];

            // creating instance of pressure feature extractor
            IPressureFeatureExtractor feature_extractor = new PressureFeatureExtractor();

            // feature vector
            List<double> features = new List<double>();
            
            // Initializing lists
            List<List<double>> pressures_sensor = new List<List<double>>();
            List<List<double>> norm_pressures_sensor = new List<List<double>>();
            for (int j = 0; j < Constants.NUM_SENSORS; j++)
            {
                pressures_sensor.Add(new List<double>());
                norm_pressures_sensor.Add(new List<double>());
            }
                
            // loading pressure data into arrays indexed by sensor number
            for (int i = 0; i < num_pressures; i++)
                for (int j = 0; j < Constants.NUM_SENSORS; j++)
                    pressures_sensor[j].Add(pressures[i].Values[j]);

            // calculating mean and standard deviation
            for (int i = 0; i < Constants.NUM_SENSORS; i++)
            {
                means[i] = feature_extractor.CalculateMean(pressures_sensor[i]);
                stddevs[i] = feature_extractor.CalculateStdDev(pressures_sensor[i]);
            }

            // normalizing data
            for (int i = 0; i < Constants.NUM_SENSORS; i++)
                for (int j = 0; i < num_pressures; i++)
                    norm_pressures_sensor.Add(feature_extractor.NormalizeData(pressures_sensor[i]));

            // calculating min, max, positive center energy, and negative center energy
            for (int i = 0; i < Constants.NUM_SENSORS; i++)
            {
                mins[i] = feature_extractor.FindMinValue(norm_pressures_sensor[i]);
                maxs[i] = feature_extractor.FindMaxValue(norm_pressures_sensor[i]);
                pecs[i] = feature_extractor.CalculatePositiveEnergyCenter(norm_pressures_sensor[i]);
                necs[i] = feature_extractor.CalculateNegativeEnergyCenter(norm_pressures_sensor[i]);
            }

            // concatenating individual features per sensor (and forming feature vector)
            features.AddRange(means);
            features.AddRange(stddevs);
            features.AddRange(mins);
            features.AddRange(maxs);
            features.AddRange(pecs);
            features.AddRange(necs);

            return features.ToArray();
        }
    }
}
