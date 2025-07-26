using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class SkipFunction : Function
    {
        public string name { get { return "skip"; } set { } }

        public List<string> parameters { get; set; }

        public SkipFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add(@"([\+-]{0,1}(%[a-zA-Z0-9_]+[\+\*\\-]{0,1}|\?[a-zA-Z0-9_]+\[[0-9]+\](\[[0-9]+\]){0,1}[\+\*\\-]{0,1}|[0-9]+[\+\*\\-]{0,1}|0x[a-zA-Z0-9]+[\+\*\\-]{0,1})+");
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
            //В зависимости от параметра поднимаем или опускаем указатель на n
            switch (parameters[0][0])
            {
                case '-':
                    Parsing.CurrentLine -= ushort.Parse(ValuesParser.GetNumber(parameters[0].Remove(0, 1)));
                    break;

                case '+':
                    Parsing.CurrentLine += ushort.Parse(ValuesParser.GetNumber(parameters[0].Remove(0, 1)));
                    break;

                default:
                    Parsing.CurrentLine += ushort.Parse(ValuesParser.GetNumber(parameters[0]));
                    break;
            }

            //Проверяем в функции ли мы, если да то не сбрасываем параметры до выхода вверх или вниз
            //Проверяем не зашли ли мы за переделы game

            parameters = null;
        }
    }
}