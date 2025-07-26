using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class MonocroFunction : Function
    {
        public string name { get { return "monocro"; } set { } }

        public List<string> parameters { get; set; }

        public MonocroFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("COLOR|off");
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

        public async System.Threading.Tasks.Task Run()
        {
            if (parameters[0] == "off")
            {
                Drawing.TintColor = Color.White;
            }
            else
            {
                Drawing.TintColor = ValuesParser.GetColor(parameters[0]);
            }

            parameters = null;
        }
    }
}