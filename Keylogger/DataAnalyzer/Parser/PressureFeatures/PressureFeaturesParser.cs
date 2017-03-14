using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Model;
using DataAnalyzer.Parser;

namespace DataAnalyzer.PressureFeatures
{
    public class PressureFeaturesParser : IParser
    {
        private List<FRS> _pressures;

        public void Parse(string filepath)
        {
            _pressures = new List<FRS>();
            string[] inputdata = DataReader.ReadText(filepath);

            for (int i = 0; i < inputdata.Length; i++)
            {
                string line = inputdata[i];
                string[] values = line.Split(';');
                if (values.Length == 4)
                {
                    FRS frs_pressures = new FRS(values);
                    _pressures.Add(frs_pressures);
                }
            }

        }
    }
}
