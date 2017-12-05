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

namespace Tiler
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D startUpScreen;
        Texture2D loadingScreen;

        // start up music, for test.
        SoundEffect startUpSound;
        private SoundEffect effect;


        SpriteFont timerFont;
        int remainingTime = 100;

        int tileWidth = 64;
        int tileHeight = 64;

        TilePlayer player1;

        Sentry sentry1;


        List<TileRef> TileRefs = new List<TileRef>();
        List<Collider> colliders = new List<Collider>();
        string[] backTileNames = { "blue box", "pavement", "blue steel", "green box", "home" };


        public enum TileType { BLUEBOX, PAVEMENT, BLUESTEEL, GREENBOX, HOME };
        int[,] tileMap = new int[,]
    {
        {1,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,2,3,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2},
        {2,1,1,2,2,2,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,2,0,0,0,0,0,2,0,0,0,0,2,0,0,0,2,0,0,0,2,2,2,2,3,2,2,2,2},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,3,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,1,1,2,1,1,4,1,1,1,1,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,3,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
    };
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
           
            

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

            //Services.AddService(new TilePlayer(this, new Vector2(64, 128), new List<TileRef>()
            //{
            //    new TileRef(15, 2, 0),
            //    new TileRef(15, 3, 0),
            //    new TileRef(15, 4, 0),
            //    new TileRef(15, 5, 0),
            //    new TileRef(15, 6, 0),
            //    new TileRef(15, 7, 0),
            //    new TileRef(15, 8, 0),
            //}, 64, 64, 0f));

            // Create a sentry for testing.
            Services.AddService(sentry1 = new Sentry(this, new Vector2(64, 128), new List<TileRef>()
            {
                new TileRef(20, 2, 0),
                new TileRef(20, 3, 0),
                new TileRef(20, 4, 0),
                new TileRef(20, 5, 0),
                new TileRef(20, 6, 0),
                new TileRef(20, 7, 0),
                new TileRef(20, 8, 0),
            }, 64, 64, 0f));


            // When I instantiate an object by adding it as a service (As I have done above), how can I refer specifically to that object when I want it to carry out its methods?
            // For example...

            // Usually it's...
            //Sentry sentry 1 = new Sentry(etcetera, etcetera);

            // ...And later this...
            //sentry1.Follow(TilePlayer player);

            // But since I have already instantiated my sentry by adding it as a service with no name given to it, I am unsure on how to make this sentry carry out its methods.

            // Is there a way to give a name to this sentry after it has been instantiated?





            SetColliders(TileType.BLUESTEEL);
            SetColliders(TileType.BLUEBOX);

            SpawnPlayer(TileType.HOME);

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
            

            //Initialize a 17 second timer for the loading screen.
            float timer = 17;        
           




            //Loads the startup screen.
            startUpScreen = Content.Load<Texture2D>(@"Winter Game Sprites/Insert Coin");



            //Trying to get the loading screen and the startup music to activate at the same time.
            //while under a 17 second timer (the length of the sound).
            if (timer <= 17 )
            {
                //Loads the loading screen and proceeds to play the startup music.
                loadingScreen = Content.Load<Texture2D>(@"Winter Game Sprites/Loading Screen");


                //Load the startup music.
                this.startUpSound = Content.Load<SoundEffect>(@"Winter Game Sound Effects Wave/PS1Startup");
            }


            //When the loading screen is finished, this should load the game.
            else if (timer >= 17)
            {
                Services.AddService(Content.Load<Texture2D>(@"Tiles/tank tiles 64 x 64"));
            }



           

           

            //Set volume.
            MediaPlayer.Volume = 0.0f;

             
            // Create font for the timer.
            timerFont = Content.Load<SpriteFont>("timerFont");


            // Tile References to be drawn on the Map corresponding to the entries in the defined 
            // Tile Map
            // "free", "pavement", "ground", "blue", "home" 
            TileRefs.Add(new TileRef(4, 2, 0));
            TileRefs.Add(new TileRef(3, 3, 1));
            TileRefs.Add(new TileRef(6, 3, 2));
            TileRefs.Add(new TileRef(6, 2, 3));
            TileRefs.Add(new TileRef(0, 2, 4));
            // Names for the Tiles

            new SimpleTileLayer(this, backTileNames, tileMap, TileRefs, tileWidth, tileHeight);
            List<Tile> found = SimpleTileLayer.GetNamedTiles("green box");
            // TODO: use this.Content to load your game content here
        }

        public void SetColliders(TileType t)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    if (tileMap[y, x] == (int)t)
                    {
                        colliders.Add(new Collider(this,
                            Content.Load<Texture2D>(@"Tiles/collider"),
                            x, y
                            ));
                    }
                }
        }

        // Experimenting with spawning the player on the home tile.
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
                        // The Vector2 constructor sets the position of the player to that of the home tile, or at least it should.
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

            else if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {

            }





            //double timer = gameTime.ElapsedGameTime.TotalSeconds;

            //if (gameTime.ElapsedGameTime.TotalSeconds > timer)
            //{
                //remainingTime -= 1;
            //}

            // TODO: Add your update logic here





            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // Draw remaining time.
            spriteBatch.DrawString(timerFont, "Remaining Time: " + remainingTime, new Vector2(20, 20), Color.White);

            // TODO: Add your drawing code here

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
