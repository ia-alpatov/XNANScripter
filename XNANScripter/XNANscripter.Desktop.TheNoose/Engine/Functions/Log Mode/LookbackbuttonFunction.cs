using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class LookbackbuttonFunction : Function
    {
        public string name { get { return "lookbackbutton"; } set { } }

        public List<string> parameters { get; set; }

        public LookbackbuttonFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
            mask.Add("$VAR");
            mask.Add("$VAR");
            mask.Add("$VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[4];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            LogMode.PageUpAc = await ValuesParser.GetSprite(parameters[0]);
            LogMode.PageUpInAc = await ValuesParser.GetSprite(parameters[1]);
            LogMode.PageDownAc = await ValuesParser.GetSprite(parameters[2]);
            LogMode.PageDownInAc = await ValuesParser.GetSprite(parameters[3]);

            parameters = null;
        }
    }
}