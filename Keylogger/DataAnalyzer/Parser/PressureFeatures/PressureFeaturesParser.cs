using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Model;
using DataAnalyzer.Parser;

namespace DataAnalyzer.Parser.PressureFeatures
{
    // Pressure features parser
    public class PressureFeaturesParser : IParser
    {
        private List<FRS> _pressures;
        public List<FRS> Pressures
        {
            get { return _pressures; }
        }

        // parser the input file
        public void Parse(string filepath)
        {
            _pressures = new List<FRS>();
            string[] inputdata = DataReader.ReadText(filepath);

            for (int i = 0; i < inputdata.Length; i++)
            {
                string line = inputdata[i];
                string[] values = line.Split(';');
                if (values.Length == Constants.NUM_SENSORS)
                {
                    FRS frs_pressures = new FRS(values);
                    _pressures.Add(frs_pressures);
                }
            }

        }
    }
}
