using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer.Model;

namespace DataAnalyzer.Parser.KeystrokeFeatures
{
    
    public class GraphsRepository
    {
        // digraphs
        public static string[] DIGRAPHS = new string[] 
                { "ch", "ck", "ff", "gh", "gn", "kn", "ll", "mb", "ng", "nk", "ph", "qu", "sh", "ss", "th", "wh", "wr", "zz" } ;
        
        // trigraphs
        public static string[] TRIGRAPHS = new string[]
                { "chr", "dge", "tch" } ;
        
        // returns whether keystroke1 + keystroke2 form a digraph
        public static bool IsDigraph(Keystroke keystroke1, Keystroke keystroke2)
        {
            bool result = false;
            string keystrokes = keystroke1.Character.ToLower() + keystroke2.Character.ToLower();

            for (int i = 0; i < DIGRAPHS.Length; i++)
            {
                if (DIGRAPHS[i].Equals(keystrokes))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        // returns whether keystroke1 + keystroke2 + keystroke3 form a trigraph
        public static bool IsTrigraph(Keystroke keystroke1, Keystroke keystroke2, Keystroke keystroke3)
        {
            bool result = false;
            string keystrokes = keystroke1.Character.ToLower() + keystroke2.Character.ToLower() + keystroke3.Character.ToLower();

            for (int i = 0; i < TRIGRAPHS.Length; i++)
            {
                if (TRIGRAPHS[i].Equals(keystrokes))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
