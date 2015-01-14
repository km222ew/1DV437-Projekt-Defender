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
    class MainMenu : StateHandler
    {
        private const int nMenuButtons = 3,
            startGameIndex = 0,
            howToPlayIndex = 1,
            quitGameIndex = 2;
        private int menuButtonHeight = 1,
            menuButtonWidth = 2;

        private Camera camera;
        private GraphicsDevice gd;
        private SpriteFont font;

        private List<Button> menuButtons = new List<Button>();

        private Texture2D[] menuButtonTexture = new Texture2D[nMenuButtons];
        private Rectangle[] menuButtonRectangle = new Rectangle[nMenuButtons];

        private Game1 game;

        public MainMenu(ContentManager content, EventHandler stateEvent, Game1 game, Camera camera, GraphicsDevice gd)
            : base(stateEvent)
        {
            this.camera = camera;
            this.game = game;
            this.gd = gd;
            font = content.Load<SpriteFont>(@"Font/SpriteFont1");

            menuButtonTexture[startGameIndex] = content.Load<Texture2D>(@"Images/Menu/startgamebutton");
            menuButtonTexture[howToPlayIndex] = content.Load<Texture2D>(@"Images/Menu/howtoplaybutton");
            menuButtonTexture[quitGameIndex] = content.Load<Texture2D>(@"Images/Menu/quitbutton");

            MenuButtonsPrep();

            for (int i = 0; i < nMenuButtons; i++)
            {
                menuButtons.Add(new Button(menuButtonTexture[i], menuButtonRectangle[i]));
            }
        }

        private void MenuButtonsPrep()
        {
            float x = 0.5f * camera.PosScaleX - menuButtonWidth * camera.SizeScale / 2;
            float y = 0.45f * camera.PosScaleY;
            for (int i = 0; i < nMenuButtons; i++)
            {
                menuButtonRectangle[i] = new Rectangle((int)x, (int)y, menuButtonWidth * (int)camera.SizeScale, menuButtonHeight * (int)camera.SizeScale);
                y += menuButtonHeight * camera.SizeScale + 0.01f * camera.PosScaleY;
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < menuButtons.Count; i++)
            {
                if (menuButtons[i].ButtonUpdate())
                {
                    if (i == startGameIndex)
                    {
                        stateEvent.Invoke(this, new StateChoice(0));
                    }
                    if (i == howToPlayIndex)
                    {
                        stateEvent.Invoke(this, new StateChoice(1));
                    }
                    if (i == quitGameIndex)
                    {
                        game.Exit();
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gd.Clear(Color.Black);
            spriteBatch.DrawString(font, "DEFENDER", new Vector2((1f * camera.PosScaleX - font.MeasureString("DEFENDER").X) / 2, 2f * camera.SizeScale), Color.Lime);

            foreach (Button button in menuButtons)
            {
                button.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}
