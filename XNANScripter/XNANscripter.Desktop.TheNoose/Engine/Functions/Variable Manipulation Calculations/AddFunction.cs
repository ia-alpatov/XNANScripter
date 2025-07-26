using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class AddFunction : Function
    {
        public string name { get { return "add"; } set { } }

        public List<string> parameters { get; set; }

        public AddFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                string value = parameters[2];
                parameters[2] = "2";
                return value;
            }
            else
            {
                mask = new List<string>();

                mask.Add("$VAR");
                mask.Add("$VAR");
                mask.Add("1");

                parameters = ValuesParser.GetParams(_parameters, mask);

                if (parameters != null)
                {
                    string value = parameters[2];
                    parameters[2] = "2";
                    return value;
                }
                else
                {
                    throw new Exception("Неверные параметры функции.");
                }
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            if (parameters[2] == "1")
            {
                int value = int.Parse(ValuesParser.GetNumber(parameters[0]));
                value += int.Parse(ValuesParser.GetNumber(parameters[1]));
                ValuesParser.SetNumber(parameters[0], value.ToString());
            }
            else
            {
                UserVariables.StringVarList[parameters[0].Remove(0, 1)] += ValuesParser.GetString(parameters[1]);
            }

            parameters = null;
        }
    }
}