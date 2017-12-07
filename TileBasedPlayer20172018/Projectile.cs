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
    class Projectile : RotatingSprite
    {
        protected Game myGame;



        // Projectile constructor
        public Projectile(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            myGame = game;

            PixelPosition = userPosition * frameWidth;

            DrawOrder = 0;

        }
              

        public override void Update(GameTime gameTime)
        {                     
            base.Update(gameTime);
        }
      

    }

}


