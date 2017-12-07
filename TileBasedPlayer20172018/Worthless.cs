//using AnimatedSprite;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Tiling;

//namespace Tiler
//{
//    public class Projectile
//    {
//        protected Game myGame;

//        SpriteBatch spriteBatch;

        
//        Vector2 projectilePosition;
//        Vector2 direction;

//        int speed = 5;

//        // Testing for the projectile sprite.
//        AnimatedSprite projectileSprite;


//        // Projectile constructor
//        public Projectile(Game game, AnimatedSprite projectileSpriteIn, Vector2 projectilePositionIn, Vector2 directionIn)
//        {
//            //myGame = game;
//            //PixelPosition = userPosition * frameWidth;

//            myGame = game;

//            spriteBatch = new SpriteBatch(game.GraphicsDevice);

//            this.projectileSprite = projectileSpriteIn;
//            this.projectilePosition = projectilePositionIn;
//            this.direction = directionIn;         
//        }

//        //public override void Update(GameTime gameTime)
//        //{
//        //    //Follow(player);

//        //    base.Update(gameTime);

//        //}

//        public void Move()
//        {
//            projectilePosition += direction* new Vector2(1, 1) * speed;
//        }




//        public void Draw(GameTime gameTime)
//        {
//            //spriteBatch.Begin();

//            //spriteBatch.Draw(projectileSprite, projectilePosition, Color.White);

//            //spriteBatch.End();

//            //base.Draw(gameTime);
//        }

//    }

//}


