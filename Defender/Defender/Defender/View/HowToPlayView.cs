using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Defender.View
{
    class HowToPlayView
    {

        private SpriteFont font;
        private Camera camera;

        public HowToPlayView(ContentManager content, Camera camera)
        {
            font = content.Load<SpriteFont>(@"Font/SpriteFont1");
            this.camera = camera;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Move with ARROW KEYS", new Vector2((1f * camera.PosScaleX - font.MeasureString("Move with arrow keys").X) / 2, 1f * camera.SizeScale), Color.Lime);
            spriteBatch.DrawString(font, "Shoot pistol with SPACEBAR", new Vector2((1f * camera.PosScaleX - font.MeasureString("Shoot pistol with SPACEBAR").X) / 2, 2f * camera.SizeScale), Color.Lime);
            spriteBatch.DrawString(font, "Fire shotgun with D (level 2 and 3)", new Vector2((1f * camera.PosScaleX - font.MeasureString("Fire shotgun with B (level 2 and 3)").X) / 2, 3f * camera.SizeScale), Color.Lime);
            spriteBatch.DrawString(font, "Paus with ESC in game", new Vector2((1f * camera.PosScaleX - font.MeasureString("Paus with ESC in game").X) / 2, 4f * camera.SizeScale), Color.Lime);
            spriteBatch.DrawString(font, "Navigate menus with MOUSE", new Vector2((1f * camera.PosScaleX - font.MeasureString("Navigate menus with MOUSE").X) / 2, 5f * camera.SizeScale), Color.Lime);

        }
    }
}
