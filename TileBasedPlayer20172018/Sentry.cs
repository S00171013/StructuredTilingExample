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
    public class Sentry : RotatingSprite
    {
        protected Game myGame;

        public bool deathExplosionOver = false;

        float chaseRadius = 200;



        bool playerFound = false;

        bool readyToFire = false;

        public Vector2 direction;

        private SuperProjectile sentrySuperProjectile;

        public SuperProjectile SentrySuperProjectile
        {
            get
            {
                return sentrySuperProjectile;
            }

            set
            {
                sentrySuperProjectile = value;
            }
        }

        // Sentry constructor
        public Sentry(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            myGame = game;

            PixelPosition = userPosition * frameWidth;

            DrawOrder = 1;

        }

        public bool inChaseZone(TilePlayer p)
        {
            float distance = Math.Abs(Vector2.Distance(this.PixelPosition, p.PixelPosition));
            if (distance <= chaseRadius)
            {
                return true;
            }

            else
            {
                return false;
            }
        }


        public void Follow(TilePlayer p)
        {
            if (inChaseZone(p) == true)
            {
                playerFound = true;
                this.angleOfRotation = TurnToFace(PixelPosition,
                                                p.PixelPosition, angleOfRotation, 1f);

                float x = p.PixelPosition.X - PixelPosition.X;
                float y = p.PixelPosition.Y - PixelPosition.Y;

                float towardsPlayer = (float)Math.Atan2(y, x);

                if (SentrySuperProjectile != null)
                {
                    // fire the rocket in the direction the player is pointing.
                    if (SentrySuperProjectile.ProjectileState == SuperProjectile.PROJECTILE_STATE.STILL && angleOfRotation == towardsPlayer)
                    {
                        SentrySuperProjectile.Fire(direction);
                    }
                }
            }

            else
            {
                playerFound = false;
            }
        }

        public void loadProjectile(SuperProjectile r)
        {
            sentrySuperProjectile = r;
        }

        public override void Update(GameTime gameTime)
        {
            // Create direction variable.
            direction = new Vector2((float)Math.Cos(angleOfRotation), (float)Math.Sin(angleOfRotation));


            //if(playerFound == true)
            //{
            //    TimeSpan projectileCoolDown = TimeSpan.FromSeconds(3);

            //    projectileCoolDown -= gameTime.ElapsedGameTime;

            //    if (projectileCoolDown < TimeSpan.Zero)
            //    {
            //        readyToFire = true;                   
            //    }

            //    //else
            //    {
            //        readyToFire = false;
            //    }
            //}

            #region Handle Projectile
            if (SentrySuperProjectile != null && SentrySuperProjectile.ProjectileState == SuperProjectile.PROJECTILE_STATE.STILL)
            {
                SentrySuperProjectile.PixelPosition = this.PixelPosition;
            }

            if (SentrySuperProjectile != null)
            {
                SentrySuperProjectile.Update(gameTime);
            }
            #endregion

            base.Update(gameTime);
        }

        //public override void Draw(GameTime gameTime)
        //{
        //    base.Draw(gameTime);

        //    if (SentrySuperProjectile != null && SentrySuperProjectile.ProjectileState != SuperProjectile.PROJECTILE_STATE.STILL)
        //    {
        //        SentrySuperProjectile.Draw(gameTime);
        //    }
        //}

    }

}


