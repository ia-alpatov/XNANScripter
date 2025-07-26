using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class IntlimitFunction : Function
    {
        public string name { get { return "intlimit"; } set { } }

        public List<string> parameters { get; set; }

        public IntlimitFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[3];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            int value = int.Parse(ValuesParser.GetNumber(parameters[0]));

            if (value < int.Parse(ValuesParser.GetNumber(parameters[1])))
            {
                ValuesParser.SetNumber(parameters[0], parameters[1]);
            }
            else
            {
                if (value > int.Parse(ValuesParser.GetNumber(parameters[2])))
                {
                    ValuesParser.SetNumber(parameters[0], parameters[1]);
                }
            }

            parameters = null;
        }
    }
}