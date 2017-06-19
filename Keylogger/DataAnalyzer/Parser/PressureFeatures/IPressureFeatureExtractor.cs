using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Parser.PressureFeatures
{
    public interface IPressureFeatureExtractor
    {
        // calculates mean value
        double CalculateMean(List<double> pressures);

        // calculates standard deviation
        double CalculateStdDev(List<double> pressures);

        // finds minimum value
        double FindMinValue(List<double> pressures);

        // finds maximum value
        double FindMaxValue(List<double> pressures);

        // calculate positive energy center
        double CalculatePositiveEnergyCenter(List<double> pressures);

        // calculate negative energy center
        double CalculateNegativeEnergyCenter(List<double> pressures);

        // Normalize data
        List<double> NormalizeData(List<double> pressures);

        // Calculates the mode of the array
        double CalculateMedian(List<double> pressures);
    }
}
