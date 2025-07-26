using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class IncFunction : Function
    {
        public string name { get { return "inc"; } set { } }

        public List<string> parameters { get; set; }

        public IncFunction()
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
            int value = int.Parse(ValuesParser.GetNumber(parameters[0]));
            value++;
            ValuesParser.SetNumber(parameters[0], value.ToString());

            parameters = null;
        }
    }
}