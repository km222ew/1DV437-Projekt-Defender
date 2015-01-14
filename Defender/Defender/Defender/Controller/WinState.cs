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
    class WinState : StateHandler
    {
        private Button mainMenuButton;
        private Texture2D mainMenuTexture;
        private Rectangle mainMenuRectangle;

        private int menuButtonHeight = 1,
            menuButtonWidth = 2;

        private Camera camera;
        private WinView view;
        private GraphicsDevice gd;

        public WinState(ContentManager content, EventHandler stateEvent, Camera camera, GraphicsDevice gd)
            : base(stateEvent)
        {
            this.camera = camera;
            this.gd = gd;
            mainMenuTexture = content.Load<Texture2D>(@"Images/Menu/mainmenubutton");

            float x = 0.5f * camera.PosScaleX - menuButtonWidth * camera.SizeScale / 2;
            float y = 0.45f * camera.PosScaleY;

            mainMenuRectangle = new Rectangle((int)x, (int)y, menuButtonWidth * (int)camera.SizeScale, menuButtonHeight * (int)camera.SizeScale);

            view = new WinView(content, camera);

            mainMenuButton = new Button(mainMenuTexture, mainMenuRectangle);

        }

        public override void Update(GameTime gameTime)
        {
            if (mainMenuButton.ButtonUpdate())
            {
                stateEvent.Invoke(this, new EventArgs());
            }
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gd.Clear(Color.Black);

            mainMenuButton.Draw(spriteBatch);
            view.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
