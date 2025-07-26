using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class DivFunction : Function
    {
        public string name { get { return "div"; } set { } }

        public List<string> parameters { get; set; }

        public DivFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
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
            int value = int.Parse(ValuesParser.GetNumber(parameters[0]));
            value /= int.Parse(ValuesParser.GetNumber(parameters[1]));
            ValuesParser.SetNumber(parameters[0], value.ToString());

            parameters = null;
        }
    }
}