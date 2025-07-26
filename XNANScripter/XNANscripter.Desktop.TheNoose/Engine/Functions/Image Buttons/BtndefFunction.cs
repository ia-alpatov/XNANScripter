using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class BtndefFunction : Function
    {
        public string name { get { return "btndef"; } set { } }

        public List<string> parameters { get; set; }

        public BtndefFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[1];
            }
            else
            {
                mask.Add("(clear)");
                mask.Add("1");

                parameters = ValuesParser.GetParams(_parameters, mask);

                if (parameters != null)
                {
                    return parameters[1];
                }
                else
                {
                    throw new Exception("Неверные параметры функции.");
                }
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            if (parameters[0] == "clear" || parameters[0] == @"""""")
            {
                UserButtons.ButtonsBuffer.Dispose();
                UserButtons.ButtonsBuffer = null;
            }
            else
            {
                UserButtons.ButtonsBuffer = Config.System.Content.Load<Texture2D>(ValuesParser.GetString(parameters[0]));
            }

            parameters = null;
        }
    }
}