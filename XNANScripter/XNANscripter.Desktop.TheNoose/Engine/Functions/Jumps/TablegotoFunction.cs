using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class TablegotoFunction : Function
    {
        public string name { get { return "tablegoto"; } set { } }

        public List<string> parameters { get; set; }

        public TablegotoFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
            mask.Add("LABEL");
            mask.Add("LABEL");
            mask.Add("1");
            mask.Add("4");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                throw new NotImplementedException();

                return null;
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            parameters = null;
        }
    }
}