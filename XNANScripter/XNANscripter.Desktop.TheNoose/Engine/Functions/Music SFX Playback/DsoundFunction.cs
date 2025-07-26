using System.Collections.Generic;
using System.Threading.Tasks;

namespace XNANScripter.Engine.Functions
{
    internal class DsoundFunction : Function
    {
        public string name { get { return "dsound"; } set { } }

        public List<string> parameters { get; set; }

        public DsoundFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            parameters = null;
        }
    }
}