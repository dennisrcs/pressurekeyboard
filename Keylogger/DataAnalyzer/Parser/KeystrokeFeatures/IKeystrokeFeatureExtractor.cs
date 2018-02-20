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
        // Calculates average typing speed
        double CalculateAverageTypingSpeed(List<Keystroke> keystrokes);

        // calculates duration of task
        double CalculateTaskDuration(List<Keystroke> keystrokes);

        // calculates the average down down time
        AverageStdDev CalculateAverageDownDownTime(List<Keystroke> keystrokes);

        // calculates the average down up time
        AverageStdDev CalculateAverageDownUpTime(List<Keystroke> keystrokes);

        // Calculate the average duration between 1st key up and next key down of the digraphs
        AverageStdDev CalculateFirstUpNextDownDigraphTime(List<Keystroke> keystrokes);

        // average duration of the digraphs (1st key down, 2nd key up)
        AverageStdDev CalculateDigraphDuration(List<Keystroke> keystrokes);

        // counts number of error keys (delete or backspace) pressed 
        double CalculateNumberOfErrorKeysPressed(List<Keystroke> keystrokes);

        // counts number of arrow keys pressed 
        double CalculateNumberOfArrowKeysPressed(List<Keystroke> keystrokes);

        // counts number of shift keys pressed 
        double CalculateNumberOfShiftKeysPressed(List<Keystroke> keystrokes);

        // counts number of control keys pressed 
        double CalculateNumberOfControlKeysPressed(List<Keystroke> keystrokes);

        // Calculates number of pauses (no activity > 0.5s)
        double CalculateNumberOfPauses(List<Keystroke> keystrokes);

        // Calculates the average pause length (no activity > 0.5s)
        AverageStdDev CalculateAveragePauseLength(List<Keystroke> keystrokes);

        /* new features */

        // average duration of the 2nd key of the digraphs
        double CalculateDownUpTimeSecondDigraphKey(List<Keystroke> keystrokes);

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
