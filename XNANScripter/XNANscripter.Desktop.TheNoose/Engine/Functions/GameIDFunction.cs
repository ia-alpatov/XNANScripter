using System;
using System.Collections.Generic;
using System.Text;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class GameIDFunction : Function
    {
        public string name { get { return "gameid"; } set { } }

        public List<string> parameters { get; set; }

        public GameIDFunction()
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
                return parameters[parameters.Count - 1];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            Core.GameId = ValuesParser.GetString(parameters[0]);
            parameters = null;
        }
    }
}
