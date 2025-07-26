using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class LocateFunction : Function
    {
        public string name { get { return "locate"; } set { } }

        public List<string> parameters { get; set; }

        public LocateFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[2];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            TextWindow.TextPointer = new Microsoft.Xna.Framework.Vector2(float.Parse(ValuesParser.GetNumber(parameters[0])), float.Parse(ValuesParser.GetNumber(parameters[1])));

            TextWindow.TextCurrentPosition.X = 0;

            parameters = null;
        }
    }
}