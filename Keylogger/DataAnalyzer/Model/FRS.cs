using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Model
{
    // force round resistor
    public class FRS
    {
        // members
        private double[] _values;
        public double[] Values
        {
            get { return _values; }
        }

        // constructor
        public FRS(string[] strvalues)
        {
            _values = new double[Constants.NUM_SENSORS];

            for (int i = 0; i < Constants.NUM_SENSORS; i++)
                _values[i] = Double.Parse(strvalues[i]);

        }
    }
}
