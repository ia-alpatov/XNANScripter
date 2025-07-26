using System.Collections.Generic;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Variables;

namespace XNANScripter.Engine.Functions
{
    internal class NsaFunction : Function
    {
        public string name { get { return "nsa"; } set { } }

        public List<string> parameters { get; set; }

        public NsaFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            Variable nsaVar = Core.EngineVarList.SingleOrDefault(var => var.name == "nsa");
            if (nsaVar != null)
            {
                nsaVar.Set("1");
            }
            else
            {
                Core.EngineVarList.Add(new NsaVariable(1));
            }
        }
    }
}