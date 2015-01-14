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
    class TransitionState : StateHandler
    {
        private const int nTransitionButtons = 2,
            nextLevelIndex = 0,
            mainMenuIndex = 1;
        private int menuButtonHeight = 1,
            menuButtonWidth = 2;

        private Texture2D[] transitionButtonTexture = new Texture2D[nTransitionButtons];
        private Rectangle[] transitionButtonRectangle = new Rectangle[nTransitionButtons];
        private List<Button> transitionButtons = new List<Button>();

        private Camera camera;
        private GraphicsDevice gd;
        private SpriteFont font;

        public TransitionState(ContentManager content, EventHandler stateEvent, Camera camera, GraphicsDevice gd)
            : base(stateEvent)
        {
            this.camera = camera;
            this.gd = gd;
            font = content.Load<SpriteFont>(@"Font/SpriteFont1");

            transitionButtonTexture[nextLevelIndex] = content.Load<Texture2D>(@"Images/Menu/nextlevelbutton");
            transitionButtonTexture[mainMenuIndex] = content.Load<Texture2D>(@"Images/Menu/mainmenubutton");

            MenuButtonsPrep();

            for (int i = 0; i < nTransitionButtons; i++)
            {
                transitionButtons.Add(new Button(transitionButtonTexture[i], transitionButtonRectangle[i]));
            }
        }

        public void MenuButtonsPrep()
        {
            float x = 0.5f * camera.PosScaleX - menuButtonWidth * camera.SizeScale / 2;
            float y = 0.45f * camera.PosScaleY;
            for (int i = 0; i < nTransitionButtons; i++)
            {
                transitionButtonRectangle[i] = new Rectangle((int)x, (int)y, menuButtonWidth * (int)camera.SizeScale, menuButtonHeight * (int)camera.SizeScale);
                y += menuButtonHeight * camera.SizeScale + 0.01f * camera.PosScaleY;
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < transitionButtons.Count; i++)
            {
                if (transitionButtons[i].ButtonUpdate())
                {
                    if (i == nextLevelIndex)
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
            spriteBatch.DrawString(font, "LEVEL COMPLETE", new Vector2((1f * camera.PosScaleX - font.MeasureString("LEVEL COMPLETE").X) / 2, 2f * camera.SizeScale), Color.Lime);

            foreach (Button button in transitionButtons)
            {
                button.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}
