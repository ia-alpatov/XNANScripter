using System.Collections.Generic;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Variables;

namespace XNANScripter.Engine.Functions
{
    internal class EffectcutFunction : Function
    {
        public string name { get { return "effectcut"; } set { } }

        public List<string> parameters { get; set; }

        public EffectcutFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            Variable ecVar = Core.EngineVarList.SingleOrDefault(var => var.name == "effectcut");
            if (ecVar != null)
            {
                ecVar.Set(null);
            }
            else
            {
                Core.EngineVarList.Add(new EffectcutVariable(null));
            }

            parameters = null;
        }
    }
}