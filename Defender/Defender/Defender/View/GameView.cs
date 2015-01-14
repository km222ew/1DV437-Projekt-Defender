using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Defender.Model;
using Defender.Model.Enemies;
using Defender.View;
using Defender.View.Effects;

namespace Defender.View
{
    class GameView : IModelListener
    {
        private Camera camera;
        private ContentManager content;
        private GameModel model;
        private Tile[,] tiles;
        private List<Splatter> splatters;
        private SplitterSystem splitterSystem;

        //textures
        private Texture2D inactiveBackground;
        private Texture2D darkBackground;
        private Texture2D tileTexture;
        private Texture2D playerTexture;
        private Texture2D bulletTexture;
        private Texture2D pelletTexture;
        private Texture2D wormTexture;
        private Texture2D spineTexture;
        private Texture2D armorTexture;

        private Color attackedColor;

        //Spritesheet locations
        private Rectangle wallTexture;
        private Rectangle floorTexture;
        private Rectangle drainTexture;
        private Rectangle dangerTexture;
        private Rectangle boxTexture;
        private Rectangle defaultTexture;

        private KeyboardState prevKeyboardState;
        private KeyboardState currKeyboardState;

        private SpriteFont font;

        public GameView(ContentManager content, Camera camera, GameModel model)
        {
            this.camera = camera;
            this.content = content;
            this.model = model;
            tiles = model.GetLevel();
            splatters = new List<Splatter>();

            inactiveBackground = content.Load<Texture2D>(@"Images/Background/pausbackground");
            darkBackground = content.Load<Texture2D>(@"Images/Background/dark");
            tileTexture = content.Load<Texture2D>(@"Images/Game/spritesheet");
            playerTexture = content.Load<Texture2D>(@"Images/Game/soldier");
            bulletTexture = content.Load<Texture2D>(@"Images/Game/Bullet2");
            pelletTexture = content.Load<Texture2D>(@"Images/Game/shotgun");
            wormTexture = content.Load<Texture2D>(@"Images/Game/worm");
            spineTexture = content.Load<Texture2D>(@"Images/Game/spine");
            armorTexture = content.Load<Texture2D>(@"Images/Game/armor");

            attackedColor = Color.White;

            splitterSystem = null;

            //Spritesheet locations
            wallTexture = new Rectangle(32, 64, 32, 32);
            floorTexture = new Rectangle(224, 96, 32, 32);
            drainTexture = new Rectangle(32, 96, 32, 32);
            dangerTexture = new Rectangle(384, 64, 32, 32);
            boxTexture = new Rectangle(256, 96, 32, 32);

            defaultTexture = new Rectangle(64, 64, 32, 32);

            font = content.Load<SpriteFont>(@"Font/SpriteFont1");
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawLevel(spriteBatch);
            DrawPlayer(spriteBatch);
            DrawHud(spriteBatch);
            DrawSplatter(spriteBatch, gameTime);
            DrawEnemies(spriteBatch, gameTime);
            DrawBullets(spriteBatch);
            //DrawPellets(spriteBatch);
            DrawParticles(spriteBatch, gameTime);
        }

        public void DrawParticles(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (splitterSystem != null)
            {
                splitterSystem.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (splitterSystem != null)
            {
                splitterSystem.Draw(spriteBatch);
            }
        }

        public void DrawSplatter(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < splatters.Count; i++)
            {
                splatters[i].Update(gameTime);
                splatters[i].Draw(spriteBatch);

                if (!splatters[i].IsVisible)
                {
                    splatters.RemoveAt(i);
                }
            }
        }

        public void DrawIdle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(inactiveBackground, new Rectangle(0, 0, (int)(Level.SIZE_X * camera.SizeScale), (int)(Level.SIZE_Y * camera.SizeScale)), Color.White);

            if (model.CurrentLevel == 2)
            {
                spriteBatch.DrawString(font, "SHOTGUN UNLOCKED", new Vector2((1f * camera.PosScaleX - font.MeasureString("SHOTGUN UNLOCKED").X) / 2, 1f * camera.SizeScale), Color.Lime);
                spriteBatch.DrawString(font, "Press D to use it every 2 seconds", new Vector2((1f * camera.PosScaleX - font.MeasureString("Press D to use it every 2 seconds").X) / 2, 2f * camera.SizeScale), Color.Lime);
            }

            spriteBatch.DrawString(font, "Press Enter to start", new Vector2((1f * camera.PosScaleX -font.MeasureString("Press Enter to start").X) / 2, 0.5f * camera.PosScaleY), Color.Lime);
        }

        private void DrawPlayer(SpriteBatch spriteBatch)
        {
            Vector2 playerPosition = model.GetPlayerPosition();

            spriteBatch.Draw(playerTexture, new Rectangle((int)(playerPosition.X * camera.SizeScale), (int)(playerPosition.Y * camera.SizeScale), (int)camera.SizeScale, (int)camera.SizeScale), attackedColor);

            attackedColor = Color.White;
        }

        private void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(darkBackground, new Rectangle(0, (int)(9f * camera.SizeScale), (int)(3f * camera.SizeScale), (int)(1f * camera.SizeScale)), Color.Black);
            spriteBatch.DrawString(font, "Level: " + model.CurrentLevel.ToString(), new Vector2(0, 9f * camera.SizeScale), Color.Lime);
            spriteBatch.DrawString(font, "Health: " + model.GetPlayerHealth().ToString(), new Vector2(0, 9.5f * camera.SizeScale), Color.Lime);
        }

        private void DrawLevel(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Level.SIZE_X; x++)
            {
                for (int y = 0; y < Level.SIZE_Y; y++)
                {
                    Vector2 tilePos = new Vector2(x * camera.SizeScale, y * camera.SizeScale);
                    Rectangle usedTexture = defaultTexture;

                    Tile tile = tiles[x, y];

                    if (tile.Type == Tile.TileType.BOX)
                    {
                        usedTexture = boxTexture;
                    }
                    else if (tile.Type == Tile.TileType.DANGER)
                    {
                        usedTexture = dangerTexture;
                    }
                    else if (tile.Type == Tile.TileType.DRAIN)
                    {
                        usedTexture = drainTexture;       
                    }
                    else if (tile.Type == Tile.TileType.FLOOR)
                    {
                        usedTexture = floorTexture;
                    }
                    else if (tile.Type == Tile.TileType.WALL)
                    {
                        usedTexture = wallTexture;
                    }

                    spriteBatch.Draw(tileTexture, new Rectangle((int)tilePos.X, (int)tilePos.Y, (int)camera.SizeScale, (int)camera.SizeScale), usedTexture, Color.White);

                }
            }
        }

        public void DrawEnemies(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var enemy in model.GetEnemies())
            {
                Rectangle viewPos = new Rectangle((int)(enemy.Position.Left * camera.SizeScale),
                        (int)(enemy.Position.Top * camera.SizeScale),
                        (int)((enemy.Position.Right - enemy.Position.Left) * camera.SizeScale),
                        (int)((enemy.Position.Bottom - enemy.Position.Top) * camera.SizeScale));

                if (enemy is Worm)
                {
                    spriteBatch.Draw(wormTexture, viewPos, Color.White);
                }
                else if (enemy is Spine)
                {
                    spriteBatch.Draw(spineTexture, viewPos, Color.White);
                }
                else if (enemy is Armor)
                {
                    spriteBatch.Draw(armorTexture, viewPos, Color.White);
                }
            }
        }

        public void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (var bullet in model.GetBullets())
            {
                Rectangle viewPos = new Rectangle((int)(bullet.Position.Left * camera.SizeScale),
                    (int)(bullet.Position.Top * camera.SizeScale),
                    (int)((bullet.Position.Right - bullet.Position.Left) * camera.SizeScale),
                    (int)((bullet.Position.Bottom - bullet.Position.Top) * camera.SizeScale));

                spriteBatch.Draw(bulletTexture, viewPos, Color.White);
            }
        }

        //Looked better with just the particles. This projectile still counts for collision though.

        //public void DrawPellets(SpriteBatch spriteBatch)
        //{
        //    foreach (var pellet in model.GetPellets())
        //    {
        //        Rectangle viewPos = new Rectangle((int)(pellet.Position.Left * camera.SizeScale),
        //            (int)(pellet.Position.Top * camera.SizeScale),
        //            (int)((pellet.Position.Right - pellet.Position.Left) * camera.SizeScale),
        //            (int)((pellet.Position.Bottom - pellet.Position.Top) * camera.SizeScale));

        //        spriteBatch.Draw(pelletTexture, viewPos, Color.White);
        //    }
        //}

        public void UpdateKeyboard()
        {
            currKeyboardState = Keyboard.GetState();
        }

        public void UpdatePrevKeyboard()
        {
            prevKeyboardState = currKeyboardState;
        }

        public bool KeyboardInput(Keys key)
        {
            if (key == Keys.Escape || key == Keys.Space || key == Keys.B)
            { 
                if (prevKeyboardState.IsKeyUp(key) && currKeyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }
            else if (currKeyboardState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }

        public bool PlayerShootsPistol()
        {
            if (KeyboardInput(Keys.Space))
            {
                return true;
            }
            return false;
        }

        public bool PlayerShootsShotgun()
        {
            if (KeyboardInput(Keys.D))
            {
                return true;
            }
            return false;
        }

        public bool IsEscapePressed()
        {
            if (KeyboardInput(Keys.Escape))
            {
                return true;
            }

            return false;
        }

        public bool IsEnterPressed()
        {
            if (KeyboardInput(Keys.Enter))
            {
                return true;
            }

            return false;
        }

        public bool PlayerMovesLeft()
        {
            if (KeyboardInput(Keys.Left))
            {
                return true;
            }

            return false;
        }

        public bool PlayerMovesRight()
        {
            if (KeyboardInput(Keys.Right))
            {
                return true;
            }

            return false;
        }

        public void PistolShot()
        {
            Game1.pistolShot.Play();
        }
        public void ShotgunShot(Vector2 playerPos)
        {
            Game1.shotgunShot.Play();
            Game1.shotgunReload.Play();

            splitterSystem = new SplitterSystem(playerPos, content, camera);
        }
        public void EnemyDeath(FloatRectangle position)
        {
            Rectangle visualPos = new Rectangle((int)(position.Left * camera.SizeScale),
                (int)(position.Top * camera.SizeScale),
                (int)((position.Right - position.Left) * camera.SizeScale),
                (int)((position.Bottom - position.Top) * camera.SizeScale));

            splatters.Add(new Splatter(content, visualPos));
        }
        public void EnemyHit()
        {
            Game1.monsterHit.Play();
        }
        public void PlayerHit()
        {
            Game1.playerHit.Play();
            attackedColor = Color.Red;
        }
        public void PlayerDeath()
        {
            Game1.playerDeath.Play();
        }
    }
}
