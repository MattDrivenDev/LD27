using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD27
{
    public enum EnemyType
    {
        Manhack,
        Quad
    }

    class EnemyController
    {
        public static EnemyController Instance;

        public List<Enemy> Enemies = new List<Enemy>();

        VoxelSprite spriteSheet;

        GraphicsDevice graphicsDevice;
        BasicEffect drawEffect;

        public EnemyController(GraphicsDevice gd, VoxelSprite sprite)
        {
            Instance = this;

            graphicsDevice = gd;

            spriteSheet = sprite;

            drawEffect = new BasicEffect(gd)
            {
                VertexColorEnabled = true
            };
        }

        public void Spawn(EnemyType type)
        {

        }

        public void Update(GameTime gameTime, Room currentRoom, Hero gameHero, List<Door> doors)
        {
            foreach (Enemy e in Enemies) e.Update(gameTime, currentRoom, gameHero, doors);

            Enemies.RemoveAll(en => !en.Active);
        }

        public void Draw(Camera gameCamera, Room currentRoom)
        {
            drawEffect.Projection = gameCamera.projectionMatrix;
            drawEffect.View = gameCamera.viewMatrix;

            foreach (Enemy e in Enemies.Where(en=>en.Room==currentRoom))
            {
                drawEffect.World = gameCamera.worldMatrix *
                                       Matrix.CreateRotationX(MathHelper.PiOver2) *
                                       Matrix.CreateRotationZ(-MathHelper.PiOver2) *
                                       Matrix.CreateScale(0.75f) *
                                       Matrix.CreateTranslation(e.Position);
                foreach (EffectPass pass in drawEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalColor>(PrimitiveType.TriangleList, e.spriteSheet.AnimChunks[e.CurrentFrame].VertexArray, 0, e.spriteSheet.AnimChunks[e.CurrentFrame].VertexArray.Length, e.spriteSheet.AnimChunks[e.CurrentFrame].IndexArray, 0, e.spriteSheet.AnimChunks[e.CurrentFrame].VertexArray.Length / 2);

                }
            }
        }

        
    }
}
