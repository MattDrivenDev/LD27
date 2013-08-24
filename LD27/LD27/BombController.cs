using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD27
{
    

    class BombController
    {
        public static BombController Instance;

        public List<Bomb> Bombs = new List<Bomb>();

        VoxelSprite spriteSheet;

        GraphicsDevice graphicsDevice;
        BasicEffect drawEffect;

        public BombController(GraphicsDevice gd, VoxelSprite sprite)
        {
            Instance = this;

            graphicsDevice = gd;

            spriteSheet = sprite;

            drawEffect = new BasicEffect(gd)
            {
                VertexColorEnabled = true
            };
        }

        public void Update(GameTime gameTime, Room currentRoom, Hero gameHero)
        {
            foreach (Bomb b in Bombs) b.Update(gameTime, currentRoom, gameHero);

            Bombs.RemoveAll(bom => !bom.Active);
        }

        public void Draw(Camera gameCamera, Room currentRoom)
        {
            drawEffect.Projection = gameCamera.projectionMatrix;
            drawEffect.View = gameCamera.viewMatrix;

            foreach (Bomb b in Bombs.Where(bom=>bom.Room==currentRoom))
            {
                drawEffect.World = gameCamera.worldMatrix *
                                       Matrix.CreateRotationX(MathHelper.PiOver2) *
                                       Matrix.CreateRotationZ(-MathHelper.PiOver2) *
                                       Matrix.CreateScale(0.75f) *
                                       Matrix.CreateTranslation(b.Position);
                foreach (EffectPass pass in drawEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalColor>(PrimitiveType.TriangleList, spriteSheet.AnimChunks[1 + b.CurrentFrame].VertexArray, 0, spriteSheet.AnimChunks[1 + b.CurrentFrame].VertexArray.Length, spriteSheet.AnimChunks[1 + b.CurrentFrame].IndexArray, 0, spriteSheet.AnimChunks[1 + b.CurrentFrame].VertexArray.Length / 2);

                }
            }
        }

        public void Spawn(Vector3 pos, Room room)
        {
            Bomb b = new Bomb(pos, room);
            Bombs.Add(b);
        }
    }
}
