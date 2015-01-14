using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defender.Controller
{
    class StateHandler
    {
        protected EventHandler stateEvent;
        public StateHandler(EventHandler stateEvent)
        {
            this.stateEvent = stateEvent;
        }

        public virtual void Update(GameTime gameTime)
        { 
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        { 
            
        }
    }
}
