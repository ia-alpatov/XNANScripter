using System.Collections.Generic;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Variables;

namespace XNANScripter.Engine.Functions
{
    internal class GlobalonFunction : Function
    {
        public string name { get { return "globalon"; } set { } }

        public List<string> parameters { get; set; }

        public GlobalonFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            if (Core.EngineVarList.SingleOrDefault(var => var.name == "globalon") != null)
            {
            }
            else
            {
                Core.EngineVarList.Add(new GlobalonVariable("1"));
            }
        }
    }
}