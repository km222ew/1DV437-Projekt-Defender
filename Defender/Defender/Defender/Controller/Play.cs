using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Defender.Model;
using Defender.View;

namespace Defender.Controller
{
    class Play : StateHandler
    {
        private int currentLevel;
        private int totalLevels;

        private bool gameIsActive;
        private bool gameIsOver;
        private bool levelIsComplete;

        private GameModel model;
        private Game game;
        private GameView gameView;
        private GraphicsDevice gd;

        private GameTime gameTime;

        public Play(ContentManager content, EventHandler stateEvent, Game1 game, Camera camera, int currentLevel, int totalLevels, GraphicsDevice gd)
            : base(stateEvent)
        {
            this.currentLevel = currentLevel;
            this.totalLevels = totalLevels;
            this.gd = gd;
            model = new GameModel();
            gameView = new GameView(content, camera, model);

            model.SetListener(gameView);
            
            this.game = game;

            NewGame();
        }

        public void NewGame()
        {
            if (!model.IsLevelPresent(currentLevel))
            {
                game.Exit();
            }

            gameIsActive = false;
            gameIsOver = false;
            levelIsComplete = false;
        }

        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            gameView.UpdateKeyboard();

            if (model.IsLevelComplete())
            {
                levelIsComplete = true;
            }

            if (model.IsPlayerDead())
            {
                gameIsOver = true;
            }

            if (gameIsActive && !gameIsOver && !levelIsComplete)
            {
                
                if (gameView.PlayerMovesRight())
                {
                    model.MovePlayerRight();
                }
                else if (gameView.PlayerMovesLeft())
                {
                    model.MovePlayerLeft();
                }
                else
                {
                    model.StopMovingPlayer();
                }

                if (gameView.PlayerShootsPistol())
                {
                    model.ShootPistol();
                }

                if (currentLevel >= 2)
                {
                    if (gameView.PlayerShootsShotgun())
                    {
                        model.ShootShotgun();
                    }
                }   

                if (gameView.IsEscapePressed())
                {
                    //Audio.SoundBank.PlayCue("blip");

                    stateEvent.Invoke(this, new StateChoice(0));
                }

                

                model.Update(gameTime);
            }
            else if (!gameIsActive || gameIsOver || levelIsComplete)
            {
                if (!gameIsActive)
                {
                    if (gameView.IsEnterPressed())
                    {
                        gameIsActive = true;
                    }
                }

                if (levelIsComplete)
                {
                    if (currentLevel == 3)
                    {
                        stateEvent.Invoke(this, new StateChoice(3));
                    }
                    else
                    {
                        stateEvent.Invoke(this, new StateChoice(1));
                    }
                    
                }

                if (gameIsOver)
                {
                    stateEvent.Invoke(this, new StateChoice(2));
                }
            }

            gameView.UpdatePrevKeyboard();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gd.Clear(Color.Black);

            gameView.Draw(spriteBatch, gameTime);

            if (!gameIsActive)
            {
                gameView.DrawIdle(spriteBatch);
            }

            
        }
    }
}
