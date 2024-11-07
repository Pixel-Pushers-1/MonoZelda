using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Scenes
{
    public abstract class Scene : IScene
    {
        public virtual void Draw(SpriteBatch batch)
        {
        }

        public virtual void LoadContent(ContentManager contentManager)
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
