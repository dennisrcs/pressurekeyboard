using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SerialCommunicationConsole
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            SerialPort serial;
            Program prg = new Program();

            serial = new SerialPort("COM4");
            prg.configSerial(serial);

            try
            {
                serial.Open();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            
            serial.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            Console.ReadKey();
            serial.Close();
        }

        public void configSerial(SerialPort serial)
        {
            serial.BaudRate = 9600;
            serial.Parity = Parity.None;
            serial.StopBits = StopBits.One;
            serial.DataBits = 8;
            serial.Handshake = Handshake.None;
        }

        public static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = (SerialPort)sender;
            try
            {
                if (serialPort.IsOpen)
                {
                    string txt = serialPort.ReadExisting();
                    StreamWriter file = new StreamWriter("test.txt", true);
                    file.WriteLine(txt);
                    file.Close();
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
