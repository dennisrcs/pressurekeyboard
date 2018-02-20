using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSplitter
{
    public class NearestSensorRepo
    {
        public Dictionary<string, int> NearestKeySensorMap { get { return _nearestKeySensorMap; } }
        private Dictionary<string, int> _nearestKeySensorMap;

        public NearestSensorRepo()
        {
            _nearestKeySensorMap = new Dictionary<string, int>();

            // SENSOR 0
            _nearestKeySensorMap.Add("Esc", 0);
            _nearestKeySensorMap.Add("F1", 0);
            _nearestKeySensorMap.Add("F2", 0);
            _nearestKeySensorMap.Add("F3", 0);
            _nearestKeySensorMap.Add("F4", 0);
            _nearestKeySensorMap.Add("F5", 0);
            _nearestKeySensorMap.Add("F6", 0);
            _nearestKeySensorMap.Add("F7", 0);
            _nearestKeySensorMap.Add("F8", 0);
            _nearestKeySensorMap.Add("Oem_3", 0);
            _nearestKeySensorMap.Add("1", 0);
            _nearestKeySensorMap.Add("2", 0);
            _nearestKeySensorMap.Add("3", 0);
            _nearestKeySensorMap.Add("4", 0);
            _nearestKeySensorMap.Add("5", 0);
            _nearestKeySensorMap.Add("6", 0);
            _nearestKeySensorMap.Add("7", 0);
            _nearestKeySensorMap.Add("8", 0);
            _nearestKeySensorMap.Add("9", 0);
            _nearestKeySensorMap.Add("0", 0);
            _nearestKeySensorMap.Add("Tab", 0);
            _nearestKeySensorMap.Add("Q", 0);
            _nearestKeySensorMap.Add("W", 0);
            _nearestKeySensorMap.Add("E", 0);
            _nearestKeySensorMap.Add("R", 0);
            _nearestKeySensorMap.Add("T", 0);
            _nearestKeySensorMap.Add("Y", 0);
            _nearestKeySensorMap.Add("U", 0);
            _nearestKeySensorMap.Add("I", 0);
            _nearestKeySensorMap.Add("O", 0);

            // SENSOR 1
            _nearestKeySensorMap.Add("Caps", 1);
            _nearestKeySensorMap.Add("A", 1);
            _nearestKeySensorMap.Add("S", 1);
            _nearestKeySensorMap.Add("D", 1);
            _nearestKeySensorMap.Add("F", 1);
            _nearestKeySensorMap.Add("G", 1);
            _nearestKeySensorMap.Add("H", 1);
            _nearestKeySensorMap.Add("J", 1);
            _nearestKeySensorMap.Add("K", 1);
            _nearestKeySensorMap.Add("L", 1);
            _nearestKeySensorMap.Add("Lshift", 1);
            _nearestKeySensorMap.Add("Z", 1);
            _nearestKeySensorMap.Add("X", 1);
            _nearestKeySensorMap.Add("C", 1);
            _nearestKeySensorMap.Add("V", 1);
            _nearestKeySensorMap.Add("B", 1);
            _nearestKeySensorMap.Add("N", 1);
            _nearestKeySensorMap.Add("M", 1);
            _nearestKeySensorMap.Add("Oem_Comma", 1);
            _nearestKeySensorMap.Add("Oem_Period", 1);
            _nearestKeySensorMap.Add("Lcontrol", 1);
            _nearestKeySensorMap.Add("windows", 1);
            _nearestKeySensorMap.Add("Lalt", 1);
            _nearestKeySensorMap.Add("space", 1);
            _nearestKeySensorMap.Add("Ralt", 1);

            // SENSOR 2
            _nearestKeySensorMap.Add("Oem_1", 2);
            _nearestKeySensorMap.Add("Oem_7", 2);
            _nearestKeySensorMap.Add("Return", 2);
            _nearestKeySensorMap.Add("Numpad4", 2);
            _nearestKeySensorMap.Add("Numpad5", 2);
            _nearestKeySensorMap.Add("Numpad6", 2);
            _nearestKeySensorMap.Add("Add", 2);
            _nearestKeySensorMap.Add("Oem_2", 2);
            _nearestKeySensorMap.Add("Rshift", 2);
            _nearestKeySensorMap.Add("Up", 2);
            _nearestKeySensorMap.Add("Numpad1", 2);
            _nearestKeySensorMap.Add("Numpad2", 2);
            _nearestKeySensorMap.Add("Numpad3", 2);
            _nearestKeySensorMap.Add("Rwin", 2);
            _nearestKeySensorMap.Add("Apps", 2);
            _nearestKeySensorMap.Add("Rcontrol", 2);
            _nearestKeySensorMap.Add("Left", 2);
            _nearestKeySensorMap.Add("Down", 2);
            _nearestKeySensorMap.Add("Right", 2);
            _nearestKeySensorMap.Add("Numpad0", 2);
            _nearestKeySensorMap.Add("Decimal", 2);

            // SENSOR 3
            _nearestKeySensorMap.Add("F9", 3);
            _nearestKeySensorMap.Add("F10", 3);
            _nearestKeySensorMap.Add("F11", 3);
            _nearestKeySensorMap.Add("F12", 3);
            _nearestKeySensorMap.Add("Snapshot", 3);
            _nearestKeySensorMap.Add("Scroll", 3);
            _nearestKeySensorMap.Add("Pause", 3);
            _nearestKeySensorMap.Add("Oem_Minus", 3);
            _nearestKeySensorMap.Add("Oem_Plus", 3);
            _nearestKeySensorMap.Add("Back", 3);
            _nearestKeySensorMap.Add("Insert", 3);
            _nearestKeySensorMap.Add("Home", 3);
            _nearestKeySensorMap.Add("Prior", 3);
            _nearestKeySensorMap.Add("Divide", 3);
            _nearestKeySensorMap.Add("Multiply", 3);
            _nearestKeySensorMap.Add("Subtract", 3);
            _nearestKeySensorMap.Add("Oem_4", 3);
            _nearestKeySensorMap.Add("Oem_6", 3);
            _nearestKeySensorMap.Add("Oem_5", 3);
            _nearestKeySensorMap.Add("Delete", 3);
            _nearestKeySensorMap.Add("End", 3);
            _nearestKeySensorMap.Add("Next", 3);
            _nearestKeySensorMap.Add("Numpad7", 3);
            _nearestKeySensorMap.Add("Numpad8", 3);
            _nearestKeySensorMap.Add("Numpad9", 3);
        }

        public int getNearestSensorFromKey(string key)
        {
            int result = 1;

            int value = 0;
            if (_nearestKeySensorMap.TryGetValue(key, out value))
                result = value;
            if (result == 0)
                result = 1;

            return result;
        }
    }
}
