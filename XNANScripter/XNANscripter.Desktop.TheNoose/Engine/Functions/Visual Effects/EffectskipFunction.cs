using System;
using System.Collections.Generic;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Variables;

namespace XNANScripter.Engine.Functions
{
    internal class EffectskipFunction : Function
    {
        public string name { get { return "effectskip"; } set { } }

        public List<string> parameters { get; set; }

        public EffectskipFunction()
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
            Variable esVar = Core.EngineVarList.SingleOrDefault(var => var.name == "effectskip");
            if (esVar != null)
            {
                esVar.Set(ValuesParser.GetNumber(parameters[0]));
            }
            else
            {
                Core.EngineVarList.Add(new EffectskipVariable(ValuesParser.GetNumber(parameters[0])));
            }

            parameters = null;
        }
    }
}