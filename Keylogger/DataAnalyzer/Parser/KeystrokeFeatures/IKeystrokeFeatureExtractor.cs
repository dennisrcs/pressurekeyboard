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

        // calculates the average down up time
        double CalculateAverageDownUpTime(List<Keystroke> keystrokes);

        // counts number of error keys (delete or backspace) pressed 
        double CalculateNumberOfErrorKeysPressed(List<Keystroke> keystrokes);

        // calculates duration of task
        double CalculateTaskDuration(List<Keystroke> keystrokes);

        /* new features */

        // duration between 1st and 2nd down keys of the digraphs
        double CalculateDownDownDigraphTime(List<Keystroke> keystrokes);

        // average duration of the 1st key of the digraphs
        double CalculateDownUpTimeFirstDigraphKey(List<Keystroke> keystrokes);

        // Calculate the average duration between 1st key up and next key down of the digraphs
        double CalculateFirstUpNextDownDigraphTime(List<Keystroke> keystrokes);
        
        // average duration of the 2nd key of the digraphs
        double CalculateDownUpTimeSecondDigraphKey(List<Keystroke> keystrokes);

        // average duration of the digraphs (1st key down, 2nd key up)
        double CalculateDigraphDuration(List<Keystroke> keystrokes);

        // average duration between 1st and 2nd down keys of the trigraphs
        double CalculateDownDownTrigraphTime(List<Keystroke> keystrokes);

        // Average duration of the 1st key of the trigraphs
        double CalculateDownUpTimeFirstTrigraphKey(List<Keystroke> keystrokes);

        // Calculate the average duration between 1st key up and next key down of the trigraphs
        double CalculateFirstUpNextDownTrigraphTime(List<Keystroke> keystrokes);

        // average duration between 2nd and 3rd down keys of the trigraphs
        double CalculateDownDownSecondThirdTrigraphTime(List<Keystroke> keystrokes);

        // average duration of the 2nd key of the trigraphs
        double CalculateDownUpTimeSecondTrigraphKey(List<Keystroke> keystrokes);

        // Calculate the average duration between 2nd key up and next key down of the trigraphs
        double CalculateSecondUpNextDownTrigraphTime(List<Keystroke> keystrokes);

        // average duration of the 3nd key of the trigraphs
        double CalculateDownUpTimeThirdTrigraphKey(List<Keystroke> keystrokes);

        // average duration of the trigraphs (1st key down, 3rd key up)
        double CalculateTrigraphDuration(List<Keystroke> keystrokes);
    }
}
