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

        public void Parse(string filepath)
        {
            string[] inputdata = DataReader.ReadText(filepath);

            for (int i = 0; i < inputdata.Length; i++)
            {
                string line = inputdata[i];
                Keystroke key = new Keystroke(line);
                _keystrokes.Add(key);
            }
        }
    }
}
