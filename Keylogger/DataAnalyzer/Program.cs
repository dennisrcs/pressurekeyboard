using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Parser;

namespace DataAnalyzer
{
    public class Program
    {
        // main function
        static void Main(string[] args)
        {
            // data root directory
            string data_dir = "C:/Users/Dennis/Source/Repos/pressurekeyboard/Keylogger/Data";

            // output file where data will be dumped
            string output_filepath = "C:/Users/Dennis/Source/Repos/pressurekeyboard/Keylogger/Data/output.txt";

            // parses data in the given directory and prints features extract into 'output_filepath'
            DataParser parser = new DataParser(data_dir);
            List<FeatureSample> data = parser.Parse();
            parser.PrintFullFeatureVectorsToFile(data, output_filepath);
        }
    }
}
