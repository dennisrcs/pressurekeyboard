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

            result[0] = feature_extractor.CalculateAverageTypingSpeed(keystrokes);
            result[1] = feature_extractor.CalculateAverageDownDownTime(keystrokes);
            result[2] = feature_extractor.CalculateAverageDownUpTime(keystrokes);
            result[3] = feature_extractor.CalculateNumberOfErrorKeysPressed(keystrokes);
            result[4] = feature_extractor.CalculateDownDownDigraphTime(keystrokes);
            result[5] = feature_extractor.CalculateDownUpTimeFirstDigraphKey(keystrokes);
            result[6] = feature_extractor.CalculateFirstUpNextDownDigraphTime(keystrokes);
            result[7] = feature_extractor.CalculateDownUpTimeSecondDigraphKey(keystrokes);
            result[8] = feature_extractor.CalculateDigraphDuration(keystrokes);

            /*
             * trigraph features
            result[10] = feature_extractor.CalculateDownDownTrigraphTime(keystrokes);
            result[11] = feature_extractor.CalculateDownUpTimeFirstTrigraphKey(keystrokes);
            result[12] = feature_extractor.CalculateFirstUpNextDownTrigraphTime(keystrokes);
            result[13] = feature_extractor.CalculateDownDownSecondThirdTrigraphTime(keystrokes);
            result[14] = feature_extractor.CalculateDownUpTimeSecondTrigraphKey(keystrokes);
            result[15] = feature_extractor.CalculateSecondUpNextDownTrigraphTime(keystrokes);
            result[16] = feature_extractor.CalculateDownUpTimeThirdTrigraphKey(keystrokes);
            result[17] = feature_extractor.CalculateTrigraphDuration(keystrokes);
            */

            return result;
        }
    }
}
