using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Model
{
    // represents a single keystroke
    public class Keystroke
    {
        // members
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

        // constructor
        public Keystroke(string line)
        {
            // parsing file line
            string[] elements = line.Split(',');

            // each keystroke should have 3 elements: 0 or 1 (keyup or keydown),
            // which character was typed, and when it was typed (timestamp)
            if (elements.Length != 3)
                throw new ArgumentOutOfRangeException("The appropriate number of elements per line should be three");

            _isKeyUp = (elements[0] == "1");
            _character = elements[1];
            _timestamp = DateTime.Parse(elements[3]);
        }
    }
}
