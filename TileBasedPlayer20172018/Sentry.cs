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
    class Sentry : RotatingSprite
    {
        protected Game myGame;


        // Sentry constructor
        public Sentry(Game game, Vector2 userPosition,         
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth) 
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {

            myGame = game;

            myGame.Components.Add(this);
            DrawOrder = 1;

        }


        public void Follow(TilePlayer p)
        {
            //if (inChaseZone(p) == true)
            //{
            this.angleOfRotation = TurnToFace(PixelPosition,
                                            p.PixelPosition, angleOfRotation, 1f);
        }

        //EnemyProjectile.fire(p.position);

        public override void Update(GameTime gameTime)
        {
            Follow(player1);

            base.Update(gameTime);

        }



        }

    }


