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

        private int _id;
        public int Id 
        {
            get { return _id; }
        }

        public Keystroke(string line, int id)
        {
            // parsing file line
            string[] elements = line.Split(',');

            // each keystroke should have 3 elements: 0 or 1 (keyup or keydown),
            // which character was typed, and when it was typed (timestamp)
            if (elements.Length != 3)
                throw new ArgumentOutOfRangeException("The appropriate number of elements per line should be three");

            _id = id;
            _isKeyUp = (elements[0] == "1");
            _character = elements[1];
            _timestamp = DateTime.Parse(elements[2]);
        }

        // constructor
        public Keystroke(string line, int id, DateTime yearMonthDay)
        {
            // parsing file line
            string[] elements = line.Split(',');

            // each keystroke should have 3 elements: 0 or 1 (keyup or keydown),
            // which character was typed, and when it was typed (timestamp)
            if (elements.Length != 3)
                throw new ArgumentOutOfRangeException("The appropriate number of elements per line should be three");

            _id = id;
            _isKeyUp = (elements[0] == "1");
            _character = elements[1];

            string full_datetime = yearMonthDay.Year + "-" + yearMonthDay.Month + "-" + yearMonthDay.Day + " " + elements[2];

            _timestamp = DateTime.Parse(full_datetime);
        }

        // constructor 2
        public Keystroke(Keystroke keystroke)
        {
            _id = keystroke.Id;
            _isKeyUp = keystroke.IsKeyUp;
            _character = keystroke.Character;
            _timestamp = keystroke.Timestamp;
        }
    }
}
