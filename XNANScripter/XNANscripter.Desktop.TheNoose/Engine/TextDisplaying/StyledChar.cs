using Microsoft.Xna.Framework;

namespace XNANScripter.Engine.TextDisplaying
{
    public class StyledChar
    {
        public string C = " ";

        public Color CharColor;

        public StyledChar(char p, Microsoft.Xna.Framework.Color color)
        {
            this.C = p.ToString();
            CharColor = color;
        }

        public StyledChar()
        {
        }

        public StyledChar(string p, Microsoft.Xna.Framework.Color color)
        {
            this.C = p;
            CharColor = color;
        }

        internal void SetColor(Microsoft.Xna.Framework.Color color)
        {
            CharColor = color;
        }
    }
}