using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class MidFunction : Function
    {
        public string name { get { return "mid"; } set { } }

        public List<string> parameters { get; set; }

        public MidFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
            mask.Add("$VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[4];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            UserVariables.StringVarList[parameters[0].Remove(0, 1)] = ValuesParser.GetString(parameters[1]).Substring(int.Parse(ValuesParser.GetNumber(parameters[2])), int.Parse(ValuesParser.GetNumber(parameters[3])));

            parameters = null;
        }
    }
}