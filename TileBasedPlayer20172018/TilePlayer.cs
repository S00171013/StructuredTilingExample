using AnimatedSprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiling;
using Microsoft.Xna.Framework.Audio;

namespace Tiler
{
    public class TilePlayer : RotatingSprite
    {
        int speed = 5;
        float turnspeed = 0.03f;
        public Vector2 previousPosition;

        protected Game myGame;
       
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

            if (MySuperProjectile.BoundingRectangle.Intersects(s.BoundingRectangle) && MySuperProjectile.Visible == true)
            {
                MySuperProjectile.ProjectileState = SuperProjectile.PROJECTILE_STATE.EXPLODING;
                s.Health -= 50;

                //if (s.Health <= 0)
                //{
                //    s.Visible = false;
                //}
            }
        }

        public void CollisionLock(Lock l)
        {
            if (BoundingRectangle.Intersects(l.BoundingRectangle) && l.Visible == true)
            {
                PixelPosition = previousPosition;
            }

            if (l.open == true)
            {
                l.Visible = false;
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
                // fire the rocket in the direction the player is pointing.
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

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (MySuperProjectile != null && MySuperProjectile.ProjectileState != SuperProjectile.PROJECTILE_STATE.STILL)
            {
                MySuperProjectile.Draw(gameTime);
            }
        }
    }
}
