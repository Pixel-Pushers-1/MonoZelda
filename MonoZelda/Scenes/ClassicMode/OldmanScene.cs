using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Scenes;
using Microsoft.Xna.Framework.Input;
using MonoZelda;

public class OldmanScene : RoomScene
{
    // variables and constants for text revealing
    private const string FIRST_ROW = "Eastmost Penninsula";
    private const string SECOND_ROW = "Is The Secret.";
    private static readonly Point FIRST_ROW_POINT = new Point(256, 352);
    private static readonly Point SECOND_ROW_POINT = new Point(320, 384);
    private int firstRowRevealedChars = 0;
    private int secondRowRevealedChars = 0;
    private float elapsedTime = 0f;
    private const float REVEAL_SPEED = 0.1f;
    private const float INVENTORY_TOGGLE_TIME = 0.5f;
    private float inventoryToggleTimer;
    private bool pauseState;

    private readonly CommandManager commandManager;
    private readonly CollisionController collisionController;
    private SpriteFont spriteFont;

    public OldmanScene(GraphicsDevice graphicsDevice, CommandManager commandManager, CollisionController collisionController, IDungeonRoom room)
        : base(graphicsDevice, commandManager, collisionController, room)
    {
        this.commandManager = commandManager;
        this.collisionController = collisionController;
    }

    public override void LoadContent(ContentManager contentManager)
    {
        base.LoadContent(contentManager);
        spriteFont ??= contentManager.Load<SpriteFont>("Fonts/Basic");
    }

    public override void Draw(SpriteBatch batch)
    {
        inventoryToggleTimer -= (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;

        if (pauseState == false && inventoryToggleTimer < 0)
        {
            batch.DrawString(spriteFont, FIRST_ROW.Substring(0, firstRowRevealedChars), FIRST_ROW_POINT.ToVector2(), Color.White);
            batch.DrawString(spriteFont, SECOND_ROW.Substring(0, secondRowRevealedChars), SECOND_ROW_POINT.ToVector2(), Color.White);
        }
    }

    public override void SetPaused(bool paused)
    {
        base.SetPaused(paused);

        inventoryToggleTimer = INVENTORY_TOGGLE_TIME;
        pauseState = paused;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (elapsedTime >= REVEAL_SPEED)
        {
            if (firstRowRevealedChars < FIRST_ROW.Length)
            {
                firstRowRevealedChars++;
            }
            else if (secondRowRevealedChars < SECOND_ROW.Length)
            {
                secondRowRevealedChars++;
            }
            elapsedTime = 0;
        }
    }
}
