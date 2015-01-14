using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Defender.View
{
    class WinView
    {
        private SpriteFont font;
        private Camera camera;

        public WinView(ContentManager content, Camera camera)
        {
            font = content.Load<SpriteFont>(@"Font/SpriteFont1");
            this.camera = camera;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "CONGRATULATIONS", new Vector2((1f * camera.PosScaleX - font.MeasureString("CONGRATULATIONS").X) / 2, 2f * camera.SizeScale), Color.Lime);
            spriteBatch.DrawString(font, "You finished all 3 levels", new Vector2((1f * camera.PosScaleX - font.MeasureString("You finished all 3 levels").X) / 2, 3f * camera.SizeScale), Color.Lime);
        }
    }
}
