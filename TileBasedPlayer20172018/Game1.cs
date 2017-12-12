using CameraNS;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Tiler;
using Tiling;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Linq;
using AnimatedSprite;

namespace Tiler
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // 2D Games Programming Assessment December 2017
        // By Jack Gilmartin & Donnacha Fallon.
        // 10th December 2017.

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Declare textures for victory screen and game over screen.
        Texture2D victory;
        Texture2D gameOver;

        bool gameOverState = false;

        #region Declare Sound Effects and Song
        // start up music, for test.
        private SoundEffect playerFire;
        private SoundEffect sentryExplosion;

        // Background Music.
        Song backgroundMusic;
        #endregion

        // Set variables for the remaining time.
        SpriteFont timerFont;
        int remainingTime = 0;

        // Test TimeSpan.
        TimeSpan timeSpan = TimeSpan.FromSeconds(10);

        // Set the width and height of the tiles.
        int tileWidth = 64;
        int tileHeight = 64;

        // Declare TilePlayer.
        TilePlayer player1;

        // Create Tile List.
        List<TileRef> TileRefs = new List<TileRef>();

        // Create Collider List.
        List<Collider> colliders = new List<Collider>();

        // Create Collider list for the player's projectile.
        List<Collider> projectileColliders = new List<Collider>();

        // Create a sentry list.
        List<Sentry> sentries = new List<Sentry>();

        // Testing removing sentries.
        List<Sentry> killList = new List<Sentry>();

        int sentryCount = 0;
        int killCount = 0;

        // Test death explosions.
        AnimateSheetSprite deathExplosion;

        TimeSpan explosionDuration = TimeSpan.FromSeconds(1);

        // Create a lock list.
        List<Lock> locks = new List<Lock>();


        string[] backTileNames = { "crates", "pavement", "red water", "sentry", "home", "exit", "skull", "locked" };

        public enum TileType { CRATES, PAVEMENT, REDWATER, SENTRY, HOME, EXIT, SKULL, LOCKED };
        int[,] tileMap = new int[,]
    {
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,1,1,1,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,1,0,1,0,1,0,1,0,1,0,1,2,2,2,2,2},
        {2,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,0,1,0,1,0,1,0,1,0,1,0,1,0,1,2,2,2,2,2},
        {2,1,1,2,2,2,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,2,2,2,2,2,2,2,2,2,2,2,0,1,0,1,0,1,0,1,0,1,0,1,0,1,2,2,2,2,2},
        {2,2,2,2,2,2,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,2,2,6,6,6,6,6,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2},
        {2,2,2,2,2,2,0,0,0,0,0,1,1,1,0,0,0,1,1,1,0,0,6,6,6,6,6,6,6,6,6,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,3,2,2,2,2},
        {2,2,2,2,2,2,0,0,0,0,0,1,1,0,0,0,3,0,1,1,6,6,6,6,6,6,3,6,6,6,6,6,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1},
        {2,2,2,2,2,2,0,0,0,0,0,1,1,0,0,0,0,0,1,1,6,6,6,6,6,6,6,6,6,6,6,6,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1},
        {2,1,1,1,2,2,0,3,0,2,1,1,1,2,2,2,2,2,3,2,6,6,2,2,2,6,6,2,2,2,6,6,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,2,2,2,2},
        {2,1,4,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,6,6,2,2,6,6,6,6,2,2,6,6,2,2,2,2,2,2,2,3,2,2,3,2,2,2,1,1,2,2,2,2},
        {2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,6,6,6,6,6,6,6,6,6,6,6,6,2,2,2,2,3,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2},
        {2,1,1,1,2,2,0,3,0,2,2,2,2,2,2,2,2,2,2,2,6,6,6,6,6,0,0,6,6,6,6,6,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,6,6,6,6,6,6,6,6,6,6,6,6,1,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,6,6,6,6,6,6,6,6,6,6,6,6,1,1,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2},
        {5,7,7,7,1,1,1,1,3,1,1,3,1,1,3,1,1,1,1,1,6,2,6,6,2,6,6,2,6,6,2,6,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2},
        {7,7,7,7,1,1,1,1,3,1,1,3,1,1,3,1,1,1,1,1,3,2,6,6,2,3,6,2,6,3,2,6,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,3,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
    };
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            new Camera(this, Vector2.Zero,
                new Vector2(tileMap.GetLength(1) * tileWidth, tileMap.GetLength(0) * tileHeight));

            new InputEngine(this);

            #region Set Colliders
            SetColliders(TileType.CRATES);
            SetColliders(TileType.REDWATER);
            SetColliders(TileType.EXIT);
            #endregion


            SpawnPlayer(TileType.HOME);
            SpawnSentries(TileType.SENTRY);

            SpawnLocks(TileType.LOCKED);

            player1.loadProjectile(new SuperProjectile(this, player1.PixelPosition, new List<TileRef>()
            {
                new TileRef(4, 0, 0)
            }, 64, 64, 0f));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);

            // Load in victory and game over textures.
            victory = Content.Load<Texture2D>(@"Winter Game Sprites/Victory");

            gameOver = Content.Load<Texture2D>(@"Winter Game Sprites/Game Over Edited");

            // Create font for the timer.
            timerFont = Content.Load<SpriteFont>("timerFont");
            

            // Load in the tile sheet.
            Texture2D tx = Content.Load<Texture2D>(@"Tiles/tank tiles 64 x 64");
            Services.AddService(tx);


            // Load Sound effects
            playerFire = Content.Load<SoundEffect>(@"Winter Game Sound Effects Wave/PlayerFire");
            sentryExplosion = Content.Load<SoundEffect>(@"Winter Game Sound Effects Wave/SentryExplosion");


            // Tile References to be drawn on the Map corresponding to the entries in the defined 
            // Tile Map

            // "crates", "pavement", "red water", "sentry", "home", "exit", "skull", locked".
            TileRefs.Add(new TileRef(11, 1, 0));
            TileRefs.Add(new TileRef(3, 3, 1));
            TileRefs.Add(new TileRef(0, 9, 0));
            TileRefs.Add(new TileRef(6, 2, 3));
            TileRefs.Add(new TileRef(0, 2, 4));
            TileRefs.Add(new TileRef(1, 2, 0));
            TileRefs.Add(new TileRef(6, 11, 0));
            TileRefs.Add(new TileRef(5, 3, 0));
            // Names for the Tiles

            new SimpleTileLayer(this, backTileNames, tileMap, TileRefs, tileWidth, tileHeight);
            List<Tile> found = SimpleTileLayer.GetNamedTiles("sentry");

            #region Load and Play BGM.
            // Load Background Music.
            this.backgroundMusic = Content.Load<Song>(@"Winter Game Music/Metal Fox");

            // Set volume.
            MediaPlayer.Volume = 0.0f;
            MediaPlayer.IsRepeating = true;
            #endregion

            // TODO: use this.Content to load your game content here
        }

        public void SetColliders(TileType t)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    if (tileMap[y, x] == 2)
                    {
                        projectileColliders.Add(new Collider(this,
                            Content.Load<Texture2D>(@"Tiles/collider"),
                            x, y
                            ));
                    }

                    else if (tileMap[y, x] == (int)t)
                    {
                        colliders.Add(new Collider(this,
                            Content.Load<Texture2D>(@"Tiles/collider"),
                            x, y
                            ));
                    }
                }
        }


        public void SpawnPlayer(TileType t)
        {
            // Declare bool to keep track of whether or not the home tile has been found on the map.
            bool homeTileFound = false;

            // Declare float values to convert the x and y value of the home tile to floats. This will allow the TilePlayer constructor to take a copy of the home tile's x and y value and spawn the player on that tile.
            float xFloatVer = 0;
            float yFloatVer = 0;

            for (int x = 0; x < tileMap.GetLength(1); x++)
            {
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    // If the current position on the map matches 4, the value for the home tile enumeration...
                    if (tileMap[y, x] == (int)t)
                    {
                        // ...Take a copy of that position and convert the values to float.                   
                        xFloatVer = (float)x;
                        yFloatVer = (float)y;


                        // Spawn the player, the vector2 constructor takes floats only, this is why the previous step is necessary.
                        //The Vector2 constructor sets the position of the player to that of the home tile.
                        Services.AddService(player1 = new TilePlayer(this, new Vector2(xFloatVer, yFloatVer), new List<TileRef>()
            {
                new TileRef(15, 2, 0),
                new TileRef(15, 3, 0),
                new TileRef(15, 4, 0),
                new TileRef(15, 5, 0),
                new TileRef(15, 6, 0),
                new TileRef(15, 7, 0),
                new TileRef(15, 8, 0),
            }, 64, 64, 0f));

                        homeTileFound = true;

                        // Exit the inner for loop when the home tile has been found, there's no need to search any more of the map.
                        break;
                    }

                }

                // If the home tile has already been found...
                if (homeTileFound == true)
                {
                    break;
                    //..Exit the outer for loop.
                }
            }
        }


        public void SpawnSentries(TileType t)
        {
            // Declare float values to convert the x and y value of the home tile to floats. This will allow the TilePlayer constructor to take a copy of the home tile's x and y value and spawn the player on that tile.
            float xFloatVer = 0;
            float yFloatVer = 0;

            for (int x = 0; x < tileMap.GetLength(1); x++)
            {
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    // If the current position on the map matches 4, the value for the home tile enumeration...
                    if (tileMap[y, x] == (int)t)
                    {
                        // ...Take a copy of that position and convert the values to float.                   
                        xFloatVer = (float)x;
                        yFloatVer = (float)y;

                        // Spawn the player, the vector2 constructor takes floats only, this is why the previous step is necessary.
                        //The Vector2 constructor sets the position of the player to that of the home tile.
                        sentries.Add(new Sentry(this, new Vector2(xFloatVer, yFloatVer), new List<TileRef>()
            {
                new TileRef(21, 2, 0),
                new TileRef(21, 3, 0),
                new TileRef(21, 4, 0),
                new TileRef(21, 5, 0),
                new TileRef(21, 6, 0),
                new TileRef(21, 7, 0),
                new TileRef(21, 8, 0),
            }, 64, 64, 0f));

                        sentryCount++;                      
                    }
                }
            }

            foreach (Sentry item in sentries)
            {
                item.loadProjectile(new SuperProjectile(this, item.PixelPosition, new List<TileRef>()
            {
                new TileRef(4, 0, 0)
            }, 64, 64, 0f));
            }

        }

        public void SpawnLocks(TileType t)
        {
            // Declare float values to convert the x and y value of the home tile to floats. This will allow the TilePlayer constructor to take a copy of the home tile's x and y value and spawn the player on that tile.
            float xFloatVer = 0;
            float yFloatVer = 0;

            for (int x = 0; x < tileMap.GetLength(1); x++)
            {
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    // If the current position on the map matches 4, the value for the home tile enumeration...
                    if (tileMap[y, x] == (int)t)
                    {
                        // ...Take a copy of that position and convert the values to float.                   
                        xFloatVer = (float)x;
                        yFloatVer = (float)y;

                        // Spawn the player, the vector2 constructor takes floats only, this is why the previous step is necessary.
                        //The Vector2 constructor sets the position of the player to that of the home tile.
                        locks.Add(new Lock(this, new Vector2(xFloatVer, yFloatVer), new List<TileRef>()
            {
                new TileRef(11, 2, 0),

            }, 64, 64, 0f));

                    }

                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && player1.MySuperProjectile.ProjectileState == SuperProjectile.PROJECTILE_STATE.STILL)
            {
                playerFire.Play();           
            }


            #region Handle behaviour between player and sentry.
            foreach (var item in sentries)
            {
                item.Follow(player1);

                if (item.Visible == true)
                {
                    player1.CollisionSentry(item);
                }

                if (item.Health <= 0)
                {                
                    if (item.Visible == true)
                    {
                        killCount++;

                        deathExplosion = new AnimateSheetSprite(this, item.PixelPosition, new List<TileRef>()
            {
                new TileRef(0, 0, 0),
                new TileRef(1, 0, 0),
                new TileRef(2, 0, 0),
            }, 64, 64, 0f);
                    }

                    item.Visible = false;
                }
            }
            #endregion
      
            foreach (Lock collisionLock in locks)
            {
                player1.CollisionLock(collisionLock);
            }

            // What to do when all sentries have been destroyed.
            if (killCount == sentryCount)
            {
                MediaPlayer.Volume -= 0.4f;

                foreach (Lock item in locks)
                {
                    item.Visible = false;
                }

                gameOverState = true;
            }

            foreach (var item in projectileColliders)
            {
                player1.MySuperProjectile.WallCollision(item);
            }

            #region Play background music when test timer is up.
            // Play background music.
            //timeSpan -= gameTime.ElapsedGameTime;

            // timeSpan < TimeSpan.Zero
            // Add the above line to the if statement below to test the countdown.

            if(MediaPlayer.Volume != 0.5f)
            {
                MediaPlayer.Play(backgroundMusic);
                MediaPlayer.Volume += 0.5f;
            }
            #endregion

            base.Update(gameTime);
            // TODO: Add your update logic here           

            /// <summary>
            /// This is called when the game should draw itself.
            /// </summary>
            /// <param name="gameTime">Provides a snapshot of timing values.</param>
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Maroon);

            spriteBatch.Begin();

            //DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);

            // Draw remaining time.
            spriteBatch.DrawString(timerFont, "Remaining Time: ", new Vector2(player1.PixelPosition.X+10, player1.PixelPosition.X + 10), Color.White);

            //DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color);

            if (gameOverState == false)
            {
                spriteBatch.Draw(gameOver, GraphicsDevice.Viewport.Bounds, Color.White);
            }


            // TODO: Add your drawing code here

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
