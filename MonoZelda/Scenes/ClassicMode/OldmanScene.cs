using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Scenes;

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
    private const float REVEAL_SPEED = 0.1f; // Adjust for desired speed (in seconds per character)

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
        // Draw revealed portion of the text
        batch.DrawString(spriteFont, FIRST_ROW.Substring(0, firstRowRevealedChars), FIRST_ROW_POINT.ToVector2(), Color.White);
        batch.DrawString(spriteFont, SECOND_ROW.Substring(0, secondRowRevealedChars), SECOND_ROW_POINT.ToVector2(), Color.White);
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
