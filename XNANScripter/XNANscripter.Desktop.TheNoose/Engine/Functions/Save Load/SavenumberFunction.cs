using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class SavenumberFunction : Function
    {
        public string name { get { return "savenumber"; } set { } }

        public List<string> parameters { get; set; }

        public SavenumberFunction()
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
            SaveSystem.SaveNumber = int.Parse(ValuesParser.GetNumber(parameters[0]));

            parameters = null;
        }
    }
}