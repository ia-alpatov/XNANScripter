using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace XNANScripter.Engine.Sprites
{
    public class Sprite
    {
        public bool IsHidden;

        public int Type;

        public List<Texture2D> texturesList = new List<Texture2D>();

        public bool IsAnimated;

        public int CellNumber;
        public bool IsDelayFull;

        public List<int> delayList = new List<int>();

        public int LoopType;

        public Microsoft.Xna.Framework.Rectangle Rectangle;

        public char TransparencyType;
    }
}