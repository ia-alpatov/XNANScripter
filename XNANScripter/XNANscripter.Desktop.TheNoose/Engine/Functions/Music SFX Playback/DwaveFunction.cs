using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class DwaveFunction : Function
    {
        public string name { get { return "dwave"; } set { } }

        public List<string> parameters { get; set; }

        public DwaveFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("$VAR");
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
            Sound.soundmanager.Play(4, int.Parse(ValuesParser.GetNumber(parameters[0])), ValuesParser.GetString(parameters[1]), false);

            parameters = null;
        }
    }
}