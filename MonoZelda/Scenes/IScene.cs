using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoZelda.Scenes;

public interface IScene
{
    void Update(GameTime gameTime);
    void LoadContent();
}
