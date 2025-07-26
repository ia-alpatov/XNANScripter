﻿using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class RndFunction : Function
    {
        public string name { get { return "rnd"; } set { } }

        public List<string> parameters { get; set; }

        public RndFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[2];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            ValuesParser.SetNumber(parameters[0], new Random().Next(int.Parse(ValuesParser.GetNumber(parameters[1]))).ToString());

            parameters = null;
        }
    }
}