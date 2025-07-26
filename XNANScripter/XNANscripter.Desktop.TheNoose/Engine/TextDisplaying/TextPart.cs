using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace The_noose.Engine.TextDisplaying
{
    public class TextPart
    {
        public Color TextColor = Color.White;
        public string Text = string.Empty;
        public bool IsEndingWithNewLine = false;
        public bool IsEndingWithWait = false;
        public bool IsColoredPart = false;
        public bool IsBr = false;

        public Vector2 Position;
    }
}
