using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD27
{
    public class Door
    {
        public Vector3 Position;

        public bool IsOpen = false;
        public int Dir = 0;

        bool opening;
        bool closing;

        double frameTime = 0;
        double frameTargetTime = 100;
        int currentFrame = 0;
        
        VoxelSprite spriteSheet;

        public Door(Vector3 pos, int dir, VoxelSprite sheet)
        {
            Position = pos;
            Dir = dir;

            spriteSheet = sheet;
        }

        public void Update(GameTime gameTime)
        {
            frameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (frameTime >= frameTargetTime)
            {
                frameTime = 0;
                if (opening)
                {
                    currentFrame++;
                    if (currentFrame == spriteSheet.AnimChunks.Count - 1)
                    {
                        opening = false;
                        IsOpen = true;
                    }
                }
                if (closing && currentFrame>0)
                {
                    currentFrame--;
                    if (currentFrame == 0)
                    {
                        closing = false;
                        IsOpen = false;
                    }
                }


            }
        }

        public void Draw(GraphicsDevice gd, Camera gameCamera, BasicEffect drawEffect)
        {
            drawEffect.World = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationZ((Dir == 1 || Dir == 3) ? MathHelper.PiOver2 : 0f) * Matrix.CreateTranslation(Position) * gameCamera.worldMatrix;

            foreach (EffectPass pass in drawEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                AnimChunk c = spriteSheet.AnimChunks[currentFrame];
                gd.DrawUserIndexedPrimitives<VertexPositionNormalColor>(PrimitiveType.TriangleList, c.VertexArray, 0, c.VertexArray.Length, c.IndexArray, 0, c.VertexArray.Length / 2);
            }
        }

        internal void Close()
        {
            closing = true;
        }

        internal void Open()
        {
            opening = true;
        }
    }
}
