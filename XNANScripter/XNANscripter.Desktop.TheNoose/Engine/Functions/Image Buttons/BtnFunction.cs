using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using XNANScripter.Engine.Buttons;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class BtnFunction : Function
    {
        public string name { get { return "btn"; } set { } }

        public List<string> parameters { get; set; }

        public BtnFunction()
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
            mask.Add("%VAR");
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
            int key = int.Parse(ValuesParser.GetNumber(parameters[0]));

            UserButton btn = new UserButton();

            btn.Rectangle = new Rectangle(int.Parse(ValuesParser.GetNumber(parameters[1])), int.Parse(ValuesParser.GetNumber(parameters[2])), int.Parse(ValuesParser.GetNumber(parameters[3])), int.Parse(ValuesParser.GetNumber(parameters[4])));

            btn.XShift = int.Parse(ValuesParser.GetNumber(parameters[5]));
            btn.YShift = int.Parse(ValuesParser.GetNumber(parameters[6]));

            UserButtons.ButtonsList.Add(key, btn);

            parameters = null;
        }
    }
}