using System;
using System.Collections.Generic;
using System.Linq;

using XNANScripter.Engine.Config;
using XNANScripter.Engine.Variables;

namespace XNANScripter.Engine.Functions
{
    internal class Automode_timeFunction : Function
    {
        public string name { get { return "automode_time"; } set { } }

        public List<string> parameters { get; set; }

        public Automode_timeFunction()
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
            if (Core.EngineVarList.SingleOrDefault(var => var.name == "automode_time") != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                Core.EngineVarList.Add(new Automode_timeVariable(ValuesParser.GetNumber(parameters[0])));
            }
        }
    }
}