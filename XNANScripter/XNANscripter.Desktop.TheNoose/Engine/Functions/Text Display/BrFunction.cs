using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.TextDisplaying;
using The_noose.Engine.TextDisplaying;
using Microsoft.Xna.Framework;

namespace XNANScripter.Engine.Functions
{
    internal class BrFunction : Function
    {
        public string name { get { return "br"; } set { } }

        public List<string> parameters { get; set; }

        public BrFunction()
        {
        }

        public string Parse(string _parameters)
        {
            parameters = new List<string>();
            parameters.Add(_parameters);
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            switch (TextWindow.TextShowMode)
            {
                case 1:
                    TextWindow.CharMode1.Add(new StyledChar(Environment.NewLine, TextWindow.TextColor));
                    break;

                case 2:
                    TextWindow.TextPointer.Y++;
                    break;
            }

            if (parameters.Count > 0 && parameters[0].Trim() == @"\")
            {
                TextWindow.TextParts.Add(new TextPart() { IsBr = true, IsEndingWithNewLine = true });
            }
            else
            {
                TextWindow.TextParts.Add(new TextPart() { IsBr = true,IsEndingWithWait = true});
            }

            TextWindow.TextPointer = Vector2.Zero;
            TextWindow.IsShowing = true;
            Parsing.Wait = true;

            parameters = null;
        }
    }
}