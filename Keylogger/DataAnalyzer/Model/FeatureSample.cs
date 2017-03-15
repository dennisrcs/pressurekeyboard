namespace DataAnalyzer.Parser
{
    // stores data associated with a feature entry
    public class FeatureSample
    {
        // members
        private int task_id; // 0 for relaxed typing, 1 for stressed (movie) and 2 for stress (expressive writing)
        private string participant_id;
        private double[] keystroke_features;
        private double[] pressure_features;

        // constructor
        public FeatureSample(string participant_id, int task_id, double[] pressure_features, double[] keystroke_features)
        {
            this.participant_id = participant_id;
            this.task_id = task_id;
            this.pressure_features = pressure_features;
            this.keystroke_features = keystroke_features;
        }

        // returns the feature vector containing both pressure and keystroke features
        public string GetFullFeatureVector()
        {
            string result = "";

            for (int i = 0; i < pressure_features.Length; i++)
                if (i != 0)
                    result = result + "," + pressure_features[i];
                else
                    result = pressure_features[i] + "";

            for (int i = 0; i < keystroke_features.Length; i++)
                result = keystroke_features[i] + "";

            result = result + "," + task_id;

            return result;
        }

        // returns the feature vector including pressure features only
        public string GetPressureFeatureVector()
        {
            string result = "";

            for (int i = 0; i < pressure_features.Length; i++)
                if (i != 0)
                    result = result + "," + pressure_features[i];
                else
                    result = pressure_features[i] + "";

            result = result + "," + task_id;

            return result;
        }

        // returns the feature vector including keystroke features only
        public string GetKeystrokeFeatureVector()
        {
            string result = "";

            for (int i = 0; i < keystroke_features.Length; i++)
                if (i != 0)
                    result = result + "," + keystroke_features[i];
                else
                    result = keystroke_features[i] + "";

            result = result + "," + task_id;

            return result;
        }
    }
}