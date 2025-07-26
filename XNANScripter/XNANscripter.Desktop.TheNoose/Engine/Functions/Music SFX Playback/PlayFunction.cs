using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class PlayFunction : Function
    {
        public string name { get { return "play"; } set { } }

        public List<string> parameters { get; set; }

        public PlayFunction()
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
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            string param = ValuesParser.GetString(parameters[0]);

            if (param[0] == '*')
            {
                throw new Exception("Невозможно выполнить функцию");
            }
            else
            {
                Sound.soundmanager.Play(1, 0, param, true);
            }

            parameters = null;
        }
    }
}