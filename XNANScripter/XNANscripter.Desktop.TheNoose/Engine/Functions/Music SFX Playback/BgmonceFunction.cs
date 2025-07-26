﻿using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class BgmonceFunction : Function
    {
        public string name { get { return "bgmonce"; } set { } }

        public List<string> parameters { get; set; }

        public BgmonceFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
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
            Sound.soundmanager.Play(5, 0, ValuesParser.GetString(parameters[0]), false);

            parameters = null;
        }
    }
}