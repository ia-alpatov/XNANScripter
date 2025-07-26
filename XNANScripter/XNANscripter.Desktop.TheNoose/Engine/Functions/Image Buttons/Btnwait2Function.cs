using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class Btnwait2Function : Function
    {
        public string name { get { return "btnwait2"; } set { } }

        public List<string> parameters { get; set; }

        public Btnwait2Function()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
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
            UserButtons.WaitVariable = parameters[0];

            UserButtons.WaitForPress = true;
            Parsing.Wait = true;

            parameters = null;
        }
    }
}