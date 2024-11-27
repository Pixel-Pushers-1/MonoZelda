using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.Sprites;
using MonoZelda.Dungeons;
using Microsoft.VisualBasic;

namespace MonoZelda.Scenes;

public class WallMasterGrabScene : Scene
{
    //variables
    private IDungeonRoom startRoom;
    private SpriteDict FakeLink;
    private SpriteDict FakeWallMaster;
    private SpriteDict FakeBackground;
    private BlankSprite LeftCurtain;
    private BlankSprite RightCurtain;
    private GraphicsDevice graphicsDevice;
    
    public WallMasterGrabScene(IDungeonRoom startRoom, GraphicsDevice graphicsDevice)
    {
        this.startRoom = startRoom;
        this.graphicsDevice = graphicsDevice;
    }

    public override void LoadContent(ContentManager contentManager)
    {
        // make curtains
        var curtainSize = new Point(512, 704);
        var Center = new Point(graphicsDevice.Viewport.Width / 2, 192);
        LeftCurtain = new BlankSprite(SpriteLayer.Triforce - 1, Center, curtainSize, Color.Black);
        RightCurtain = new BlankSprite(SpriteLayer.Triforce - 1, Center, curtainSize, Color.Black);

        // create fake link and wallmaster
        FakeLink = new SpriteDict(SpriteType.Player, SpriteLayer.HUD, PlayerState.Position);
        FakeWallMaster = new SpriteDict(SpriteType.Enemies, SpriteLayer.HUD, PlayerState.Position);
    }

    private void CreateFakeDoors(IDungeonRoom room)
    {
        foreach (var door in room.GetDoors())
        {
            var doorSprite = new SpriteDict(SpriteType.Blocks, SpriteLayer.Triforce - 2, door.Position);
            doorSprite.SetSprite(door.Type.ToString());
        }
    }

    public override void Update(GameTime gameTime)
    {
        
    }
}
