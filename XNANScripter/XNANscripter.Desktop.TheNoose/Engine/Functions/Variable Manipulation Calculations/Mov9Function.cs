using System;
using System.Collections.Generic;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Variables.Alias;

namespace XNANScripter.Engine.Functions
{
    internal class Mov9Function : Function
    {
        public string name { get { return "mov9"; } set { } }

        public List<string> parameters { get; set; }

        public Mov9Function()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[10];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            int j = 1;

            for (int i = int.Parse(parameters[0]); i < int.Parse(parameters[0]) + 9; i++)
            {
                NumVariable key = UserVariables.NumVarList.First(var => var.Key.number == i).Key;
                UserVariables.NumVarList[key] = int.Parse(ValuesParser.GetNumber(parameters[j]));
                j++;
            }

            parameters = null;
        }
    }
}