using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Defender.Model.Enemies;


namespace Defender.Model
{
    class GameModel
    {
        IModelListener listener;

        Level level;
        Player player;
        List<PistolBullet> bullets;
        List<ShotgunPellet> pellets;

        private int currentLevel;

        //enemy fields
        private const int LEVEL_1_ENEMIES = 50;
        private const int LEVEL_2_ENEMIES = 100;
        private const int LEVEL_3_ENEMIES = 150;
        private int currentEnemySpawn;

        private List<Enemy> enemies;

        private Random random;
        private float enemySpawnTimeElapsed;
        private float enemySpawnMaxTime;
        private bool spawnEnemy = true;

        //shotgun timer
        private float shotgunTimeElapsed;
        private float shotgunMaxTime = 2f;
        private bool canShootShotgun = true;

        //pistol timer
        private float pistolTimeElapsed;
        private float pistolMaxTime = 0.25f;
        private bool canShootPistol = true;

        //regen timer
        private float lifeRegenTimeElapsed;
        private float lifeRegenMaxTime = 0.5f;

        public GameModel()
        {
            level = new Level();
            player = new Player(level.PlayerStartPosition());
            bullets = new List<PistolBullet>();
            pellets = new List<ShotgunPellet>();
            random = new Random();
            enemies = new List<Enemy>();

            NewEnemySpawnTime();
        }

        public void SetListener(IModelListener listener)
        {
            this.listener = listener;
        }

        public void Update(GameTime gameTime)
        {
            float timeStep = 0.001f;
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (seconds > 0)
            {
                int numIterations = (int)(timeStep / seconds);

                for (int i = 0; i < numIterations; i++)
                {
                    UpdateModel(timeStep);
                }

                float timeLeft = seconds - timeStep * numIterations;
                UpdateModel(timeLeft);
            }           
        }

        private void UpdateModel(float gameTime)
        {
            enemySpawnTimeElapsed += gameTime;
            if (enemySpawnTimeElapsed >= enemySpawnMaxTime)
            {
                spawnEnemy = true;
                enemySpawnTimeElapsed = 0;
                NewEnemySpawnTime();
            }

            lifeRegenTimeElapsed += gameTime;
            if (lifeRegenTimeElapsed >= lifeRegenMaxTime)
            {
                player.Regenerate();
                lifeRegenTimeElapsed = 0;
            }

            shotgunTimeElapsed += gameTime;
            if (shotgunTimeElapsed >= shotgunMaxTime)
            {
                canShootShotgun = true;
                shotgunTimeElapsed = 0;
            }

            pistolTimeElapsed += gameTime;
            if (pistolTimeElapsed >= pistolMaxTime)
            {
                canShootPistol = true;
                pistolTimeElapsed = 0;
            }

            if (spawnEnemy == true)
            {
                spawnEnemy = false;
                SpawnEnemy();
            }

            player.Update(gameTime);

            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
                if (enemy.HasStopped())
                {
                    enemy.AttackSpeedTimeElapsed += gameTime;
                    if (enemy.AttackSpeedTimeElapsed >= enemy.AttackSpeed)
                    {
                        player.GetHit(enemy.Attack());
                        enemy.AttackSpeedTimeElapsed = 0;
                        listener.PlayerHit();
                    }
                }
                else if (enemy.Position.Bottom >= 7)
                {
                    enemy.Stop();
                }
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gameTime);

                for (int j = 0; j < enemies.Count; j++)
                {
                    if (bullets[i].EnemyCollision(enemies[j].Position))
                    {
                        enemies[j].Hit();
                        listener.EnemyHit();

                        if (EnemyIsDead(enemies[j]))
                        {
                            listener.EnemyDeath(enemies[j].Position);
                            enemies.RemoveAt(j);
                        }
                        break;
                    }
                }

                if (bullets[i].Hit == true || bullets[i].Position.Top < 0)
                {
                    bullets.RemoveAt(i);
                }
            }

            for (int i = 0; i < pellets.Count; i++)
            {
                pellets[i].Update(gameTime);

                for (int j = 0; j < enemies.Count; j++)
                {
                    if (pellets[i].EnemyCollision(enemies[j].Position))
                    {
                        enemies[j].Hit();
                        listener.EnemyHit();

                        if (EnemyIsDead(enemies[j]))
                        {
                            listener.EnemyDeath(enemies[j].Position);
                            enemies.RemoveAt(j);
                        }
                        break;
                    }                    
                }

                if (pellets[i].TotalDamage == 0 || pellets[i].Position.Top < 4)
                {
                    pellets.RemoveAt(i);
                }
            }           
        }

        private bool EnemyIsDead(Enemy enemy)
        {
            if (enemy.Health <= 0)
            {
                return true;
            }
            return false;
        }

        private void NewEnemySpawnTime()
        {
            float max = 1f;
            float min = 0.4f;

            enemySpawnMaxTime = (float)random.NextDouble() * (max - min) + min; 
        }

        private void createWorm(float speed, float max, float min)
        {
            Vector2 topLeft = new Vector2((float)(random.NextDouble() * (max - min) + min), -1);
            Vector2 bottomRight = new Vector2(topLeft.X + 0.65f, topLeft.Y + 0.75f);

            FloatRectangle position = new FloatRectangle(topLeft, bottomRight);

            enemies.Add(new Worm(position, speed));
        }

        private void createSpine(float speed, float max, float min)
        {
            Vector2 topLeft = new Vector2((float)(random.NextDouble() * (max - min) + min), -1);
            Vector2 bottomRight = new Vector2(topLeft.X + 0.9f, topLeft.Y + 1.1f);

            FloatRectangle position = new FloatRectangle(topLeft, bottomRight);

            enemies.Add(new Spine(position, speed));
        }

        private void createArmor(float speed, float max, float min)
        {
            Vector2 topLeft = new Vector2((float)(random.NextDouble() * (max - min) + min), -1);
            Vector2 bottomRight = new Vector2(topLeft.X + 0.9f, topLeft.Y + 1.1f);

            FloatRectangle position = new FloatRectangle(topLeft, bottomRight);

            enemies.Add(new Armor(position, speed));
        }

        private void SpawnEnemy()
        {
            float speed = (float)(random.NextDouble() * (5f - 1.5f) + 1.5f);
            float max = 5f;
            float min = 1f;

            if (currentLevel == 1 )
            {
                if (currentEnemySpawn >= LEVEL_1_ENEMIES)
                {
                    return;                    
                }
                else
                {
                    max = 4;
                    min = 2;
                    createWorm(speed, max, min);
                }               
            }
            else if (currentLevel == 2)
            {
                if (currentEnemySpawn >= LEVEL_2_ENEMIES)
                {
                    return;
                }
                else if (random.NextDouble() > 0.6f)
                {
                    createSpine(speed, max, min);
                }
                else
                {
                    createWorm(speed, max, min);
                }
            }
            else if (currentLevel == 3)
            {
                double randEnemy = random.NextDouble();

                if (currentEnemySpawn >= LEVEL_3_ENEMIES)
                {
                    return;
                }
                else if (randEnemy > 0.7)
                {
                    createSpine(speed, max, min);
                }
                else if (randEnemy >0.3)
                {
                    createWorm(speed, max, min);
                }
                else
                {
                    createArmor(speed, max, min);
                }
            }
            currentEnemySpawn += 1;
        }

        public Vector2 GetPlayerPosition()
        {
            return player.TopLeftCornerPos;
        }

        public int GetPlayerHealth()
        {
            return player.Health;
        }

        public void ShootShotgun()
        {
            if (canShootShotgun == true)
            {
                Vector2 bulletDisplacement = player.TopLeftCornerPos + new Vector2(0.15f, -0.5f);
                pellets.Add(new ShotgunPellet(new FloatRectangle(bulletDisplacement, bulletDisplacement + new Vector2(1f, 0.6f))));
                canShootShotgun = false;
                listener.ShotgunShot(bulletDisplacement);
            }           
        }

        public void ShootPistol()
        {
            if (canShootPistol == true)
            {
                Vector2 bulletDisplacement = player.TopLeftCornerPos + new Vector2(0.55f, 0f);
                bullets.Add(new PistolBullet(new FloatRectangle(bulletDisplacement, bulletDisplacement + new Vector2(0.2f, 0.2f))));
                canShootPistol = false;
                listener.PistolShot();
            }            
        }

        public void MovePlayerRight()
        {
            player.MovePlayerRight();
        }

        public void MovePlayerLeft()
        {
            player.MovePlayerLeft();
        }

        public void StopMovingPlayer()
        {
            player.StopMoving();
        }

        public bool IsLevelPresent(int currentLevel)
        {
            if (level.LoadLevel(currentLevel))
            {
                this.currentLevel = currentLevel;
                currentEnemySpawn = 0;
                return true;
            }
            return false;
        }

        public bool IsLevelComplete()
        {
            if (currentLevel == 1)
            {
                if (currentEnemySpawn >= LEVEL_1_ENEMIES && enemies.Count <= 0)
                {
                    return true;
                }
            }
            else if (currentLevel == 2)
            {
                if (currentEnemySpawn >= LEVEL_2_ENEMIES && enemies.Count <= 0)
                {
                    return true;
                }
            }
            else if (currentLevel == 3)
            {
                if (currentEnemySpawn >= LEVEL_3_ENEMIES && enemies.Count <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPlayerDead()
        {
            if (player.IsDead())
            {
                listener.PlayerDeath();
                return true;
            }
            return false;
        }

        public Tile[,] GetLevel()
        {
            return level.Tiles;
        }
        public List<Enemy> GetEnemies()
        {
            return enemies;
        }

        public List<PistolBullet> GetBullets()
        {
            return bullets;        
        }

        public List<ShotgunPellet> GetPellets()
        {
            return pellets;
        }

        public int CurrentLevel
        {
            get { return currentLevel; }
        }
    }
}
