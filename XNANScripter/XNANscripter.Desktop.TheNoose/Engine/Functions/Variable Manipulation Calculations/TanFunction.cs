using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class TanFunction : Function
    {
        public string name { get { return "tan"; } set { } }

        public List<string> parameters { get; set; }

        public TanFunction()
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
                return parameters[parameters.Count - 1];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            ValuesParser.SetNumber(parameters[0], (Math.Tan(double.Parse(ValuesParser.GetNumber(parameters[1]))) * 1000).ToString());

            parameters = null;
        }
    }
}