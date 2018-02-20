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
            for (int i = 1; i <= 9; i++)
            {
                // data root directory
                string participant_id = "Participant" + i;
                string data_dir = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\Parsed";
                bool normalize = true;

                // parses data in the given directory and prints features extract into 'output_filepath'
                DataParser parser = new DataParser(data_dir, participant_id);
                List<FeatureSample> data = parser.Parse(normalize);

                // output file where data will be dumped
                string output_filepath_t1 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\" + participant_id + "\\task1.csv";
                string output_filepath_t2 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\" + participant_id + "\\task2.csv";
                string output_filepath_t3 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\" + participant_id + "\\task3.csv";

                // output file where data will be dumped
                string output_filepath_keystrokesonly_t1 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\" + participant_id + "\\keystroke_only\\task1.csv";
                string output_filepath_keystrokesonly_t2 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\" + participant_id + "\\keystroke_only\\task2.csv";
                string output_filepath_keystrokesonly_t3 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\" + participant_id + "\\keystroke_only\\task3.csv";

                // output file where data will be dumped
                string output_filepath_pressureonly_t1 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\" + participant_id + "\\pressure_only\\task1.csv";
                string output_filepath_pressureonly_t2 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\" + participant_id + "\\pressure_only\\task2.csv";
                string output_filepath_pressureonly_t3 = "C:\\Users\\Dennis\\Source\\Repos\\pressurekeyboard\\Keylogger\\Data\\output\\" + participant_id + "\\pressure_only\\task3.csv";

                parser.PrintTaskFullFeatureVectorsToFile(data, output_filepath_t1, 0);
                parser.PrintTaskFullFeatureVectorsToFile(data, output_filepath_t2, 1);
                parser.PrintTaskFullFeatureVectorsToFile(data, output_filepath_t3, 2);

                parser.PrintTaskKeystrokeFeatureVectorsToFile(data, output_filepath_keystrokesonly_t1, 0);
                parser.PrintTaskKeystrokeFeatureVectorsToFile(data, output_filepath_keystrokesonly_t2, 1);
                parser.PrintTaskKeystrokeFeatureVectorsToFile(data, output_filepath_keystrokesonly_t3, 2);

                parser.PrintTaskPressureFeatureVectorsToFile(data, output_filepath_pressureonly_t1, 0);
                parser.PrintTaskPressureFeatureVectorsToFile(data, output_filepath_pressureonly_t2, 1);
                parser.PrintTaskPressureFeatureVectorsToFile(data, output_filepath_pressureonly_t3, 2);
            }

           
        }
    }
}
