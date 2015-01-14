using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Defender.View;

namespace Defender.Controller
{
    class PausMenu : StateHandler
    {
        private const int nPausButtons = 2,
            continueGameIndex = 0,
            mainMenuIndex = 1;
        private int menuButtonHeight = 1,
            menuButtonWidth = 2;

        private Texture2D[] pausButtonTexture = new Texture2D[nPausButtons];
        private Rectangle[] pausButtonRectangle = new Rectangle[nPausButtons];
        private List<Button> pausButtons = new List<Button>();

        private Camera camera;
        private GraphicsDevice gd;
        private SpriteFont font;

        public PausMenu(ContentManager content, EventHandler stateEvent, Camera camera, GraphicsDevice gd)
            : base(stateEvent)
        {
            this.camera = camera;
            this.gd = gd;
            font = content.Load<SpriteFont>(@"Font/SpriteFont1");

            pausButtonTexture[continueGameIndex] = content.Load<Texture2D>(@"Images/Menu/continuebutton");
            pausButtonTexture[mainMenuIndex] = content.Load<Texture2D>(@"Images/Menu/mainmenubutton");

            MenuButtonsPrep();

            for (int i = 0; i < nPausButtons; i++)
            {
                pausButtons.Add(new Button(pausButtonTexture[i], pausButtonRectangle[i]));
            }
        }

        public void MenuButtonsPrep()
        {
            float x = 0.5f * camera.PosScaleX - menuButtonWidth * camera.SizeScale / 2;
            float y = 0.45f * camera.PosScaleY;
            for (int i = 0; i < nPausButtons; i++)
            {
                pausButtonRectangle[i] = new Rectangle((int)x, (int)y, menuButtonWidth * (int)camera.SizeScale, menuButtonHeight * (int)camera.SizeScale);
                y += menuButtonHeight * camera.SizeScale + 0.01f * camera.PosScaleY;
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < pausButtons.Count; i++)
            {
                if (pausButtons[i].ButtonUpdate())
                {
                    if (i == continueGameIndex)
                    {
                        stateEvent.Invoke(this, new StateChoice(0));
                    }
                    if (i == mainMenuIndex)
                    {
                        stateEvent.Invoke(this, new StateChoice(1));
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gd.Clear(Color.Black);
            spriteBatch.DrawString(font, "PAUSED", new Vector2((1f * camera.PosScaleX - font.MeasureString("PAUSED").X) / 2, 2f * camera.SizeScale), Color.Lime);

            foreach (Button button in pausButtons)
            {
                button.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}
