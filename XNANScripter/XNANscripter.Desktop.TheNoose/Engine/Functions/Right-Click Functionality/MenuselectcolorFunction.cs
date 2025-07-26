using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class MenuselectcolorFunction : Function
    {
        public string name { get { return "menuselectcolor"; } set { } }

        public List<string> parameters { get; set; }

        public MenuselectcolorFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("COLOR");
            mask.Add("COLOR");
            mask.Add("COLOR");
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
            RightMenu.MouseoverColor = ValuesParser.GetColor(parameters[0]);
            RightMenu.MouseoffColor = ValuesParser.GetColor(parameters[1]);
            RightMenu.EmptySavefileColor = ValuesParser.GetColor(parameters[2]);

            parameters = null;
        }
    }
}