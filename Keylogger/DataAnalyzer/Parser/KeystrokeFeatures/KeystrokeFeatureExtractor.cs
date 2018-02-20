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
        /* User features */

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

            double diff_s = CalculateTaskDuration(keystrokes);

            // returns number of keystrokes per second
            result = counter / diff_s;
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
            result = (int)span.TotalMilliseconds / 1000;

            return result;
        }

        /* Pause-based Features */

        // calculates the number of pauses (interval between keystrokes > 0.5s)
        public double CalculateNumberOfPauses(List<Keystroke> keystrokes)
        {
            double result = 0;

            for (int i = 0; i < keystrokes.Count - 1; i++)
            {
                DateTime timestamp_keystroke = keystrokes[i].Timestamp;
                DateTime timestamp_next_keystrone = keystrokes[i + 1].Timestamp;
                TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                if ((int)span.TotalMilliseconds > 500)
                    result = result + 1;
            }

            return result;
        }

        // calculates pause length (interval between keystrokes > 0.5s)
        public AverageStdDev CalculateAveragePauseLength(List<Keystroke> keystrokes)
        {
            double avg = 0;
            double sd = 0;
            double sum_sqrd_diffs = 0;

            AverageStdDev result = null;
            List<double> pausesMiliseconds = new List<double>();

            for (int i = 0; i < keystrokes.Count - 1; i++)
            {
                DateTime timestamp_keystroke = keystrokes[i].Timestamp;
                DateTime timestamp_next_keystrone = keystrokes[i + 1].Timestamp;
                TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;

                if ((int)span.TotalMilliseconds > 500)
                    pausesMiliseconds.Add((int)span.TotalMilliseconds);
            }

            avg = pausesMiliseconds.Average();
            sum_sqrd_diffs = pausesMiliseconds.Select(val => (val - avg) * (val - avg)).Sum();
            sd = Math.Sqrt(sum_sqrd_diffs / pausesMiliseconds.Count);

            result = new AverageStdDev(avg, sd);

            return result;
        }

        /* Frequency Features */

        // counts number of error keys (delete or backspace) pressed 
        public double CalculateNumberOfErrorKeysPressed(List<Keystroke> keystrokes)
        {
            string[] keys = new string[2];
            keys[0] = "Back";
            keys[1] = "Delete";

            return CalculateFrequency(keystrokes, keys);
        }

        // counts number of error keys (delete or backspace) pressed 
        public double CalculateNumberOfArrowKeysPressed(List<Keystroke> keystrokes)
        {
            string[] keys = new string[4];
            keys[0] = "Right";
            keys[1] = "Left";
            keys[2] = "Up";
            keys[3] = "Down";

            return CalculateFrequency(keystrokes, keys);
        }

        // counts number of error keys (delete or backspace) pressed 
        public double CalculateNumberOfShiftKeysPressed(List<Keystroke> keystrokes)
        {
            string[] keys = new string[2];
            keys[0] = "Lshift";
            keys[1] = "Rshift";

            return CalculateFrequency(keystrokes, keys);
        }

        // counts number of error keys (delete or backspace) pressed 
        public double CalculateNumberOfControlKeysPressed(List<Keystroke> keystrokes)
        {
            string[] keys = new string[2];
            keys[0] = "Lcontrol";
            keys[1] = "Rcontrol";

            return CalculateFrequency(keystrokes, keys);
        }

        // Counts how many keys are found in the keystrokes list
        public double CalculateFrequency(List<Keystroke> keystrokes, string[] key)
        {
            double result = 0;
            bool contains;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count; i++)
            {
                contains = false;
                for (int j = 0; j < key.Length; j++)
                    contains |= keydowns[i].Character.Equals(key[j]);
                if (contains)
                    result += 1;
            }

            return result;
        }

        /* Latency features */

        // calculates the average down down time
        public AverageStdDev CalculateAverageDownDownTime(List<Keystroke> keystrokes)
        {
            AverageStdDev result = null;
            double avg = 0;
            double sd = 0;
            double sum_sqrd_diffs = 0;

            // selecting keydowns (i.e., key presses)
            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            double[] diffsArray = new double[keydowns.Count - 1];

            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                DateTime timestamp_keystroke = keydowns[i].Timestamp;
                DateTime timestamp_next_keystrone = keydowns[i + 1].Timestamp;
                TimeSpan span = timestamp_next_keystrone - timestamp_keystroke;
                diffsArray[i] = (int)span.TotalMilliseconds;
            }

            avg = diffsArray.Average();
            sum_sqrd_diffs = diffsArray.Select(val => (val - avg) * (val - avg)).Sum();
            sd = Math.Sqrt(sum_sqrd_diffs / diffsArray.Length);

            result = new AverageStdDev(avg, sd);

            return result;
        }

        // calculates the average down up time
        public AverageStdDev CalculateAverageDownUpTime(List<Keystroke> keystrokes)
        {
            double avg = 0;
            double sd = 0;
            double sum_sqrd_diffs = 0;

            List<double> diffsArray = new List<double>() ;
            AverageStdDev result = null;

            Queue<Keystroke> queue = new Queue<Keystroke>();

            int i = 0;
            while (i < keystrokes.Count)
            {
                Keystroke current = keystrokes[i];
                if (!current.IsKeyUp)
                    queue.Enqueue(current);
                else
                {
                    Keystroke popped_keystroke = queue.Dequeue();
                    DateTime timestamp_current_keystroke = current.Timestamp;
                    DateTime timestamp_old_keystrone = popped_keystroke.Timestamp;
                    TimeSpan span = timestamp_current_keystroke - timestamp_old_keystrone;
                    diffsArray.Add((int)span.TotalMilliseconds);
                }
                i += 1;
            }
            
            avg = diffsArray.Average();
            sum_sqrd_diffs = diffsArray.Select(val => (val - avg) * (val - avg)).Sum();
            sd = Math.Sqrt(sum_sqrd_diffs / diffsArray.Count);

            result = new AverageStdDev(avg, sd);
            
            return result;
        }
                
        // Calculate the average duration between 1st key up and next key down of the digraphs
        public AverageStdDev CalculateFirstUpNextDownDigraphTime(List<Keystroke> keystrokes)
        {
            double sum_sqrd_diffs = 0;
            double sd = 0;
            double avg = 0;

            AverageStdDev result = null;
            List<double> diffsArray = new List<double>();

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];

                for (int j = keystroke1.Id; j < keystrokes.Count; j++)
                {
                    if (keystrokes[j].IsKeyUp)
                    {
                        if (keystrokes[j].Character == keystroke1.Character)
                        {
                            DateTime init_time = keystrokes[j].Timestamp;
                            DateTime final_time = keystroke2.Timestamp;
                            TimeSpan span = final_time - init_time;
                            diffsArray.Add((int)span.TotalMilliseconds);
                            break;
                        }
                    }
                }   
            }

            avg = diffsArray.Average();
            sum_sqrd_diffs = diffsArray.Select(val => (val - avg) * (val - avg)).Sum();
            sd = Math.Sqrt(sum_sqrd_diffs / diffsArray.Count);

            result = new AverageStdDev(avg, sd);
            return result;
        }

        // average duration of the digraphs (1st key down, 2nd key up)
        public AverageStdDev CalculateDigraphDuration(List<Keystroke> keystrokes)
        {
            double sd = 0;
            double avg = 0;
            double sum_sqrd_diffs = 0;

            AverageStdDev result = null;
            List<double> diffsArray = new List<double>();

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];

                for (int j = keystroke2.Id; j < keystrokes.Count; j++)
                {
                    if (keystrokes[j].IsKeyUp)
                    {
                        if (keystrokes[j].Character == keystroke2.Character)
                        {
                            DateTime init_time = keystroke1.Timestamp;
                            DateTime final_time = keystrokes[j].Timestamp;
                            TimeSpan span = final_time - init_time;
                            diffsArray.Add((int)span.TotalMilliseconds);
                            break;
                        }
                    }
                }
            }

            // Calculating average and standard deviation
            avg = diffsArray.Average();
            sum_sqrd_diffs = diffsArray.Select(val => (val - avg) * (val - avg)).Sum();
            sd = Math.Sqrt(sum_sqrd_diffs / diffsArray.Count);

            result = new AverageStdDev(avg, sd);
            return result;
        }

        // average duration between 1st and 2nd down keys of the trigraphs
        public double CalculateDownDownTrigraphTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 2; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];
                Keystroke keystroke3 = keydowns[i + 2];

                DateTime init_time = keystroke1.Timestamp;
                DateTime final_time = keystroke2.Timestamp;
                TimeSpan span = final_time - init_time;
                sum += (int)span.TotalMilliseconds;
                counter += 1;
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }

        // Average duration of the 1st key of the trigraphs
        public double CalculateDownUpTimeFirstTrigraphKey(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 2; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];
                Keystroke keystroke3 = keydowns[i + 2];

                for (int j = keystroke1.Id; j < keystrokes.Count; j++)
                {
                    if (keystrokes[j].IsKeyUp)
                    {
                        if (keystrokes[j].Character == keystroke1.Character)
                        {
                            DateTime init_time = keystroke1.Timestamp;
                            DateTime final_time = keystrokes[j].Timestamp;
                            TimeSpan span = final_time - init_time;
                            sum += (int)span.TotalMilliseconds;
                            counter += 1;
                            break;
                        }
                    }
                }
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }

        // Calculate the average duration between 1st key up and next key down of the trigraphs
        public double CalculateFirstUpNextDownTrigraphTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 2; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];
                Keystroke keystroke3 = keydowns[i + 2];

                for (int j = keystroke1.Id; j < keystrokes.Count; j++)
                {
                    if (keystrokes[j].IsKeyUp)
                    {
                        if (keystrokes[j].Character == keystroke1.Character)
                        {
                            DateTime init_time = keystrokes[j].Timestamp;
                            DateTime final_time = keystroke2.Timestamp;
                            TimeSpan span = final_time - init_time;
                            sum += (int)span.TotalMilliseconds;
                            counter += 1;
                            break;
                        }
                    }
                }
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }

        // average duration between 2nd and 3rd down keys of the trigraphs
        public double CalculateDownDownSecondThirdTrigraphTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 2; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];
                Keystroke keystroke3 = keydowns[i + 2];

                DateTime init_time = keystroke2.Timestamp;
                DateTime final_time = keystroke3.Timestamp;
                TimeSpan span = final_time - init_time;
                sum += (int)span.TotalMilliseconds;
                counter += 1;
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }

        // average duration of the 2nd key of the trigraphs
        public double CalculateDownUpTimeSecondTrigraphKey(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 2; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];
                Keystroke keystroke3 = keydowns[i + 2];

                for (int j = keystroke2.Id; j < keystrokes.Count; j++)
                {
                    if (keystrokes[j].IsKeyUp)
                    {
                        if (keystrokes[j].Character == keystroke2.Character)
                        {
                            DateTime init_time = keystroke2.Timestamp;
                            DateTime final_time = keystrokes[j].Timestamp;
                            TimeSpan span = final_time - init_time;
                            sum += (int)span.TotalMilliseconds;
                            counter += 1;
                            break;
                        }
                    }
                }
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }

        // Calculate the average duration between 2nd key up and next key down of the trigraphs
        public double CalculateSecondUpNextDownTrigraphTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 2; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];
                Keystroke keystroke3 = keydowns[i + 2];

                for (int j = keystroke2.Id; j < keystrokes.Count; j++)
                {
                    if (keystrokes[j].IsKeyUp)
                    {
                        if (keystrokes[j].Character == keystroke2.Character)
                        {
                            DateTime init_time = keystrokes[j].Timestamp;
                            DateTime final_time = keystroke3.Timestamp;
                            TimeSpan span = final_time - init_time;
                            sum += (int)span.TotalMilliseconds;
                            counter += 1;
                            break;
                        }
                    }
                }
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }

        // average duration of the 3nd key of the trigraphs
        public double CalculateDownUpTimeThirdTrigraphKey(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 2; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];
                Keystroke keystroke3 = keydowns[i + 2];

                for (int j = keystroke3.Id; j < keystrokes.Count; j++)
                {
                    if (keystrokes[j].IsKeyUp)
                    {
                        if (keystrokes[j].Character == keystroke3.Character)
                        {
                            DateTime init_time = keystroke3.Timestamp;
                            DateTime final_time = keystrokes[j].Timestamp;
                            TimeSpan span = final_time - init_time;
                            sum += (int)span.TotalMilliseconds;
                            counter += 1;
                            break;
                        }
                    }
                }
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }

        // average duration of the trigraphs (1st key down, 3rd key up)
        public double CalculateTrigraphDuration(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 2; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];
                Keystroke keystroke3 = keydowns[i + 2];

                for (int j = keystroke3.Id; j < keystrokes.Count; j++)
                {
                    if (keystrokes[j].IsKeyUp)
                    {
                        if (keystrokes[j].Character == keystroke3.Character)
                        {
                            DateTime init_time = keystroke1.Timestamp;
                            DateTime final_time = keystrokes[j].Timestamp;
                            TimeSpan span = final_time - init_time;
                            sum += (int)span.TotalMilliseconds;
                            counter += 1;
                            break;
                        }
                    }
                }
            }

            result = (counter != 0) ? (sum/counter) : 0;
            return result;
        }

        public double CalculateDownUpTimeSecondDigraphKey(List<Keystroke> keystrokes)
        {
            throw new NotImplementedException();
        }
    }
}
