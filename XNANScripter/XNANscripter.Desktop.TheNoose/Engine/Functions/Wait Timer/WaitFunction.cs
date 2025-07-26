using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class WaitFunction : Function
    {
        public string name { get { return "wait"; } set { } }

        public List<string> parameters { get; set; }

        public WaitFunction()
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

            Delay.StartOnTouch = false;

            Parsing.Wait = true;

            parameters = null;
        }
    }
}