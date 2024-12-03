using System;
using Microsoft.Xna.Framework;
using MonoZelda.Enemies.EnemyClasses;
using MonoZelda.Items;
using MonoZelda.Sprites;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using System.Diagnostics;
using MonoZelda.Sound;
namespace MonoZelda.Enemies
{
    public class EnemyStateMachine
    {

        //states
        public enum Direction { Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight, None }
        private Direction direction = Direction.None;
        public bool Spawning {get; set; }
        public bool Dead { get; set; }
        public bool TakingKnockback { get; set; }

        //not states
        private const float KNOCKBACK_FORCE = 1000f;
        private const float DAMAGE_FLASH_TIME = .5f;
        private float velocity = 120;
        private float dt;
        private double animationTimer;
        private float knockbackTimer;
        private Vector2 knockbackDirection;
        private SpriteDict enemySpriteDict;
        private string currentSprite;
        private Vector2 movement;
        private ItemFactory itemFactory;
        private SpriteDict keyDict;
        private bool hasItem;
        private bool itemSpawned = false;

        public EnemyStateMachine(SpriteDict spriteDict, ItemFactory itemFactory, bool hasItem)
        {
            enemySpriteDict = spriteDict;
            Spawning = true;
            Dead = false;
            animationTimer = 0;
            enemySpriteDict.SetSprite("cloud");
            this.itemFactory = itemFactory;
            this.hasItem = hasItem;
            keyDict = new SpriteDict(SpriteType.Items, 0, new Point(0, 0));
            keyDict.SetSprite("key_0");
            keyDict.Enabled = false;
        }
      
        public void ChangeDirection(Direction newDirection)
        {
            direction = newDirection;
        }

        public void SetSprite(string newSprite)
        {
            currentSprite = newSprite;
        }

        public void DamageFlash()
        {
            enemySpriteDict.SetFlashing(SpriteDict.FlashingType.Colorful, DAMAGE_FLASH_TIME);
        }

        public void ChangeSpeed(float newSpeed)
        {
            velocity = newSpeed;
        }

        public void Knockback(bool takesKnockback, Link.Direction collisionDirection)
        {
            if (takesKnockback)
            {
                TakingKnockback = true;
                knockbackDirection = collisionDirection switch
                {
                    Link.Direction.Up => new Vector2(0, 1),
                    Link.Direction.Down => new Vector2(0, -1),
                    Link.Direction.Left => new Vector2(1, 0),
                    Link.Direction.Right => new Vector2(-1, 0),
                    _ => Vector2.Zero
                };
            }
        }

        public void Die(Boolean respawning)
        {
            if (respawning)
            {
                enemySpriteDict.Enabled = false;
            }
            else
            {
                Dead = true;
                animationTimer = 0;
                enemySpriteDict.SetSprite("death");
            }
        }

        public void DropItem(Point pos, Enemy enemy)
        {
            Random r = new Random();
            var selection = r.Next(101);
            var item = ItemList.Triforce;
            if (hasItem)
            {
                item = ItemList.Key;
            }
            else if (selection <= 20)
            {
                item = ItemList.Rupee;
            }else if (selection <= 30)
            {
                item = ItemList.Heart;
            }else if (selection <= 40)
            {
                item = ItemList.Bomb;
            }else if (selection <= 50)
            {
                item = ItemList.Clock;
            }else if (enemy.GetType() == typeof(Aquamentus))
            {
                item = ItemList.Fairy;
            }

            if (item != ItemList.Triforce && !itemSpawned)
            {
                if (enemy.GetType() == typeof(Aquamentus))
                {
                    itemFactory.CreateItem(new ItemSpawn(pos,ItemList.HeartContainer), true);
                }
                itemFactory.CreateItem(new ItemSpawn(new Point(pos.X - 16, pos.Y - 32),item),true);
                itemSpawned = true;
            }
        }
        
        public Point Update(Enemy enemy, Point position)
        {
            dt = (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
            Vector2 enemyPosition = position.ToVector2();
            if (TakingKnockback)
            {
                knockbackTimer += dt;
                if (knockbackTimer >= 0.05)
                {
                    TakingKnockback = false;
                }
                enemyPosition += (KNOCKBACK_FORCE * knockbackDirection) * dt;
                enemySpriteDict.SetSprite("gel_turquoise");
            }
            else if (Spawning)
            {
                animationTimer += dt;
                if (animationTimer >= 1)
                {
                    enemy.ChangeDirection();
                    Spawning = false;
                }
            }
            else if (Dead)
            {
                animationTimer += dt;
                keyDict.Enabled = false;
                var xp = ItemList.XPOrb;
                if (animationTimer >= 0.5)
                {
                    enemySpriteDict.Enabled = false;
                    enemy.Alive = false;
                    if (enemy.GetType() != typeof(Keese) && enemy.GetType() != typeof(Gel))
                    {
                        DropItem(position, enemy);
                    }
                    itemFactory.CreateItem(new ItemSpawn(new Point(position.X, position.Y), xp), true);

                }
            }
            else
            {
                knockbackTimer = 0;
                movement = direction switch
                {
                    Direction.Up => new Vector2(0, -1),
                    Direction.Down => new Vector2(0, 1),
                    Direction.Left => new Vector2(-1, 0),
                    Direction.Right => new Vector2(1, 0),
                    Direction.UpLeft => new Vector2(-1, -1),
                    Direction.UpRight => new Vector2(1, -1),
                    Direction.DownLeft => new Vector2(-1, 1),
                    Direction.DownRight => new Vector2(1, 1),
                    _ => Vector2.Zero
                };
                enemyPosition += (velocity * movement)*dt;
                if (direction != Direction.None)
                {
                    enemySpriteDict.Enabled = true;
                }
                enemySpriteDict.SetSprite(currentSprite);
            }

            position = enemyPosition.ToPoint();
            enemySpriteDict.Position = position;
            if (hasItem && enemy.GetType() == typeof(Stalfos) && enemy.Alive)
            {
                keyDict.Enabled = true;
                keyDict.Position = new Point(position.X - 16, position.Y - 32);
            }
            return position;
        }
    }
}
