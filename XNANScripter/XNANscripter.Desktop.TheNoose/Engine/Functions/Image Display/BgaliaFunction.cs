using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class BgaliaFunction : Function
    {
        public string name { get { return "bgalia"; } set { } }

        public List<string> parameters { get; set; }

        public BgaliaFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[4];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            Drawing.BackgroundRectangle = new Rectangle(int.Parse(ValuesParser.GetNumber(parameters[0])), int.Parse(ValuesParser.GetNumber(parameters[1])), int.Parse(ValuesParser.GetNumber(parameters[2])), int.Parse(ValuesParser.GetNumber(parameters[3])));

            parameters = null;
        }
    }
}