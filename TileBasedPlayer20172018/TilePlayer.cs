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
        //List<TileRef> images = new List<TileRef>() { new TileRef(15, 2, 0)};
        //TileRef currentFrame;

        //public Vector2 playerVelocity;

        //playerVelocity.X = (float)Math.Cos(angleOfRotation* 2 * Math.PI / 360);

        //        playerVelocity.Y = (float)-Math.Sin(angleOfRotation* 2 * Math.PI / 360);

        int speed = 5;
        float turnspeed = 0.03f;
        public Vector2 previousPosition;

        

        public TilePlayer(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 1;

        }

        public void Collision(Collider c)
        {
            if (BoundingRectangle.Intersects(c.CollisionField))
                PixelPosition = previousPosition;
        }

        public override void Update(GameTime gameTime)
        {
            previousPosition = PixelPosition;

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
            }          

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {                
                this.angleOfRotation += turnspeed;
            }
                           
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
