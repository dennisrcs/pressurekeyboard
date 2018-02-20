using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public void Parse(string filepath, bool normed)
        {
            _pressures = new List<FRS>();
            string[] inputdata = DataReader.ReadText(filepath);

            for (int i = 0; i < inputdata.Length; i++)
            {
                string line = inputdata[i];
                string[] values = line.Split(';');

                Regex regex = new Regex(@"^\d+$");
                bool valid = true;

                int sensorNum = (normed) ? 1 : Constants.NUM_SENSORS;
                if (values.Length == sensorNum)
                {
                    // validating
                    for (int j = 0; j < sensorNum; j++)
                    {
                        if (values[j] == "")
                            valid &= false;
                    }
                        
                    // if valid
                    if (valid)
                    {
                        FRS frs_pressures = new FRS(values, normed);
                        _pressures.Add(frs_pressures);
                    }
                    
                }
            }

        }
    }
}
