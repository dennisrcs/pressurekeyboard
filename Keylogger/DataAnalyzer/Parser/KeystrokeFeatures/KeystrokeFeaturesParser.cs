using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Model;
using DataAnalyzer.Parser;

namespace DataAnalyzer.KeystrokeFeatures
{
    // Keystroke feature parser
    public class KeystrokeFeaturesParser : IParser
    {
        private List<Keystroke> _keystrokes = new List<Keystroke>();
        public List<Keystroke> Keystrokes
        {
            get { return _keystrokes; }
        }

        public void Parse(string filepath, bool normed)
        {
            string[] inputdata = DataReader.ReadText(filepath);

            // ignore first keystroke if it is not key down
            Keystroke first_key = new Keystroke(inputdata[0],0);
            if (!first_key.IsKeyUp)
                _keystrokes.Add(first_key);

            for (int i = 1; i < inputdata.Length; i++)
            {
                string line = inputdata[i];
                Keystroke key = new Keystroke(line,i);
                _keystrokes.Add(key);
            }
        }

        // Finds first minute cutoff index
        internal static int FindFirstMinuteCutoffIndex(List<Keystroke> keystrokes)
        {
            int result = -1;

            List<Keystroke> keydowns = (List<Keystroke>)keystrokes.Where(x => !x.IsKeyUp).ToList();
            DateTime firstTimestamp = keydowns[0].Timestamp;

            for (int i = 0; i < keydowns.Count; i++)
            {
                DateTime currentTimestamp = keydowns[i].Timestamp;
                TimeSpan span = currentTimestamp - firstTimestamp;
                if ((int)span.TotalSeconds < 120)
                    continue;
                else
                {
                    result = i;
                    break;
                }
            }

            return result;
        }
    }
}
