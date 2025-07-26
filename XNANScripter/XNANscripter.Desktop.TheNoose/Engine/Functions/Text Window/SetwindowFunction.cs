using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class SetwindowFunction : Function
    {
        public string name { get { return "setwindow"; } set { } }

        public List<string> parameters { get; set; }

        public SetwindowFunction()
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
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("COLOR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[16];
            }
            else
            {
                mask.Clear();

                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("$VAR");
                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("1");

                parameters = ValuesParser.GetParams(_parameters, mask);

                if (parameters != null)
                {
                    return parameters[14];
                }
                else
                {
                    throw new Exception("Неверные параметры функции.");
                }
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            //http://unclemion.com/onscripter/api/NScrAPI.html#setwindow

            //TextWindow.TextBackgroundRectangle = new Rectangle((int)(int.Parse(ValuesParser.GetNumber(parameters[0])) * Drawing.backgroundScale), (int)(int.Parse(ValuesParser.GetNumber(parameters[1])) * Drawing.backgroundScale),,);

            parameters = null;
        }
    }
}