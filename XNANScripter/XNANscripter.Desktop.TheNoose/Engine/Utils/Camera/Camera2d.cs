using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_noose.Engine.Utils.Camera
{
    public class Camera2D
    {
        public float Zoom { get; set; }
        public Vector2 Location { get; set; }
        public float Rotation { get; set; }

        private Rectangle Bounds { get; set; }

        public Matrix TransformMatrix
        {
            get
            {

                return Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0))*
                       Matrix.CreateRotationZ(Rotation)*
                       Matrix.CreateScale(Zoom);
            }
        }

        public Camera2D(Viewport viewport)
        {
            Bounds = viewport.Bounds;
            Zoom = 1;
        }
    }
}
