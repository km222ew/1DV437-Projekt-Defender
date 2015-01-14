using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Defender.Model
{
    abstract class Enemy
    {
        private FloatRectangle position;
        private float speed;
        private int health;
        private int damage;
        private float attackSpeed;
        private float attackSpeedTimeElapsed;

        public Enemy(FloatRectangle position, float speed)
        {
            this.position = position;
            this.speed = speed;
        }

        public virtual float AttackSpeedTimeElapsed
        {
            get { return attackSpeedTimeElapsed; }
            set { attackSpeedTimeElapsed = value; }
        }

        public virtual int Health
        {
            get { return health; }
        }

        public virtual void Hit()
        {
            health -= 1;
        }

        public virtual bool HasStopped()
        {
            if (speed == 0)
            {
                return true;
            }

            return false;
        }

        public virtual void Stop()
        {
            speed = 0;
        }

        public virtual float AttackSpeed
        {
            get { return attackSpeed; }
        }

        public virtual int Attack()
        {
            return damage;
        }

        public virtual FloatRectangle Position
        {
            get { return position; }
        }

        public virtual void Update(float gameTime)
        {
            position.updateY(speed, gameTime);
        }
    }
}
