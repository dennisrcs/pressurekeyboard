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
            string data_dir = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\Parsed";

            // parses data in the given directory and prints features extract into 'output_filepath'
            DataParser parser = new DataParser(data_dir);
            List<FeatureSample> data = parser.Parse();

            // output file where data will be dumped
            string output_filepath_t1 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\task1.csv";
            string output_filepath_t2 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\task2.csv";
            string output_filepath_t3 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\task3.csv";
            string output_filepath_t4 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\task4.csv";
            string output_filepath_t5 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\task5.csv";

            // output file where data will be dumped
            string output_filepath_keystrokesonly_t1 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\keystroke_only\\task1.csv";
            string output_filepath_keystrokesonly_t2 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\keystroke_only\\task2.csv";
            string output_filepath_keystrokesonly_t3 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\keystroke_only\\task3.csv";
            string output_filepath_keystrokesonly_t4 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\keystroke_only\\task4.csv";
            string output_filepath_keystrokesonly_t5 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\keystroke_only\\task5.csv";

            // output file where data will be dumped
            string output_filepath_pressureonly_t1 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\pressure_only\\task1.csv";
            string output_filepath_pressureonly_t2 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\pressure_only\\task2.csv";
            string output_filepath_pressureonly_t3 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\pressure_only\\task3.csv";
            string output_filepath_pressureonly_t4 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\pressure_only\\task4.csv";
            string output_filepath_pressureonly_t5 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\pressure_only\\task5.csv";

            parser.PrintTaskFullFeatureVectorsToFile(data, output_filepath_t1, 0);
            parser.PrintTaskFullFeatureVectorsToFile(data, output_filepath_t2, 1);
            parser.PrintTaskFullFeatureVectorsToFile(data, output_filepath_t3, 2);
            parser.PrintTaskFullFeatureVectorsToFile(data, output_filepath_t4, 3);
            parser.PrintTaskFullFeatureVectorsToFile(data, output_filepath_t5, 4);

            parser.PrintTaskKeystrokeFeatureVectorsToFile(data, output_filepath_keystrokesonly_t1, 0);
            parser.PrintTaskKeystrokeFeatureVectorsToFile(data, output_filepath_keystrokesonly_t2, 1);
            parser.PrintTaskKeystrokeFeatureVectorsToFile(data, output_filepath_keystrokesonly_t3, 2);
            parser.PrintTaskKeystrokeFeatureVectorsToFile(data, output_filepath_keystrokesonly_t4, 3);
            parser.PrintTaskKeystrokeFeatureVectorsToFile(data, output_filepath_keystrokesonly_t5, 4);

            parser.PrintTaskPressureFeatureVectorsToFile(data, output_filepath_pressureonly_t1, 0);
            parser.PrintTaskPressureFeatureVectorsToFile(data, output_filepath_pressureonly_t2, 1);
            parser.PrintTaskPressureFeatureVectorsToFile(data, output_filepath_pressureonly_t3, 2);
            parser.PrintTaskPressureFeatureVectorsToFile(data, output_filepath_pressureonly_t4, 3);
            parser.PrintTaskPressureFeatureVectorsToFile(data, output_filepath_pressureonly_t5, 3);
        }
    }
}
