using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Model;

namespace DataAnalyzer.Parser.KeystrokeFeatures
{
    // Repo with all keystroke feature functions
    public class KeystrokeFeatureExtractor : IKeystrokeFeatureExtractor
    {
        // calculate average typing speed
        public double CalculateAverageTypingSpeed(List<Keystroke> keystrokes)
        {
            double result = 0;
            double counter = 0;

            for (int i = 0; i < keystrokes.Count; i++)
            {
                Keystroke key = keystrokes[i];
                if (!key.IsKeyUp)
                    counter += 1;
            }

            double diff_ms = CalculateTaskDuration(keystrokes);

            result = counter / diff_ms;
            return result;
        }

        // calculates the average down down time
        public double CalculateAverageDownDownTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum_consecutive_diffs = 0;

            // selecting keydowns (i.e., key presses)
            List<Keystroke> keydowns = (List<Keystroke>) keystrokes.Where(x => !x.IsKeyUp);
            
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                DateTime timestamp_keystroke = keydowns[i].Timestamp;
                DateTime timestamp_next_keystrone = keydowns[i + 1].Timestamp;
                TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                int diff_ms = (int)span.TotalMilliseconds;
                sum_consecutive_diffs += diff_ms;
            }

            result = sum_consecutive_diffs / keydowns.Count;
            return result;
        }

        // calculates the smallest down down time
        public double CalculateSmallestDownDownTime(List<Keystroke> keystrokes)
        {
            double result = Double.MaxValue;
            
            // selecting keydowns online (i.e., key presses)
            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp);

            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                DateTime timestamp_keystroke = keydowns[i].Timestamp;
                DateTime timestamp_next_keystrone = keydowns[i + 1].Timestamp;
                TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                int diff_ms = (int)span.TotalMilliseconds;

                if (diff_ms < result )
                    result = diff_ms;
            }

            return result;
        }

        // calculates the largest down down time
        public double CalculateLargestDownDownTime(List<Keystroke> keystrokes)
        {
            double result = Double.MinValue;
            
            // selecting keydowns online (i.e., key presses)
            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp);

            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                DateTime timestamp_keystroke = keydowns[i].Timestamp;
                DateTime timestamp_next_keystrone = keydowns[i + 1].Timestamp;
                TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                int diff_ms = (int)span.TotalMilliseconds;

                if (result < diff_ms)
                    result = diff_ms;
            }

            return result;
        }

        // calculates the average down up time
        public double CalculateAverageDownUpTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum_consecutive_diffs = 0;

            Queue<Keystroke> stack = new Queue<Keystroke>();

            int i = 0;
            while (i < keystrokes.Count)
            {
                Keystroke current = keystrokes[i];
                if (!current.IsKeyUp)
                    stack.Enqueue(current);
                else
                {
                    Keystroke popped_keystroke = stack.Dequeue(); ;
                    DateTime timestamp_keystroke = current.Timestamp;
                    DateTime timestamp_next_keystrone = popped_keystroke.Timestamp;
                    TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                    sum_consecutive_diffs = (int)span.TotalMilliseconds;
                }
            }
            
            result = sum_consecutive_diffs / keystrokes.Count;
            return result;
        }

        // calculates the smallest down up time
        public double CalculateSmallestDownUpTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            Queue<Keystroke> stack = new Queue<Keystroke>();

            int i = 0;
            while (i < keystrokes.Count)
            {
                Keystroke current = keystrokes[i];
                if (!current.IsKeyUp)
                    stack.Enqueue(current);
                else
                {
                    Keystroke popped_keystroke = stack.Dequeue(); ;
                    DateTime timestamp_keystroke = current.Timestamp;
                    DateTime timestamp_next_keystrone = popped_keystroke.Timestamp;
                    TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                    int diff_ms = (int)span.TotalMilliseconds;

                    if (diff_ms < result)
                        result = diff_ms;

                }
            }
            
            return result;
        }

        // calculates the largest down up time
        public double CalculateLargestDownUpTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            Queue<Keystroke> stack = new Queue<Keystroke>();

            int i = 0;
            while (i < keystrokes.Count)
            {
                Keystroke current = keystrokes[i];
                if (!current.IsKeyUp)
                    stack.Enqueue(current);
                else
                {
                    Keystroke popped_keystroke = stack.Dequeue(); ;
                    DateTime timestamp_keystroke = current.Timestamp;
                    DateTime timestamp_next_keystrone = popped_keystroke.Timestamp;
                    TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                    int diff_ms = (int)span.TotalMilliseconds;

                    if (result < diff_ms)
                        result = diff_ms;

                }
            }

            return result;
        }

        // number of pauses between 0.5 and 1 seconds
        public double CalculateNumberPauses05And1Seconds(List<Keystroke> keystrokes)
        {
            double result = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp);
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                DateTime timestamp_keystroke = keydowns[i].Timestamp;
                DateTime timestamp_next_keystrone = keydowns[i + 1].Timestamp;
                TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                int diff_s = (int)span.TotalSeconds;

                if (diff_s >= 0.5 && diff_s < 1)
                    result += 1;
            }

            return result;
        }

        // number of pauses between 1 and 1.5 seconds
        public double CalculateNumberPauses1And15Seconds(List<Keystroke> keystrokes)
        {
            double result = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp);
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                DateTime timestamp_keystroke = keydowns[i].Timestamp;
                DateTime timestamp_next_keystrone = keydowns[i + 1].Timestamp;
                TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                int diff_s = (int)span.TotalSeconds;

                if (diff_s >= 1 && diff_s < 1.5)
                    result += 1;
            }

            return result;
        }

        // number of pauses between 1.5 and 2 seconds
        public double CalculateNumberPauses15And2Seconds(List<Keystroke> keystrokes)
        {
            double result = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp);
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                DateTime timestamp_keystroke = keydowns[i].Timestamp;
                DateTime timestamp_next_keystrone = keydowns[i + 1].Timestamp;
                TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                int diff_s = (int)span.TotalSeconds;

                if (diff_s >= 1.5 && diff_s < 2)
                    result += 1;
            }

            return result;
        }

        // number of pauses between 2 and 3 seconds
        public double CalculateNumberPauses2And3Seconds(List<Keystroke> keystrokes)
        {
            double result = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp);
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                DateTime timestamp_keystroke = keydowns[i].Timestamp;
                DateTime timestamp_next_keystrone = keydowns[i + 1].Timestamp;
                TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                int diff_s = (int)span.TotalSeconds;

                if (diff_s >= 2 && diff_s < 3)
                    result += 1;
            }

            return result;
        }

        // counts number of error keys (delete or backspace) pressed 
        public double CalculateNumberOfErrorKeysPressed(List<Keystroke> keystrokes)
        {
            double result = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp);
            for (int i = 0; i < keydowns.Count; i++)
                if (keydowns[i].Character.Equals("Back") || keydowns[i].Character.Equals("Delete"))
                    result += 1;

            return result;
        }

        // calculates duration of task
        public double CalculateTaskDuration(List<Keystroke> keystrokes)
        {
            double result = 0;
            
            // calculate differences between times in first and last key presses
            DateTime init_time = keystrokes[0].Timestamp;
            DateTime final_time = keystrokes[keystrokes.Count - 1].Timestamp;
            TimeSpan span = final_time - init_time;
            result = (int)span.TotalMilliseconds;

            return result;
        }
    }
}
