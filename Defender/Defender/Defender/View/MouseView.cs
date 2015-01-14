using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Defender.View
{
    class MouseView
    {
        private MouseState prevMouseState;
        private MouseState currMouseState;

        Camera camera;

        public MouseView(Camera camera)
        {
            this.camera = camera;
        }

        public bool IsButtonPressed()
        {
            currMouseState = Mouse.GetState();
            bool isClicked = false;

            if(currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                isClicked = true;
            }

            prevMouseState = currMouseState;

            return isClicked;
        }

        public Vector2 GetMousePosition()
        {
            currMouseState = Mouse.GetState();

            float x = currMouseState.X;
            float y = currMouseState.Y;

            Vector2 mouseModelPos = camera.ToModelPosition(x, y);

            return mouseModelPos;
        }
    }
}
