using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class Itoa2Function : Function
    {
        public string name { get { return "itoa2"; } set { } }

        public List<string> parameters { get; set; }

        public Itoa2Function()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
            mask.Add("$VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[2];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            UserVariables.StringVarList[parameters[0].Remove(0, 1)] = ValuesParser.GetNumber(parameters[1]);

            parameters = null;
        }
    }
}