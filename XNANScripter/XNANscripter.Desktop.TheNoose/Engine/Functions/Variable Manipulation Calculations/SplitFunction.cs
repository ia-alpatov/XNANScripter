using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class SplitFunction : Function
    {
        public string name { get { return "split"; } set { } }

        public List<string> parameters { get; set; }

        public SplitFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
            mask.Add("$VAR");
            mask.Add("$VAR|%VAR");
            mask.Add("$VAR|%VAR");
            mask.Add("$VAR|%VAR");
            mask.Add("3");
            mask.Add("4");

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
            int count = parameters.Count - 3;

            string value = ValuesParser.GetString(parameters[0]);
            char ch = ValuesParser.GetString(parameters[1])[0];
            string tempValue;

            for (int i = 2; i < count; i++)
            {
                int index = value.IndexOf(ch);

                if (index != -1)
                {
                    tempValue = value.Substring(0, index + 1);
                    value = value.Remove(0, index + 1);
                    //Определяем тип переменной
                    switch (parameters[i][0])
                    {
                        case '$':
                            UserVariables.StringVarList[parameters[i].Remove(0, 1)] = tempValue;
                            break;

                        default:
                            ValuesParser.SetNumber(parameters[i], int.Parse(tempValue).ToString());
                            break;
                    }
                }
                else
                {
                    //Определяем тип переменной
                    switch (parameters[i][0])
                    {
                        case '$':
                            UserVariables.StringVarList[parameters[i].Remove(0, 1)] = value;
                            break;

                        default:
                            ValuesParser.SetNumber(parameters[i], "0");
                            break;
                    }
                }
            }

            parameters = null;
        }
    }
}