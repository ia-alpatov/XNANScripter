using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class MenusetwindowFunction : Function
    {
        public string name { get { return "menusetwindow"; } set { } }

        public List<string> parameters { get; set; }

        public MenusetwindowFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("COLOR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[7];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            RightMenu.TextFontWidth = int.Parse(ValuesParser.GetNumber(parameters[0]));
            RightMenu.TextFontHeight = int.Parse(ValuesParser.GetNumber(parameters[1]));
            RightMenu.TextSpacingX = int.Parse(ValuesParser.GetNumber(parameters[2]));
            RightMenu.TextSpacingY = int.Parse(ValuesParser.GetNumber(parameters[3]));
            RightMenu.BoldFace = (ValuesParser.GetNumber(parameters[4]) == "1");
            RightMenu.DropShadow = (ValuesParser.GetNumber(parameters[5]) == "1");

            RightMenu.WindowColor = ValuesParser.GetColor(parameters[6]);

            parameters = null;
        }
    }
}