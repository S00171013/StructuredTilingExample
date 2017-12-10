using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimatedSprite;
using Microsoft.Xna.Framework;
using Tiling;

namespace Tiler
{
    public class Lock : RotatingSprite
    {
        protected Game myGame;

        public bool open = false;

        float chaseRadius = 200;

        // Lock constructor
        public Lock(Game game, Vector2 location,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, location, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            myGame = game;

            PixelPosition = location * frameWidth;

            DrawOrder = 1;

        }     
     

        public override void Update(GameTime gameTime)
        {                
            base.Update(gameTime);
        }

    }

}
