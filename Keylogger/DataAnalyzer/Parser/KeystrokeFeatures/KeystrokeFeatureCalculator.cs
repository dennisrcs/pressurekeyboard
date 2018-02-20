using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Model;

namespace DataAnalyzer.Parser.KeystrokeFeatures
{
    // keystroke feature calculator
    public class KeystrokeFeatureCalculator
    {
        // returns feature vector for keystroke features
        public double[] CalculateFeatures(List<Keystroke> keystrokes)
        {
            List<double> result = new List<double>();

            // creating instance of keystroke feature extractor
            IKeystrokeFeatureExtractor feature_extractor = new KeystrokeFeatureExtractor();

            // Adding user features
            result.Add(feature_extractor.CalculateAverageTypingSpeed(keystrokes));
            result.Add(feature_extractor.CalculateTaskDuration(keystrokes));

            // Adding latency features
            AverageStdDev ddTime = feature_extractor.CalculateAverageDownDownTime(keystrokes);
            result.Add(ddTime.Average);
            result.Add(ddTime.StdDev);

            AverageStdDev duTime = feature_extractor.CalculateAverageDownUpTime(keystrokes);
            result.Add(duTime.Average);
            result.Add(duTime.StdDev);

            AverageStdDev udTime = feature_extractor.CalculateFirstUpNextDownDigraphTime(keystrokes);
            result.Add(udTime.Average);
            result.Add(udTime.StdDev);

            AverageStdDev digdurTime = feature_extractor.CalculateDigraphDuration(keystrokes);
            result.Add(digdurTime.Average);
            result.Add(digdurTime.StdDev);

            // Adding frequency features
            result.Add(feature_extractor.CalculateNumberOfErrorKeysPressed(keystrokes));
            //result.Add(feature_extractor.CalculateNumberOfArrowKeysPressed(keystrokes));
            //result.Add(feature_extractor.CalculateNumberOfShiftKeysPressed(keystrokes));
            //result.Add(feature_extractor.CalculateNumberOfControlKeysPressed(keystrokes));

            // Adding pause features
            result.Add(feature_extractor.CalculateNumberOfPauses(keystrokes));

            AverageStdDev pauseLenght = feature_extractor.CalculateAveragePauseLength(keystrokes);
            result.Add(pauseLenght.Average);
            result.Add(pauseLenght.StdDev);

            return result.ToArray();
        }
    }
}
