using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Procedures;

namespace XNANScripter.Engine.Functions
{
    internal class ReturnFunction : Function
    {
        public string name { get { return "return"; } set { } }

        public List<string> parameters { get; set; }

        public ReturnFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("LABEL");
            mask.Add("-1");
            mask.Add("2");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                //Если нет параметра, то возвращаемся к строке
                //А если есть, то удаляем и создаём
                if (parameters.Count == 2)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    SubroutineLevel sub = Parsing.SubroutinesLevels.Pop();
                    Parsing.CurrentLine = sub.StoppedLine;
                    return sub.LeftoverValue;
                }
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            parameters = null;
        }
    }
}