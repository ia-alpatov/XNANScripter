using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class SelectcolorFunction : Function
    {
        public string name { get { return "selectcolor"; } set { } }

        public List<string> parameters { get; set; }

        public SelectcolorFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("COLOR");
            mask.Add("COLOR");
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
            Choices.MouseoverColor = ValuesParser.GetColor(parameters[0]);
            Choices.MouseoffColor = ValuesParser.GetColor(parameters[1]);

            parameters = null;
        }
    }
}