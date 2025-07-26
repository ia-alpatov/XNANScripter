using System.Collections.Generic;
using System.Threading.Tasks;

namespace XNANScripter.Engine.Functions
{
    internal class AutomodeFunction : Function
    {
        public string name { get { return "automode"; } set { } }

        public List<string> parameters { get; set; }

        public AutomodeFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            //throw new NotImplementedException();
        }
    }
}