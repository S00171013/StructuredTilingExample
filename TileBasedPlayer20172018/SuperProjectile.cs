using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using AnimatedSprite;
using Tiling;

namespace Tiler
{
    public class SuperProjectile : RotatingSprite
    {
        public enum PROJECTILE_STATE { STILL, FIRING, EXPLODING };
        PROJECTILE_STATE projectileState = PROJECTILE_STATE.STILL;


        protected Game myGame;
        protected float RocketVelocity = 4.0f;

        Vector2 initialPosition;
        Vector2 Target;
        AnimateSheetSprite explosion;
        float ExplosionTimer = 0;
        float ExplosionVisibleLimit = 1000;
        Vector2 StartPosition;


        public PROJECTILE_STATE ProjectileState
        {
            get { return projectileState; }
            set { projectileState = value; }
        }

        public AnimateSheetSprite Explosion
        {
            get { return explosion; }
            set { explosion = value; }
        }

        public SuperProjectile(Game game, Vector2 userPosition, List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
            : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            Target = Vector2.Zero;
           
            myGame = game;

            explosion = new AnimateSheetSprite(myGame, Target, new List<TileRef>()
            {
                new TileRef(2, 0, 0),             
            }, 64, 64, 0f);

            explosion.Visible = false;

            //initialPosition = new Vector2(20, 20);

            //explosion = rocketExplosion;
            //explosion.position -= textureCenter;
            //explosion.Visible = false;


            //StartPosition = position;
            //PixelPosition = initialPosition;
            ProjectileState = PROJECTILE_STATE.STILL;

        }

        public void WallCollision(Collider c)
        {
            if (BoundingRectangle.Intersects(c.CollisionField))
            {
                ProjectileState = PROJECTILE_STATE.EXPLODING;
            }
        }




        public override void Update(GameTime gametime)
        {
            switch (projectileState)
            {
                case PROJECTILE_STATE.STILL:
                    this.Visible = false;
                    explosion.Visible = false;
                    break;


                // Using Lerp here could use target - pos and normalise for direction and then apply
                // Velocity
                case PROJECTILE_STATE.FIRING:
                    this.Visible = true;
                    //PixelPosition = Vector2.Lerp(PixelPosition, Target, 0.02f * RocketVelocity);
                    PixelPosition += Target;
                    // rotate towards the Target
                    this.angleOfRotation = TurnToFace(PixelPosition,
                                            Target, angleOfRotation, 1f);

                    //if (Vector2.Distance(PixelPosition, Target) < 2)
                    //    projectileState = PROJECTILE_STATE.EXPLODING;
                    break;

                case PROJECTILE_STATE.EXPLODING:
                    explosion.PixelPosition = Target;
                    explosion.Visible = true;
                    break;
            }

            // if the explosion is visible then just play the animation and count the timer
            if (explosion.Visible)
            {
                explosion.Update(gametime);
                ExplosionTimer += gametime.ElapsedGameTime.Milliseconds;
            }

            // if the timer goes off the explosion is finished.
            if (ExplosionTimer > ExplosionVisibleLimit)
            {
                explosion.Visible = false;
                ExplosionTimer = 0;
                projectileState = PROJECTILE_STATE.STILL;
            }

            base.Update(gametime);
        }
        public void Fire(Vector2 directionIn)
        {
            Target = directionIn * new Vector2(1, 1) * RocketVelocity;

            projectileState = PROJECTILE_STATE.FIRING;

            //this.PixelPosition += direction * new Vector2(1, 1) * speed;
        
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            //spriteBatch.Begin();
            //spriteBatch.Draw(spriteImage, position, SourceRectangle,Color.White);
            //spriteBatch.End();

            if (explosion.Visible)
            {
                explosion.Draw(gameTime);
            }

            //if (explosion.Visible)
            //{
            //    explosion.Draw(gameTime);
            //}
        }
    }
}
