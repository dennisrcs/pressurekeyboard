using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Model;

namespace DataAnalyzer.Parser.KeystrokeFeatures
{
    public interface IKeystrokeFeatureExtractor
    {
        double CalculateAverageTypingSpeed(List<Keystroke> keystrokes);

        // calculates the average down down time
        double CalculateAverageDownDownTime(List<Keystroke> keystrokes);

        // calculates the smallest down down time
        double CalculateSmallestDownDownTime(List<Keystroke> keystrokes);

        // calculates the largest down down time
        double CalculateLargestDownDownTime(List<Keystroke> keystrokes);

        // calculates the average down up time
        double CalculateAverageDownUpTime(List<Keystroke> keystrokes);

        // calculates the smallest down up time
        double CalculateSmallestDownUpTime(List<Keystroke> keystrokes);

        // calculates the largest down up time
        double CalculateLargestDownUpTime(List<Keystroke> keystrokes);

        // number of pauses between 0.5 and 1 seconds
        double CalculateNumberPauses05And1Seconds(List<Keystroke> keystrokes);

        // number of pauses between 1 and 1.5 seconds
        double CalculateNumberPauses1And15Seconds(List<Keystroke> keystrokes);

        // number of pauses between 1.5 and 2 seconds
        double CalculateNumberPauses15And2Seconds(List<Keystroke> keystrokes);

        // number of pauses between 2 and 3 seconds
        double CalculateNumberPauses2And3Seconds(List<Keystroke> keystrokes);

        // counts number of error keys (delete or backspace) pressed 
        double CalculateNumberOfErrorKeysPressed(List<Keystroke> keystrokes);

        // calculates duration of task
        double CalculateTaskDuration(List<Keystroke> keystrokes);
    }
}
