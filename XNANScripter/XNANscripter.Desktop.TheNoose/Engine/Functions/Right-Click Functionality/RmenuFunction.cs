using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class RmenuFunction : Function
    {
        public string name { get { return "rmenu"; } set { } }

        public List<string> parameters { get; set; }

        public RmenuFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
            mask.Add("(skip|reset|save|load|lookback|windowerase){0,1}");
            mask.Add("$VAR");
            mask.Add("((skip|reset|save|load|lookback|windowerase)|(?=,))");
            mask.Add("1");
            mask.Add("4");

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
            string key, title;

            for (int i = 0; i < parameters.Count - 1; i += 2)
            {
                title = ValuesParser.GetString(parameters[i]);
                key = parameters[i + 1];

                RightMenu.RightMenuList.Remove(key);

                RightMenu.RightMenuList.Add(key, title);
            }

            parameters = null;
        }
    }
}