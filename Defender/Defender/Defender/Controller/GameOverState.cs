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
    class GameOverState : StateHandler
    {
        private const int nGameOverButtons = 2,
            restartIndex = 0,
            mainMenuIndex = 1;
        private int menuButtonHeight = 1,
            menuButtonWidth = 2;
        
        private Texture2D[] gameOverButtonTexture = new Texture2D[nGameOverButtons];
        private Rectangle[] gameOverButtonRectangle = new Rectangle[nGameOverButtons];
        private List<Button> gameOverButtons = new List<Button>();

        private Camera camera;
        private GraphicsDevice gd;
        private SpriteFont font;

        public GameOverState(ContentManager content, EventHandler stateEvent, Camera camera, GraphicsDevice gd)
            : base(stateEvent)
        {
            this.camera = camera;
            this.gd = gd;
            font = content.Load<SpriteFont>(@"Font/SpriteFont1");

            gameOverButtonTexture[restartIndex] = content.Load<Texture2D>(@"Images/Menu/restartbutton");
            gameOverButtonTexture[mainMenuIndex] = content.Load<Texture2D>(@"Images/Menu/mainmenubutton");

            MenuButtonsPrep();

            for (int i = 0; i < nGameOverButtons; i++)
            {
                gameOverButtons.Add(new Button(gameOverButtonTexture[i], gameOverButtonRectangle[i]));
            }
        }

        public void MenuButtonsPrep()
        {
            float x = 0.5f * camera.PosScaleX - menuButtonWidth * camera.SizeScale / 2;
            float y = 0.45f * camera.PosScaleY;
            for (int i = 0; i < nGameOverButtons; i++)
            {
                gameOverButtonRectangle[i] = new Rectangle((int)x, (int)y, menuButtonWidth * (int)camera.SizeScale, menuButtonHeight * (int)camera.SizeScale);
                y += menuButtonHeight * camera.SizeScale + 0.01f * camera.PosScaleY;
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < gameOverButtons.Count; i++)
            {
                if (gameOverButtons[i].ButtonUpdate())
                {
                    if (i == restartIndex)
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
            spriteBatch.DrawString(font, "GAME OVER", new Vector2((1f * camera.PosScaleX - font.MeasureString("GAME OVER").X) / 2, 2f * camera.SizeScale), Color.Lime);

            foreach (Button button in gameOverButtons)
            {
                button.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}
