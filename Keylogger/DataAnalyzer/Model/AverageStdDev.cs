using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Model
{
    public class AverageStdDev
    {
        private double _average;
        public double Average { get { return _average; } }

        private double _stdDev;
        public double StdDev { get { return _stdDev; } }

        public AverageStdDev(double average, double stdDev)
        {
            this._average = average;
            this._stdDev = stdDev;
        }
    }
}
