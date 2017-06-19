using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Model
{
    public class KeystrokeFRS : Keystroke
    {
        // members
        private double[] _values;
        public double[] Values
        {
            get { return _values; }
        }

        // constructor 1
        public KeystrokeFRS(string line, int id, string[] strvalues) : base(line, id)
        {
            _values = new double[Constants.NUM_SENSORS];

            for (int i = 0; i < Constants.NUM_SENSORS; i++)
                _values[i] = Double.Parse(strvalues[i]);
        }

        // constructor 2
        public KeystrokeFRS(Keystroke keystroke, string[] strvalues) : base(keystroke)
        {
            _values = new double[Constants.NUM_SENSORS];

            for (int i = 0; i < Constants.NUM_SENSORS; i++)
                _values[i] = Double.Parse(strvalues[i]);
        }
    }
}
