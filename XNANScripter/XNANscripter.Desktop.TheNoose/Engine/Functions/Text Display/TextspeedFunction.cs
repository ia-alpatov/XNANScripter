using System;
using System.Collections.Generic;
using System.Linq;

using XNANScripter.Engine.Config;
using XNANScripter.Engine.Variables;

namespace XNANScripter.Engine.Functions
{
    internal class TextspeedFunction : Function
    {
        public string name { get { return "textspeed"; } set { } }

        public List<string> parameters { get; set; }

        public TextspeedFunction()
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
            if (Core.EngineVarList.SingleOrDefault(var => var.name == "textspeed") != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                Core.EngineVarList.Add(new TextspeedVariable(ValuesParser.GetNumber(parameters[0])));
            }
        }
    }
}