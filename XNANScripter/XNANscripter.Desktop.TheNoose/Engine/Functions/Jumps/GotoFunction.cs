using System;
using System.Collections.Generic;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Procedures;

namespace XNANScripter.Engine.Functions
{
    internal class GotoFunction : Function
    {
        public string name { get { return "goto"; } set { } }

        public List<string> parameters { get; set; }

        public GotoFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("LABEL");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return null;
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            //Устанавливаем указатель

            try
            {
                Parsing.CurrentLine = UserDefine.ProceduresList.First(var => var.name == parameters[0]).startPoint;
            }
            catch (InvalidOperationException)
            {
                ushort line = ushort.Parse(UserDefine.ProceduresList[UserDefine.ProceduresList.Count - 1].startPoint.ToString());
                line++;

                while (true)
                {
                    line++;

                    if (Parsing.ScriptText[line].Length != 0 && Parsing.ScriptText[line].TrimStart().StartsWith("*"))
                    {
                        //Регуляркой получаем название (можно будет заменить простым циклом)
                        matcher = System.Text.RegularExpressions.Regex.Match(Parsing.ScriptText[line], @"^[\s]*\*[A-Za-z0-9_]+");

                        if (matcher.Success)
                        {
                            ushort start = line;
                            string value = matcher.Value.Trim();

                            if (value == parameters[0])
                            {
                                UserDefine.ProceduresList.Add(new Procedure(value, start, line));
                                line++;

                                Parsing.CurrentLine = line;
                                break;
                            }

                            /*
                            while (true)
                            {
                                count++;

                                if (count == LinesCount)
                                {
                                    HasProcedures = false;
                                    break;
                                }

                                if (Parsing.ScriptText[count].Length != 0)
                                {
                                    matcher = System.Text.RegularExpressions.Regex.Match(Parsing.ScriptText[count], @"^[\s]*\*[A-Za-z0-9_]+");

                                    if (matcher.Success)
                                    {
                                        count--;
                                        break;
                                    }
                                }
                            }
                             */
                            UserDefine.ProceduresList.Add(new Procedure(value, start, line));

                            line++;
                        }
                    }
                }
            }
            //Проверяем в функции ли мы, если да то не сбрасываем параметры до выхода вверх или вниз
            //Проверяем не зашли ли мы за переделы game

            parameters = null;
        }

        public System.Text.RegularExpressions.Match matcher { get; set; }
    }
}