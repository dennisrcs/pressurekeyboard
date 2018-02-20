using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Parser
{
    // Feature parsers interface
    public interface IParser
    {
        // for a given input filepath parsers its data
        void Parse(string filepath, bool normed);
    }
}
