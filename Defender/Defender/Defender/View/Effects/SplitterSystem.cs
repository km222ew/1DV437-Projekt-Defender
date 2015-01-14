using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Defender.View.Effects
{
    class SplitterSystem
    {
        private Vector2 position;
        private Texture2D splitterTexture;
        private List<SplitterParticle> particles;
        private int NUM_PARTICLES = 50;
        private float totalTime = 0;
        private static float MAX_TIME = 0.5f;
        private static float maxSpeed = 8f;
        private static float minSpeed = 4f;
        private Camera camera;

        public SplitterSystem(Vector2 position, ContentManager content, Camera camera)
        {
            this.camera = camera;
            this.position = position;
            splitterTexture = content.Load<Texture2D>(@"Images/Game/bullet2");
            particles = new List<SplitterParticle>();

            spawnNewSystem();
        }

        private void spawnNewSystem()
        {
            Random rand = new Random();

            for (int i = 0; i < NUM_PARTICLES; i++)
            {
                Vector2 velocity = new Vector2(((float)rand.NextDouble() - 0.5f), ((float)rand.NextDouble() - 0.5f));
                //velocity.Normalize();
                velocity = velocity * ((float)rand.NextDouble() * maxSpeed - minSpeed);

                particles.Add(new SplitterParticle(velocity, position));
            }
        }

        public void Update(float timeElapsed)
        {
            totalTime += timeElapsed;
            if (totalTime < MAX_TIME)
            {
                for (int i = 0; i < NUM_PARTICLES; i++)
                {
                    particles[i].Update(timeElapsed);
                }
            }
            else
            {
                particles = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (particles != null)
            {
                for (int i = 0; i < NUM_PARTICLES; i++)
                {
                    particles[i].Draw(spriteBatch, splitterTexture, camera);
                }
            }

            
        }
    }
}
