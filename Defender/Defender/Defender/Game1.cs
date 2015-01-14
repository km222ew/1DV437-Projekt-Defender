using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Defender.Controller;
using Defender.View;

namespace Defender
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Camera camera;

        private MainMenu mainMenu;
        private Play play;
        private HowToPlay howToPlay;
        private PausMenu pausMenu;
        private TransitionState transitionState;
        private GameOverState gameOverState;
        private WinState winState;

        private int currentLevel;
        private int totalLevels;

        private StateHandler currentState;
        private StateChoice sc;

        //audio
        public static SoundEffect pistolShot;
        public static SoundEffect shotgunShot;
        public static SoundEffect shotgunReload;
        public static SoundEffect monsterHit;
        public static SoundEffect monsterDeath1;
        public static SoundEffect monsterDeath2;
        public static SoundEffect monsterDeath3;
        public static SoundEffect playerDeath;
        public static SoundEffect playerHit;
        public static SoundEffect buttonClick;
        public static SoundEffect buttonHover;
        public static SoundEffect gameOver;
        public static SoundEffect nextLevel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 448;
            this.graphics.PreferredBackBufferHeight = 640;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            camera = new Camera(graphics.GraphicsDevice.Viewport);

            pistolShot = Content.Load<SoundEffect>(@"Audio/Handgun-MelonHea-7519_hifi");
            shotgunShot = Content.Load<SoundEffect>(@"Audio/Shotgun_-Flash_Ma-7446_hifi");
            shotgunReload = Content.Load<SoundEffect>(@"Audio/Shoutgun-Jhon-7486_hifi");
            playerDeath = Content.Load<SoundEffect>(@"Audio/Death-BKGr-7835_hifi");
            playerHit = Content.Load<SoundEffect>(@"Audio/Punch-joshman_-7996_hifi");
            monsterHit = Content.Load<SoundEffect>(@"Audio/Bir Poop Splat");
            monsterDeath1 = Content.Load<SoundEffect>(@"Audio/alien_sc-skullkee-8340_hifi");
            monsterDeath2 = Content.Load<SoundEffect>(@"Audio/Beast_sl-tim1050-8287_hifi");
            monsterDeath3 = Content.Load<SoundEffect>(@"Audio/monster_-Brian_Kl-8153_hifi");

            buttonHover = Content.Load<SoundEffect>(@"Audio/Dink-Public_D-146_hifi");
            buttonClick = Content.Load<SoundEffect>(@"Audio/Blip_1-Surround-7482_hifi");


            mainMenu = new MainMenu(this.Content, new EventHandler(MainMenuEvent), this, camera, GraphicsDevice);
            //play = new Play(this.Content, new EventHandler(PlayEvent), this, camera);
            howToPlay = new HowToPlay(this.Content, new EventHandler(HowToPlayEvent), this, camera, GraphicsDevice);
            pausMenu = new PausMenu(this.Content, new EventHandler(PausMenuEvent), camera, GraphicsDevice);
            transitionState = new TransitionState(this.Content, new EventHandler(TransitionStateEvent), camera, GraphicsDevice);
            gameOverState = new GameOverState(this.Content, new EventHandler(GameOverStateEvent), camera, GraphicsDevice);
            winState = new WinState(this.Content, new EventHandler(WinStateEvent), camera, GraphicsDevice);

            currentLevel = 1;
            totalLevels = 3;

            IsMouseVisible = true;

            currentState = mainMenu;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            currentState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            currentState.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void MainMenuEvent(object obj, EventArgs e)
        {
            sc = (StateChoice)e;
            switch (sc.choice)
            {
                case 0:
                    currentLevel = 1;
                    play = new Play(this.Content, new EventHandler(PlayEvent), this, camera, currentLevel, totalLevels, GraphicsDevice);
                    currentState = play;
                    break;
                case 1:
                    currentState = howToPlay;
                    break;
                case 2:
                    //currentState = options;
                    break;
                default:
                    break;
            }
        }

        public void HowToPlayEvent(object obj, EventArgs e)
        {
            currentState = mainMenu;
        }

        public void PlayEvent(object obj, EventArgs e)
        {
            currentState = pausMenu;

            sc = (StateChoice)e;
            switch (sc.choice)
            {
                //paus game
                case 0:
                    currentState = pausMenu;
                    break;
                //level completed
                case 1:
                    currentState = transitionState;
                    break;
                //level failed
                case 2:
                    currentState = gameOverState;
                    break;
                case 3:
                    currentState = winState;
                    break;
                default:
                    break;
            }
        }

        public void PausMenuEvent(object obj, EventArgs e)
        {
            sc = (StateChoice)e;
            switch (sc.choice)
            {
                //return to game
                case 0:
                    currentState = play;
                    break;
                //main menu
                case 1:                   
                    currentState = mainMenu;
                    break;
                default:
                    break;
            }
        }

        public void TransitionStateEvent(object obj, EventArgs e)
        {
            sc = (StateChoice)e;
            switch (sc.choice)
            {
                //next level
                case 0:
                    currentLevel += 1;
                    play = new Play(this.Content, new EventHandler(PlayEvent), this, camera, currentLevel, totalLevels, GraphicsDevice);
                    currentState = play;
                    break;
                //main menu
                case 1:
                    currentLevel = 1;
                    currentState = mainMenu;
                    break;
                default:
                    break;
            }
        }

        public void GameOverStateEvent(object obj, EventArgs e)
        {
            sc = (StateChoice)e;
            switch (sc.choice)
            {
                //restart level
                case 0:
                    play = new Play(this.Content, new EventHandler(PlayEvent), this, camera, currentLevel, totalLevels, GraphicsDevice);
                    currentState = play;
                    break;
                //main menu
                case 1:
                    currentState = mainMenu;
                    break;
                default:
                    break;
            }
        }

        public void WinStateEvent(object obj, EventArgs e)
        {
            currentState = mainMenu;
        }
    }
}
