using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Model
{
    public class FRS
    {
        private int[] _values;
        public int[] Values
        {
            get { return _values; }
        }

        public FRS(string[] strvalues)
        {
            _values = new int[Constants.NUM_SENSORS];

            for (int i = 0; i < Constants.NUM_SENSORS; i++)
                _values[i] = Int32.Parse(strvalues[i]);

        }
    }
}
