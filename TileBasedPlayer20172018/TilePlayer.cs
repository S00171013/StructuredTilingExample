using AnimatedSprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiling;

namespace Tiler
{
    public class TilePlayer : RotatingSprite
    {
        int speed = 5;
        float turnspeed = 0.03f;
        public Vector2 previousPosition;

        protected Game myGame;

        Texture2D projectileSprite;

        Projectile playerProjectile;

        private SuperProjectile mySuperProjectile;

        public SuperProjectile MySuperProjectile
        {
            get
            {
                return mySuperProjectile;
            }

            set
            {
                mySuperProjectile = value;
            }
        }
        

              

        public TilePlayer(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 1;
            PixelPosition = userPosition * FrameWidth;

            myGame = game;
                                  
            projectileSprite = myGame.Content.Load<Texture2D>(@"Winter Game Sprites/Projectile");   

        }



        public void Collision(Collider c)
        {
            if (BoundingRectangle.Intersects(c.CollisionField))
            {
                PixelPosition = previousPosition;
            }
        }

        public void CollisionSentry(Sentry s)
        {
            if (BoundingRectangle.Intersects(s.BoundingRectangle))
            {
                PixelPosition = previousPosition;
            }

            if (MySuperProjectile.BoundingRectangle.Intersects(s.BoundingRectangle))
            {
                MySuperProjectile.ProjectileState = SuperProjectile.PROJECTILE_STATE.EXPLODING;
            }
        }

        public void loadProjectile(SuperProjectile r)
        {
            MySuperProjectile = r;
        }


        public override void Update(GameTime gameTime)
        {
            previousPosition = PixelPosition;

            #region Handle Input
            // Create direction variable.
            Vector2 direction = new Vector2((float)Math.Cos(angleOfRotation), (float)Math.Sin(angleOfRotation));

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.PixelPosition += direction * new Vector2(1, 1) * speed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
               
                this.PixelPosition -= direction * new Vector2(1, 1) * speed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {        
                this.angleOfRotation -= turnspeed;
                //BoundingRectangle = angleOfRotation;
            }          

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {                
                this.angleOfRotation += turnspeed;
            }
            #endregion

            #region Handle Projectile
            if (MySuperProjectile != null && MySuperProjectile.ProjectileState == SuperProjectile.PROJECTILE_STATE.STILL)
            {
                MySuperProjectile.PixelPosition = this.PixelPosition;
            }

            if (MySuperProjectile != null)
            {
                // fire the rocket and it looks for the target
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && MySuperProjectile.ProjectileState == SuperProjectile.PROJECTILE_STATE.STILL)
                {
                    MySuperProjectile.Fire(direction);
                }
            }

            if (MySuperProjectile != null)
            {
                MySuperProjectile.Update(gameTime);
            }

            #endregion

            //if (Keyboard.GetState().IsKeyDown(Keys.Space))
            //{
            //    playerProjectile = new Projectile(myGame, new Vector2(PixelPosition.X, PixelPosition.Y), new List<TileRef>()
            //{
            //    new TileRef(0, 0, 0),
            //    new TileRef(1, 0, 0),
            //    new TileRef(2, 0, 0),              
            //}, 64, 64, 0f);

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (MySuperProjectile != null && MySuperProjectile.ProjectileState != SuperProjectile.PROJECTILE_STATE.STILL)
            {
                MySuperProjectile.Draw(gameTime);
            }

            //if (playerProjectile != null)
            //{
            //   playerProjectile.Draw(gameTime);
            //}       



        }
    }
}
