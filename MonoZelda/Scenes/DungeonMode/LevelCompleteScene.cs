using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Sprites;
using MonoZelda.Commands;
using MonoZelda.Sound;

namespace MonoZelda.Scenes;

public class LevelCompleteScene : Scene
{
    private IDungeonRoom startRoom;
    private ICommand loadRoomCommand;
    private BlankSprite LeftCurtain;
    private BlankSprite RightCurtain;
    private BlankSprite WhiteCurtain;
    private SpriteDict FakeLink;
    private SpriteDict FakeTriforce;
    private SpriteDict FakeBackground;
    private const float LEVEL_COMPLETION_TIME = 25f;
    private float timer;

    private enum CurtainDirection
    {
        In,
        Out
    }

    public LevelCompleteScene(IDungeonRoom startRoom, ICommand loadRoomCommand)
    {
        this.startRoom = startRoom;
        this.loadRoomCommand = loadRoomCommand;
    }

    public override void LoadContent(ContentManager contentManager)
    {
        //make curtains
        var origin = new Point(0, 192);
        var leftPosition = new Point(-512, 192);
        var rightPosition = new Point(1024, 192);
        var curtainSize = new Point(512, 704);
        var whiteCurtainSize = new Point(1024, 704);
        LeftCurtain = new BlankSprite(SpriteLayer.Triforce - 1 ,leftPosition, curtainSize, Color.Black);
        RightCurtain = new BlankSprite(SpriteLayer.Triforce - 1 ,rightPosition, curtainSize, Color.Black);
        WhiteCurtain = new BlankSprite(SpriteLayer.Triforce - 1, DungeonConstants.DungeonPosition, whiteCurtainSize, Color.White);
        WhiteCurtain.Enabled = false;

        // create fake Link, Background, and Triforce
        FakeLink = new SpriteDict(SpriteType.Player, SpriteLayer.Triforce, PlayerState.Position);
        FakeLink.SetSprite("pickupitem_twohands");
        FakeTriforce = new SpriteDict(SpriteType.Items, SpriteLayer.Triforce, PlayerState.Position + new Point(-32, -84));
        FakeTriforce.SetSprite("triforce");
        FakeBackground = new SpriteDict(SpriteType.Blocks, SpriteLayer.Triforce - 2, DungeonConstants.BackgroundPosition);
        FakeBackground.SetSprite("room_11");

        // Play Victory sounds
        SoundManager.StopSound("LOZ_Dungeon_Theme");
        SoundManager.PlaySound("LOZ_Victory", false);
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
        timer += (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        if ((2 <= timer) && (timer <= 16))
        {
            if(LeftCurtain.Position.X != 0)
            {
                LeftCurtain.Position += new Point(4, 0);
                RightCurtain.Position += new Point(-4, 0);
            }
        }
        else if((16 <= timer) && (timer <= 18))
        {
            FakeTriforce.Enabled = false;
            FakeLink.SetSprite("cloud");
        }
        else if((18 <= timer) && (timer <= 19))
        {
            CreateFakeDoors(startRoom);
            FakeBackground.SetSprite("room_1");
            FakeLink.Enabled = false;
            if(LeftCurtain.Position.X != -512)
            {
                LeftCurtain.Position += new Point(-8, 0);
                RightCurtain.Position += new Point(8, 0);
            }
        }
        else if(timer > 19)
        {
            FakeBackground.Enabled = false;
            PlayerState.Position = new Point(500, 725);
            SoundManager.PlaySound("LOZ_Dungeon_Theme", false);
            loadRoomCommand.Execute(startRoom.RoomName);
        }
    }
}
