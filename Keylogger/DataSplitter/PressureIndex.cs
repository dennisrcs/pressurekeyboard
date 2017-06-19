using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSplitter
{
    public class PressureIndex
    {
        // members
        private string _pressure;
        public string Pressure { get { return _pressure; } set { _pressure = value; } }

        private int _index;
        public int Index { get { return _index; } }

        // constructor
        public PressureIndex(string pressure, int index)
        {
            this._pressure = pressure;
            this._index = index;
        }
    }
}
