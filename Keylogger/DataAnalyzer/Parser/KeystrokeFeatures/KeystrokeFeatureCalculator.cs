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
            double[] result = new double[Constants.NUM_KEYSTROKE_FEATURES];

            // creating instance of keystroke feature extractor
            IKeystrokeFeatureExtractor feature_extractor = new KeystrokeFeatureExtractor();

            // calculating keystroke features
            result[0] = feature_extractor.CalculateAverageTypingSpeed(keystrokes);
            result[1] = feature_extractor.CalculateAverageDownDownTime(keystrokes);
            result[2] = feature_extractor.CalculateLargestDownDownTime(keystrokes);
            result[3] = feature_extractor.CalculateSmallestDownDownTime(keystrokes);
            result[4] = feature_extractor.CalculateAverageDownUpTime(keystrokes);
            result[5] = feature_extractor.CalculateLargestDownUpTime(keystrokes);
            result[6] = feature_extractor.CalculateSmallestDownUpTime(keystrokes);
            result[7] = feature_extractor.CalculateNumberPauses05And1Seconds(keystrokes);
            result[8] = feature_extractor.CalculateNumberPauses1And15Seconds(keystrokes);
            result[9] = feature_extractor.CalculateNumberPauses15And2Seconds(keystrokes);
            result[10] = feature_extractor.CalculateNumberPauses2And3Seconds(keystrokes);
            result[11] = feature_extractor.CalculateNumberOfErrorKeysPressed(keystrokes);
            result[12] = feature_extractor.CalculateTaskDuration(keystrokes);
            
            return result;
        }
    }
}
