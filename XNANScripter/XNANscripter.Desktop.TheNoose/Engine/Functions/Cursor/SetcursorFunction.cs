using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Sprites;

namespace XNANScripter.Engine.Functions
{
    internal class SetcursorFunction : Function
    {
        public string name { get { return "setcursor"; } set { } }

        public List<string> parameters { get; set; }

        public SetcursorFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("$VAR");
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
            ushort val = ushort.Parse(ValuesParser.GetNumber(parameters[0]));

            Sprite spr = await ValuesParser.GetSprite(parameters[1]);

            switch (val)
            {
                case 0:
                    TextWindow.ClickWaitCursor = spr;
                    TextWindow.ClickWaitCursorVector = new Vector2(float.Parse(parameters[2]), float.Parse(parameters[3]));
                    break;

                case 1:
                    TextWindow.PageWaitCursor = spr;
                    TextWindow.PageWaitCursorVector = new Vector2(float.Parse(parameters[2]), float.Parse(parameters[3]));
                    break;

                default:
                    throw new Exception("Неверные параметры функции.");
                    break;
            }

            parameters = null;
        }
    }
}