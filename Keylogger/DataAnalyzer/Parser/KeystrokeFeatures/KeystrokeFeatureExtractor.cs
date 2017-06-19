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

            // returns number of keystrokes per second
            result = counter / (diff_ms / 1000);
            return result;
        }

        // calculates the average down down time
        public double CalculateAverageDownDownTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum_consecutive_diffs = 0;

            // selecting keydowns (i.e., key presses)
            List<Keystroke> keydowns = (List<Keystroke>) keystrokes.Where(x => !x.IsKeyUp).ToList();
            
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

        // calculates the average down up time
        public double CalculateAverageDownUpTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum_consecutive_diffs = 0;

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
                    sum_consecutive_diffs += (int)span.TotalMilliseconds;
                }
                i += 1;
            }
            
            result = sum_consecutive_diffs / keystrokes.Count;
            return result;
        }

        // counts number of error keys (delete or backspace) pressed 
        public double CalculateNumberOfErrorKeysPressed(List<Keystroke> keystrokes)
        {
            double result = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
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

        // duration between 1st and 2nd down keys of the digraphs
        public double CalculateDownDownDigraphTime(List<Keystroke> keystrokes)
        {
            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            double result = 0;
            double sum = 0;
            int counter = 0;

            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i+1];

                if (GraphsRepository.IsDigraph(keystroke1, keystroke2))
                {
                    DateTime init_time = keystroke1.Timestamp;
                    DateTime final_time = keystroke2.Timestamp;
                    TimeSpan span = final_time - init_time;
                    sum += (int)span.TotalMilliseconds;
                    counter += 1;
                }
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }

        // average duration of the 1st key of the digraphs
        public double CalculateDownUpTimeFirstDigraphKey(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];

                if (GraphsRepository.IsDigraph(keystroke1, keystroke2))
                {
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
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }
        
        // Calculate the average duration between 1st key up and next key down of the digraphs
        public double CalculateFirstUpNextDownDigraphTime(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];

                if (GraphsRepository.IsDigraph(keystroke1, keystroke2))
                {
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
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }

        // average duration of the 2nd key of the digraphs
        public double CalculateDownUpTimeSecondDigraphKey(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];

                if (GraphsRepository.IsDigraph(keystroke1, keystroke2))
                {
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
            }

            result = (counter != 0) ? (sum / counter) : 0;
            return result;
        }

        // average duration of the digraphs (1st key down, 2nd key up)
        public double CalculateDigraphDuration(List<Keystroke> keystrokes)
        {
            double result = 0;
            double sum = 0;
            int counter = 0;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            for (int i = 0; i < keydowns.Count - 1; i++)
            {
                Keystroke keystroke1 = keydowns[i];
                Keystroke keystroke2 = keydowns[i + 1];

                if (GraphsRepository.IsDigraph(keystroke1, keystroke2))
                {
                    for (int j = keystroke2.Id; j < keystrokes.Count; j++)
                    {
                        if (keystrokes[j].IsKeyUp)
                        {
                            if (keystrokes[j].Character == keystroke2.Character)
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
            }

            result = (counter != 0) ? (sum / counter) : 0;
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

                if (GraphsRepository.IsTrigraph(keystroke1, keystroke2, keystroke3))
                {
                    DateTime init_time = keystroke1.Timestamp;
                    DateTime final_time = keystroke2.Timestamp;
                    TimeSpan span = final_time - init_time;
                    sum += (int)span.TotalMilliseconds;
                    counter += 1;
                }
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

                if (GraphsRepository.IsTrigraph(keystroke1, keystroke2, keystroke3))
                {
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

                if (GraphsRepository.IsTrigraph(keystroke1, keystroke2, keystroke3))
                {
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

                if (GraphsRepository.IsTrigraph(keystroke1, keystroke2, keystroke3))
                {
                    DateTime init_time = keystroke2.Timestamp;
                    DateTime final_time = keystroke3.Timestamp;
                    TimeSpan span = final_time - init_time;
                    sum += (int)span.TotalMilliseconds;
                    counter += 1;
                }
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

                if (GraphsRepository.IsTrigraph(keystroke1, keystroke2, keystroke3))
                {
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

                if (GraphsRepository.IsTrigraph(keystroke1, keystroke2, keystroke3))
                {
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

                if (GraphsRepository.IsTrigraph(keystroke1, keystroke2, keystroke3))
                {
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

                if (GraphsRepository.IsTrigraph(keystroke1, keystroke2, keystroke3))
                {
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
            }

            result = (counter != 0) ? (sum/counter) : 0;
            return result;
        }
    }
}
