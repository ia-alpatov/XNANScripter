using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class DelayFunction : Function
    {
        public string name { get { return "delay"; } set { } }

        public List<string> parameters { get; set; }

        public DelayFunction()
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
            Delay.TimeToSleep = ushort.Parse(ValuesParser.GetNumber(parameters[0]));

            Delay.Sleeping = true;

            Delay.StartOnTouch = true;

            Parsing.Wait = true;

            parameters = null;
        }
    }
}