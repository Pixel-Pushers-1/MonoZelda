using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.UI
{
    internal interface IScreenWidget
    {
        void Load(ContentManager content);
        void Update();
        void Draw(SpriteBatch sb);
    }
}
