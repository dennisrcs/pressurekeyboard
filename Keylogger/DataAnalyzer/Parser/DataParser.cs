using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.KeystrokeFeatures;
using DataAnalyzer.Parser.KeystrokeFeatures;
using DataAnalyzer.Parser.PressureFeatures;

namespace DataAnalyzer.Parser
{
    // parser data
    public class DataParser
    {
        // member
        private string _root;

        // constructor
        public DataParser(string root_dir)
        {
            this._root = root_dir;
        }

        // parses features extract and build list of feature samples
        public List<FeatureSample> Parse()
        {
            List<FeatureSample> data = new List<FeatureSample>();

            for (int i = 0; i < Constants.NUM_TASKS; i++)
            {
                for (int j = 0; j < Constants.NUM_PARTICIPANTS; j++)
                {
                    // getting task and participant's info
                    string participant_id = j + "";
                    int task_id = i;

                    // parsing pressure features
                    string pressure_path = Path.Combine(_root, "Pressure", "Task" + (i+1), "Participant" + (j + 1) + ".txt");
                    PressureFeaturesParser pressure_parser = new PressureFeaturesParser();
                    pressure_parser.Parse(pressure_path);

                    // calculating pressure features
                    PressureFeatureCalculator pressure_calculator = new PressureFeatureCalculator();
                    double[] pressure_features = pressure_calculator.CalculateFeatures(pressure_parser.Pressures);

                    // parsing keystroke features
                    string keystroke_path = Path.Combine(_root, "Keystrokes", "Task" + (i + 1), "Participant" + (j + 1) + ".txt");
                    KeystrokeFeaturesParser keystroke_parser = new KeystrokeFeaturesParser();
                    keystroke_parser.Parse(keystroke_path);

                    // calculates keystroke features
                    KeystrokeFeatureCalculator keystroke_calculator = new KeystrokeFeatureCalculator();
                    double[] keystroke_features = keystroke_calculator.CalculateFeatures(keystroke_parser.Keystrokes);

                    // concatenates data generated and add to FeatureSample list
                    FeatureSample sample = new FeatureSample(participant_id, task_id, pressure_features, keystroke_features);
                    data.Add(sample);
                }
            }

            return data;
        }

        // prints the full feature vector to a text file
        public void PrintFullFeatureVectorsToFile(List<FeatureSample> data, string filepath)
        {
            string[] lines = new string[data.Count];

            for (int i = 0; i < data.Count; i++)
                lines[i] = data[i].GetFullFeatureVector();

            File.WriteAllLines(filepath, lines);
        }

        // prints the pressure feature vector to a text file
        public void PrintPressureFeatureVectorsToFile(List<FeatureSample> data, string filepath)
        {
            string[] lines = new string[data.Count];

            for (int i = 0; i < data.Count; i++)
                lines[i] = data[i].GetPressureFeatureVector();

            File.WriteAllLines(filepath, lines);
        }

        // prints the keystroke feature vector to a text file
        public void PrintKeystrokeFeatureVectorsToFile(List<FeatureSample> data, string filepath)
        {
            string[] lines = new string[data.Count];

            for (int i = 0; i < data.Count; i++)
                lines[i] = data[i].GetKeystrokeFeatureVector();

            File.WriteAllLines(filepath, lines);
        }
    }
}
