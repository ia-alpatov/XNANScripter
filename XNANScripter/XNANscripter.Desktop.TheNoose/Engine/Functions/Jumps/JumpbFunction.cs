using System.Collections.Generic;

using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class JumpbFunction : Function
    {
        public string name { get { return "jumpb"; } set { } }

        public List<string> parameters { get; set; }

        public JumpbFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return null;
        }

        public async System.Threading.Tasks.Task Run()
        {
            //Идём вверх до первой процедуры и ищем ~, если такой знак не встретиться, то запускаем заново
            for (ushort i = Parsing.CurrentLine; i >= 0; i--)
            {
                Parsing.CurrentLine = i;

                if (Parsing.ScriptText[i].Length > 0 && Parsing.ScriptText[i].Trim()[0] == '~')
                {
                    break;
                }
            }

            //Проверяем в функции ли мы, если да то не сбрасываем параметры до выхода вверх или вниз
            //Проверяем не зашли ли мы за переделы game

            parameters = null;
        }
    }
}