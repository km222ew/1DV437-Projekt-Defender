using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Defender.View;

namespace Defender.Controller
{
    class HowToPlay : StateHandler
    {
        private int menuButtonHeight = 1,
            menuButtonWidth = 2;

        private Button backButton;
        private Texture2D backButtonTexture;
        private Rectangle backButtonRectangle;

        private HowToPlayView view;
        private GraphicsDevice gd;

        public HowToPlay(ContentManager content, EventHandler screenEvent, Game1 game1, Camera camera, GraphicsDevice gd)
            : base(screenEvent)
        {
            backButtonTexture = content.Load<Texture2D>("Images/Menu/backbutton");

            float x = 0.5f * camera.PosScaleX - menuButtonWidth * camera.SizeScale / 2;
            float y = 0.78f * camera.PosScaleY;
            backButtonRectangle = new Rectangle((int)x, (int)y, menuButtonWidth * (int)camera.SizeScale, menuButtonHeight * (int)camera.SizeScale);

            backButton = new Button(backButtonTexture, backButtonRectangle);

            view = new HowToPlayView(content, camera);
            this.gd = gd;
        }

        public override void Update(GameTime gameTime)
        {
            if (backButton.ButtonUpdate())
            {
                stateEvent.Invoke(this, new EventArgs());
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gd.Clear(Color.Black);

            backButton.Draw(spriteBatch);
            view.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
