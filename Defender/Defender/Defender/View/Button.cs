using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Defender.View
{
    enum MenuButtonState
    {
        MouseHover,
        MouseButtonUp,
        MouseButtonReleased,
        MouseButtonDown
    }

    class Button
    {
        private Texture2D texture;
        private Rectangle area;
        private Color color;

        private MouseState mouseState;
        private MenuButtonState state = new MenuButtonState();

        private bool currMouseState, prevMouseState = false;
        private bool mouseIsIntersecting;

        public Button(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            this.area = rectangle;
        }

        public bool ButtonUpdate()
        {
            mouseState = Mouse.GetState();
            prevMouseState = currMouseState;
            currMouseState = mouseState.LeftButton == ButtonState.Pressed;

            if (new Rectangle(mouseState.X, mouseState.Y, 1, 1).Intersects(area))
            {
                if (!mouseIsIntersecting)
                {
                    Game1.buttonHover.Play();
                    mouseIsIntersecting = true;
                }

                if (currMouseState)
                {
                    state = MenuButtonState.MouseButtonDown;
                    color = Color.LimeGreen;
                    return false;
                }
                else if (!currMouseState && prevMouseState)
                {
                    if (state == MenuButtonState.MouseButtonDown)
                    {
                        Game1.buttonClick.Play();
                        state = MenuButtonState.MouseButtonReleased;
                        return true;
                    }
                    return false;
                }
                else
                {
                    state = MenuButtonState.MouseHover;
                    color = Color.Green;
                    return false;
                }
            }
            else
            {
                state = MenuButtonState.MouseButtonUp;
                mouseIsIntersecting = false;
                color = Color.White;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, area, color);
        }
    }


}
