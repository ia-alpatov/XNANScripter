using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class MovFunction : Function
    {
        public string name { get { return "mov"; } set { } }

        public List<string> parameters { get; set; }

        public MovFunction()
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
                parameters[2] = "1";
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
                ValuesParser.SetNumber(parameters[0], parameters[1]);
            }
            else
            {
                UserVariables.StringVarList[parameters[0]] = ValuesParser.GetString(parameters[1]);
            }

            parameters = null;
        }
    }
}