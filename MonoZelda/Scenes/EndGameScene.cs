using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZelda.Scenes;

public class EndGameScene : Scene
{
    private GraphicsDevice graphicsDevice;

    public EndGameScene(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;   
    }

    public override void Update(GameTime gameTime)
    {
        // To-do: animate waterfall
    }

    public override void LoadContent(ContentManager contentManager)
    {
        // Do Nothing
    }
}
