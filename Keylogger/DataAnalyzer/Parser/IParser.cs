using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Parser
{
    public interface IParser
    {
        void Parse(string filepath);
    }
}
