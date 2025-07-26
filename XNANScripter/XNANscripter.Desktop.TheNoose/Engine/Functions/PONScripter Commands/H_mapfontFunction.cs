using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class H_mapfontFunction : Function
    {
        public string name { get { return "h_mapfont"; } set { } }

        public List<string> parameters { get; set; }

        public H_mapfontFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
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
            TextWindow.FontsList.Add(int.Parse(ValuesParser.GetNumber(parameters[0])), ValuesParser.GetString(parameters[1]));

            parameters = null;
        }
    }
}