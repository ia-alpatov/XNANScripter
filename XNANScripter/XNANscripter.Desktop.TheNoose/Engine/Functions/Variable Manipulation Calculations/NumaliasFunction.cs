using System;
using System.Collections.Generic;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Variables.Alias;

namespace XNANScripter.Engine.Functions
{
    internal class NumaliasFunction : Function
    {
        public string name { get { return "numalias"; } set { } }

        public List<string> parameters { get; set; }

        public NumaliasFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("NAME");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters.Trim(), mask);

            if (parameters != null)
            {
                return parameters[2];
            }
            else
            {
                throw new Exception("Неверные параметры фунции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            int number = int.Parse(parameters[1]);

            NumVariable key = UserVariables.NumVarList.First(var => var.Key.number == number).Key;
            UserVariables.NumVarList.Remove(key);

            UserVariables.NumVarList.Add(new NumVariable(parameters[0], number), 0);

            parameters = null;
        }
    }
}