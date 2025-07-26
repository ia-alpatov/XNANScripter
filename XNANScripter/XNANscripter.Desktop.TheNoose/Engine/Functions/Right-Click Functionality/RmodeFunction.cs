using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class RmodeFunction : Function
    {
        public string name { get { return "rmode"; } set { } }

        public List<string> parameters { get; set; }

        public RmodeFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
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
            ushort val = ushort.Parse(ValuesParser.GetNumber(parameters[0]));

            switch (val)
            {
                case 0:
                    RightMenu.IsOn = false;
                    break;

                case 1:
                    RightMenu.IsOn = true;
                    break;

                default:
                    throw new Exception("Неверные параметры функции.");
                    break;
            }

            parameters = null;
        }
    }
}