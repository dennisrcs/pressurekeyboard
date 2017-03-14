using System;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Linq;
using Keylogger;

namespace KeystrokeLogger
{
    //Class responsible for communicating with serial port
    public class SerialServices
    {
        // serial port reference
        private SerialPort _serial;

        // static members
        private static string _filename;

        public static string Filename {
            get { return _filename; }
        }

        // constructor
        public SerialServices(SerialPort serial)
        {
            _serial = serial;
            _serial.BaudRate = 9600;
            _serial.Parity = Parity.None;
            _serial.StopBits = StopBits.One;
            _serial.DataBits = 8;
            _serial.Handshake = Handshake.None;

            _filename = Utils.RandomString(10);
        }

        // opens the communication channel and sets the callback 
        public void Open()
        {
            try
            {
                _serial.Open();
                _serial.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        // events that is triggered whenever there is data available
        public static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = (SerialPort)sender;
            try
            {
                // before reading checks if port is opened
                if (serialPort.IsOpen)
                {
                    string txt = serialPort.ReadExisting();
                    StreamWriter file = new StreamWriter("pressure" + _filename, true);
                    file.WriteLine("time:" + Utils.GetTimestamp(DateTime.Now));
                    file.WriteLine(txt);
                    file.Close();
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

    }
}
