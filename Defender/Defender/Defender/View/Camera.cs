using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defender.View
{
    class Camera
    {
        private float sizeScale;
        private float posScaleX;       
        private float posScaleY;

        public Camera(Viewport viewPort)
        {
            float sizeScaleX = viewPort.Width / Model.Level.SIZE_X;
            float sizeScaleY = viewPort.Height / Model.Level.SIZE_Y;

            sizeScale = sizeScaleX;
            if (sizeScaleY < sizeScaleX)
            {
                sizeScale = sizeScaleY;
            }

            posScaleX = viewPort.Width;
            posScaleY = viewPort.Height;
        }

        public float SizeScale
        {
            get { return sizeScale; }
            set { sizeScale = value; }
        }

        public float PosScaleX
        {
            get { return posScaleX; }
            set { posScaleX = value; }
        }

        public float PosScaleY
        {
            get { return posScaleY; }
            set { posScaleY = value; }
        }

        public Vector2 ToModelPosition(float x, float y)
        {
            float modelX = (x - sizeScale) / posScaleX;
            float modelY = (y - sizeScale) / posScaleY;

            Vector2 modelPos = new Vector2(modelX, modelY);

            return modelPos;
        }
    }
}
