using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSplitter
{
    // splits pressure data into appropriate files
    public class PressureDataSplitter : IDataSplitter
    {
        // members
        private string _root;

        // constructor
        public PressureDataSplitter(string root)
        {

        }

        // splits content into appropriate files
        public void Split(TaskInfo taskInfo, string[] content)
        {
            throw new NotImplementedException();
        }
    }
}
