using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Defender.Model
{
    class Player
    {
        Vector2 topLeftCornerPos;        
        private float playerVelocity;
        private float maxPlayerVelocity;
        private int health;

        public Player(Vector2 startPosition)
        {
            topLeftCornerPos = startPosition;
            playerVelocity = 0;
            maxPlayerVelocity = 5f;
            health = 100;
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public void GetHit(int damage)
        {
            health -= damage;
        }

        public bool IsDead()
        {
            if (health <= 0)
            {
                return true;
            }

            return false;
        }

        public void Regenerate()
        {
            if (health < 100)
            {
                health += 1;
            }
        }

        public Vector2 TopLeftCornerPos
        {
            get { return topLeftCornerPos; }
        }

        public void MovePlayerRight()
        {
            playerVelocity = maxPlayerVelocity;
        }

        public void MovePlayerLeft()
        {
            playerVelocity = -maxPlayerVelocity;
        }

        public void StopMoving()
        {
            playerVelocity = 0;
        }

        public void Update(float gameTime)
        {
            topLeftCornerPos.X = topLeftCornerPos.X + gameTime * playerVelocity;

            if (topLeftCornerPos.X > Level.SIZE_X - 2)
            {
                topLeftCornerPos.X = Level.SIZE_X - 2;
            }

            if (topLeftCornerPos.X < 1)
            {
                topLeftCornerPos.X = 1;
            }
        }
    }
}
