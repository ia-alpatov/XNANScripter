using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class DimFunction : Function
    {
        public string name { get { return "dim"; } set { } }

        public List<string> parameters { get; set; }

        public DimFunction()
        {
        }

        public string Parse(string _parameters)
        {
            parameters = new List<string>();

            string regExpr = @"^[\s]*\?[a-zA-Z0-9]+\[[0-9]+\](?!\[)";
            System.Text.RegularExpressions.Match matcher = System.Text.RegularExpressions.Regex.Match(_parameters, regExpr);

            if (matcher.Success)
            {
                string value = matcher.Value.Trim().Remove(0, 1);

                string name = null, count = null;
                int j = 0;

                //Или использовать регулярки
                while (true)
                {
                    if (value[j] != '[')
                    {
                        name += value[j];
                        j++;
                    }
                    else
                    {
                        break;
                    }
                }

                parameters.Add(name);

                j++;

                while (true)
                {
                    if (value[j] != ']')
                    {
                        count += value[j];
                        j++;
                    }
                    else
                    {
                        break;
                    }
                }

                parameters.Add(count);

                return _parameters.Remove(0, value.Length + 1);
            }
            else
            {
                regExpr = @"^[\s]*\?[a-zA-Z0-9]+\[[0-9]+\]\[[0-9]+\]";
                matcher = System.Text.RegularExpressions.Regex.Match(_parameters, regExpr);

                if (matcher.Success)
                {
                    string value = matcher.Value.Trim().Remove(0, 1);

                    string name = null, count1 = null, count2 = null;
                    int j = 0;

                    //Или использовать регулярки
                    while (true)
                    {
                        if (value[j] != '[')
                        {
                            name += value[j];
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    parameters.Add(name);
                    j++;

                    while (true)
                    {
                        if (value[j] != ']')
                        {
                            count1 += value[j];
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    parameters.Add(count1);
                    j += 2;

                    while (true)
                    {
                        if (value[j] != ']')
                        {
                            count2 += value[j];
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    parameters.Add(count2);

                    return _parameters.Remove(0, value.Length + 1);
                }
                else
                {
                    throw new Exception("Неверные параметры функции.");
                }
            }
        }

        //Цифры не могут быть переменными и длина массива не больше двух
        public async System.Threading.Tasks.Task Run()
        {
            if (parameters.Count == 2)
            {
                UserVariables.ArrayVarList.Add(parameters[0], new int[1, int.Parse(parameters[1]) + 1]);
            }
            else
            {
                UserVariables.ArrayVarList.Add(parameters[0], new int[int.Parse(parameters[1]) + 1, int.Parse(parameters[2]) + 1]);
            }

            parameters = null;
        }
    }
}