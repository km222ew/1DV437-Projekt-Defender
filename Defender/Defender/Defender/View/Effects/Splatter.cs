using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Defender.View.Effects
{
    class Splatter
    {
        private Rectangle position;
        private Texture2D splatterTexture;

        private float maxTime;
        private float timeElapsed;
        private bool isVisible;

        public Splatter(ContentManager content, Rectangle position)
        {
            maxTime = 20f;
            this.position = position;
            isVisible = true;

            splatterTexture = content.Load<Texture2D>(@"Images/Game/splat");
        }

        public bool IsVisible
        {
            get { return isVisible; }
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed >= maxTime)
            {
                isVisible = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(splatterTexture, position, Color.White);
        }
    }
}
