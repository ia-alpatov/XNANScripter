using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Window_Menubar;

namespace XNANScripter.Engine.Functions
{
    internal class InsertmenuFunction : Function
    {
        public string name { get { return "insertmenu"; } set { } }

        public List<string> parameters { get; set; }

        public InsertmenuFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
            mask.Add("NAME");
            mask.Add("%VAR");
            mask.Add("1");
            mask.Add("2");

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
            int level = 0;
            int.TryParse(parameters[2], out level);
            TopMenu.MenuList.Add(new MenuItem(ValuesParser.GetString(parameters[0]), parameters[1], level));

            parameters = null;
        }
    }
}