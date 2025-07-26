using System;
using System.Collections.Generic;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Procedures;

namespace XNANScripter.Engine.Functions
{
    internal class GosubFunction : Function
    {
        public string name { get { return "gosub"; } set { } }

        public List<string> parameters { get; set; }

        public GosubFunction()
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
            //Переходим к метке и возвращаемся к обратно при return
            //Записываем индекс строчки, оставшуюся её часть для последующего выполнения
            Parsing.SubroutinesLevels.Push(new SubroutineLevel(Parsing.CurrentLine, parameters[1]));
            //Ставим указатель на начало положение метки
            Parsing.CurrentLine = UserDefine.ProceduresList.First(var => var.name == parameters[0]).startPoint;

            parameters = null;
        }
    }
}