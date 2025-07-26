using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class LookbackcolorFunction : Function
    {
        public string name { get { return "lookbackcolor"; } set { } }

        public List<string> parameters { get; set; }

        public LookbackcolorFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("COLOR");
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
            LogMode.TextColor = ValuesParser.GetColor(parameters[0]);

            parameters = null;
        }
    }
}