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


namespace MonoZelda.HUD
{
    public class HUDManager
    {
        private PlayerState _playerState;
        private List<SpriteDict> hearts;
        private ContentManager contentManager;
        public HUDManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            this.hearts = new List<SpriteDict>();
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
            Point rupeePosition = new Point(300, 20);
            SpriteDict rupee = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, rupeePosition);
            rupee.SetSprite("rupee");
            //set key
            int numberOfKeys = PlayerState.Keys;
            Point keyPosition = new Point(300, 100);
            SpriteDict key = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, keyPosition);
            key.SetSprite("key_0");
            //set bomb
            int numberOfBombs = PlayerState.Bombs;
            Point bombPosition = new Point(300, 125);
            SpriteDict bomb = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, bombPosition);
            bomb.SetSprite("bomb");


        }
        public void UpdateHeartDisplay()
        {
            Debug.WriteLine("UPDATE");
            for (int i = hearts.Count - 1; i >= PlayerState.Health; i--)
            {
                hearts[i].SetSprite("heart_empty");
            }

        }
    }
}
