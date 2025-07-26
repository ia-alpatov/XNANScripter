using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class VersionstrFunction : Function
    {
        public string name { get { return "versionstr"; } set { } }

        public List<string> parameters { get; set; }

        public VersionstrFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
            mask.Add("$VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters.TrimStart(), mask);

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
            parameters = null;
        }
    }
}