using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer
{
    public class DataReader
    {
        public static string[] ReadText(string filepath)
        {
            string[] result = null;    

            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("File was not found. Please check if filepath was appropriately set");
            }
            else
            {
                try
                {
                    result = File.ReadAllLines(filepath);
                } catch (Exception ex)
                {
                    System.Console.Write("Error message: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
                }

            }

            return result;
        }
    }
}
