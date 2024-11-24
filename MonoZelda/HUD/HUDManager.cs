//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using MonoZelda.Link;
//using System.Diagnostics;
//using MonoZelda.Sprites;
//using MonoZelda.Link.Projectiles;


//namespace MonoZelda.HUD
//{
//    public class HUDManager
//    {
//        private List<SpriteDict> hearts;
//        private ProjectileManager projectileManager;
//        private SpriteDict proj;

//        // Dictionary to map ProjectileType to sprite names
//        Dictionary<WeaponType, string> weaponSpriteMap = new Dictionary<WeaponType, string>()
//        {
//            { WeaponType.Bow, "bow" },
//            { WeaponType.Boomerang, "boomerang" },
//            { WeaponType.CandleBlue, "candle_blue" },
//            { WeaponType.Bomb, "bomb" }
//        };

//        public HUDManager(ProjectileManager projectileManager)
//        {
//            this.hearts = new List<SpriteDict>();
//            this.projectileManager = projectileManager;
//        }

//        private void Initialize()
//        {
//            //sets hearts
//            int numberOfHearts = PlayerState.Health;
//            for (int i = 0; i < numberOfHearts; i++)
//            {
//                Point heartPosition = new Point(700 + (i * 32),125);
//                SpriteDict heart = new SpriteDict(SpriteType.Items, 0, heartPosition);
//                heart.SetSprite("heart_full");
//                hearts.Add(heart);
//            }
//            //Set rupees
//            int numberOfRupees = PlayerState.Rupees;
//            Point rupeePosition = new Point(300, 10);
//            SpriteDict rupee = new SpriteDict(SpriteType.Items, 0, rupeePosition);
//            rupee.SetSprite("rupee");
//            //set key
//            int numberOfKeys = PlayerState.Keys;
//            Point keyPosition = new Point(300, 80);
//            SpriteDict key = new SpriteDict(SpriteType.Items, 0, keyPosition);
//            key.SetSprite("key_0");
//            //set bomb
//            int numberOfBombs = PlayerState.Bombs;
//            Point bombPosition = new Point(300, 150);
//            SpriteDict bomb = new SpriteDict(SpriteType.Items, 0, bombPosition);
//            bomb.SetSprite("bomb");


//            //show equipped projecetilie
//            WeaponType weaponType = projectileManager.EquippedWeapon;
//            Point projPosition = new Point(450, 75);
//            this.proj = new SpriteDict(SpriteType.Items, 0, projPosition);
//            string spriteName = weaponSpriteMap.TryGetValue(weaponType, out var name) ? name : "arrow";
//            proj.SetSprite(spriteName);
//        }

//        public void UpdateHeartDisplay()
//        {
//            for (int i = hearts.Count - 1; i >= PlayerState.Health; i--)
//            {
//                hearts[i].SetSprite("heart_empty");
//            }

//        }

//        public void UpdateSelectedWeapon()
//        {
            
//            WeaponypeT weaponType = projectileManager.EquippedWeapon;
//            Debug.WriteLine(weaponType.ToString());
//            string spriteName = weaponSpriteMap.TryGetValue(weaponType, out var name) ? name : "arrow";
//            proj.SetSprite(spriteName);
//        }
//    }
//}
