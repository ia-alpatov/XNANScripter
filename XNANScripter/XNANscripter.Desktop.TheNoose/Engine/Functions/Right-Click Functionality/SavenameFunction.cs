using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class SavenameFunction : Function
    {
        public string name { get { return "savename"; } set { } }

        public List<string> parameters { get; set; }

        public SavenameFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
            mask.Add("$VAR");
            mask.Add("$VAR");
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
            SaveSystem.SaveMenuTitle = ValuesParser.GetString(parameters[0]);
            SaveSystem.LoadMenuTitle = ValuesParser.GetString(parameters[1]);
            SaveSystem.SlotTitle = ValuesParser.GetString(parameters[2]);

            parameters = null;
        }
    }
}