using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSplitter
{
    public interface IDataSplitter
    {
        void Split(TaskInfo taskInfo, string[] keystrokes_content);
    }
}
