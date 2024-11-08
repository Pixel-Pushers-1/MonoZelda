using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZelda.Scenes;

public interface IScene
{
    void Update(GameTime gameTime);
    void LoadContent(ContentManager contentManager);
    public void Draw(SpriteBatch batch);
}
