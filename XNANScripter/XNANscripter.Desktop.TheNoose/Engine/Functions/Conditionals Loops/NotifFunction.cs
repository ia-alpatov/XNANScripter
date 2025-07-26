using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class NotifFunction : Function
    {
        public string name { get { return "notif"; } set { } }

        public List<string> parameters { get; set; }

        public NotifFunction()
        {
        }

        public string Parse(string _parameters)
        {
            //Получаем результат условия
            List<string> conditionsResult = ValuesParser.GetConditionResult(_parameters);
            bool result = bool.Parse(conditionsResult[0]);

            //Проверяем результат
            if (result == false)
            {
                return conditionsResult[1];
            }
            else
            {
                return null;
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            parameters = null;
        }
    }
}