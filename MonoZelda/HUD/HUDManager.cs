using MonoZelda.Items.ItemClasses;
using MonoZelda.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Link;
using System.Diagnostics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Link.Projectiles;


namespace MonoZelda.HUD
{
    public class HUDManager
    {
        private PlayerState _playerState;
        private List<SpriteDict> hearts;
        private ContentManager contentManager;
        private ProjectileManager projectileManager;
        private SpriteDict proj;

        // Dictionary to map ProjectileType to sprite names
        Dictionary<ProjectileType, string> projectileSpriteMap = new Dictionary<ProjectileType, string>
        {
            { ProjectileType.Arrow, "arrow" },
            { ProjectileType.ArrowBlue, "arrow_blue" },
            { ProjectileType.Boomerang, "boomerang" },
            { ProjectileType.BoomerangBlue, "boomerang_blue" },
            { ProjectileType.Bomb, "bomb" },
            { ProjectileType.CandleBlue, "fire" }
        };
        public HUDManager(ContentManager contentManager, ProjectileManager projectileManager)
        {
            this.contentManager = contentManager;
            this.hearts = new List<SpriteDict>();
            this.projectileManager = projectileManager;
        }
        public PlayerState PlayerState
        {
            get => _playerState;
            set
            {
                _playerState = value;
                Initialize();
            }
        }
        private void Initialize()
        {
            //sets hearts
            int numberOfHearts = PlayerState.Health;
            for (int i = 0; i < numberOfHearts; i++)
            {
                Point heartPosition = new Point(700 + (i * 32),125);
                SpriteDict heart = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, heartPosition);
                heart.SetSprite("heart_full");
                hearts.Add(heart);
            }
            //Set rupees
            int numberOfRupees = PlayerState.Rupees;
            Point rupeePosition = new Point(300, 10);
            SpriteDict rupee = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, rupeePosition);
            rupee.SetSprite("rupee");
            //set key
            int numberOfKeys = PlayerState.Keys;
            Point keyPosition = new Point(300, 80);
            SpriteDict key = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, keyPosition);
            key.SetSprite("key_0");
            //set bomb
            int numberOfBombs = PlayerState.Bombs;
            Point bombPosition = new Point(300, 150);
            SpriteDict bomb = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, bombPosition);
            bomb.SetSprite("bomb");


            //show equipped projecetilie
            ProjectileType projectileType = projectileManager.EquippedProjectile;
            Point projPosition = new Point(450, 75);
            this.proj = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, projPosition);
            string spriteName = projectileSpriteMap.TryGetValue(projectileType, out var name) ? name : "arrow";
            proj.SetSprite(spriteName);
        }
        public void UpdateHeartDisplay()
        {
            Debug.WriteLine("UPDATE");
            for (int i = hearts.Count - 1; i >= PlayerState.Health; i--)
            {
                hearts[i].SetSprite("heart_empty");
            }

        }
        public void UpdateSelectedWeapon()
        {

            ProjectileType projectileType = projectileManager.EquippedProjectile;
            Debug.WriteLine(projectileType.ToString());
            string spriteName = projectileSpriteMap.TryGetValue(projectileType, out var name) ? name : "arrow";
            proj.SetSprite(spriteName);
        }
    }
}
