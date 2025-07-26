using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class MovlFunction : Function
    {
        public string name { get { return "movl"; } set { } }

        public List<string> parameters { get; set; }

        public MovlFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add(@"(\?[a-zA-Z0-9_]+(\[[0-9]+\]){0,2})");
            mask.Add("%VAR");
            mask.Add("0");
            mask.Add("3");

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
            string arrName = parameters[0].Remove(parameters[0].IndexOf('[')).Remove(0, 1);

            int n1;

            n1 = int.Parse(parameters[0].Remove(0, parameters[0].IndexOf('[') + 1).Remove(parameters[0].IndexOf(']')));

            for (int i = 0; i < parameters.Count - 3; i++)
            {
                UserVariables.ArrayVarList[arrName][n1, i] = int.Parse(ValuesParser.GetNumber(parameters[i + 1]));
            }

            parameters = null;
        }
    }
}