using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSplitter
{
    // keystroke data splliter
    public class KeystrokesDataSplitter : IDataSplitter
    {
        // members
        private string _root;

        // constructor
        public KeystrokesDataSplitter(string root)
        {

        }

        // split content into appropriate keystroke files
        public void Split(TaskInfo taskInfo, string[] content)
        {
            throw new NotImplementedException();
        }
    }
}
