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

            PixelPosition = userPosition*frameWidth;
            
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
                this.angleOfRotation = TurnToFace(PixelPosition,
                                                p.PixelPosition, angleOfRotation, 1f);

                if (SentrySuperProjectile != null)
                {
                    // fire the rocket in the direction the player is pointing.
                    if(SentrySuperProjectile.ProjectileState == SuperProjectile.PROJECTILE_STATE.STILL)
                    {
                        SentrySuperProjectile.Fire(direction);
                    }
                }
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


