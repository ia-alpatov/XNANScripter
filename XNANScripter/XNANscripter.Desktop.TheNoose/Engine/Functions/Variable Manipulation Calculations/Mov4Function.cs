﻿using System;
using System.Collections.Generic;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Variables.Alias;

namespace XNANScripter.Engine.Functions
{
    internal class Mov4Function : Function
    {
        public string name { get { return "mov4"; } set { } }

        public List<string> parameters { get; set; }

        public Mov4Function()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[5];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            int j = 1;

            for (int i = int.Parse(parameters[0]); i < int.Parse(parameters[0]) + 4; i++)
            {
                NumVariable key = UserVariables.NumVarList.First(var => var.Key.number == i).Key;
                UserVariables.NumVarList[key] = int.Parse(ValuesParser.GetNumber(parameters[j]));
                j++;
            }

            parameters = null;
        }
    }
}