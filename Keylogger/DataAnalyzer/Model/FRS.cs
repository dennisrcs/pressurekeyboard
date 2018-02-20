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
        public FRS(string[] strvalues, bool normed)
        {
            int numSensors = (normed) ? 1 : Constants.NUM_SENSORS;
            _values = new double[numSensors];

            for (int i = 0; i < numSensors; i++)
                _values[i] = Double.Parse(strvalues[i]);

        }
    }
}
