using System;
using System.Collections.Generic;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Variables;

namespace XNANScripter.Engine.Functions
{
    internal class EffectblankFunction : Function
    {
        public string name { get { return "effectblank"; } set { } }

        public List<string> parameters { get; set; }

        public EffectblankFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[1];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            Variable ebVar = Core.EngineVarList.SingleOrDefault(var => var.name == "effectblank");
            if (ebVar != null)
            {
                ebVar.Set(ValuesParser.GetNumber(parameters[0]));
            }
            else
            {
                Core.EngineVarList.Add(new EffectblankVariable(ValuesParser.GetNumber(parameters[0])));
            }

            parameters = null;
        }
    }
}