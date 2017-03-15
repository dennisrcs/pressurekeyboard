using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Model
{
    public class Keystroke
    {
        private bool _isKeyUp;
        public bool IsKeyUp
        {
            get { return _isKeyUp; }
        }

        private string _character;
        public string Character
        {
            get { return _character; }
        }

        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get { return _timestamp; }
        }

        public Keystroke(string line)
        {
            string[] elements = line.Split(',');

            if (elements.Length != 3)
                throw new ArgumentOutOfRangeException("The appropriate number of elements per line should be three");

            _isKeyUp = (elements[0] == "1");
            _character = elements[1];
            _timestamp = DateTime.Parse(elements[3]);
        }
    }
}
