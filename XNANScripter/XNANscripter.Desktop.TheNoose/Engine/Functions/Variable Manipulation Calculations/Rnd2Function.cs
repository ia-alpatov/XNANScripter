using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class Rnd2Function : Function
    {
        public string name { get { return "rnd2"; } set { } }

        public List<string> parameters { get; set; }

        public Rnd2Function()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[3];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            ValuesParser.SetNumber(parameters[0], new Random().Next(int.Parse(ValuesParser.GetNumber(parameters[1])), int.Parse(ValuesParser.GetNumber(parameters[2]))).ToString());

            parameters = null;
        }
    }
}