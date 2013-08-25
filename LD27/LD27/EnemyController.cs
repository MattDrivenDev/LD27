using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LD27
{
    public enum EnemyType
    {
        Manhack,
        Sentinel
    }

    public class EnemyController
    {
        public static EnemyController Instance;

        public List<Enemy> Enemies = new List<Enemy>();

        Dictionary<string, VoxelSprite> spriteSheets = new Dictionary<string,VoxelSprite>();

        GraphicsDevice graphicsDevice;
        BasicEffect drawEffect;

        public EnemyController(GraphicsDevice gd)
        {
            Instance = this;

            graphicsDevice = gd;

            drawEffect = new BasicEffect(gd)
            {
                VertexColorEnabled = true
            };
        }

        public void LoadContent(ContentManager content)
        {
            VoxelSprite manhack = new VoxelSprite(16,16,16);
            LoadVoxels.LoadSprite(Path.Combine(content.RootDirectory, "enemies\\manhack.vxs"), ref manhack);
            spriteSheets.Add("Manhack", manhack);

            VoxelSprite sentinel = new VoxelSprite(16,16,16);
            LoadVoxels.LoadSprite(Path.Combine(content.RootDirectory, "enemies\\sentinel.vxs"), ref sentinel);
            spriteSheets.Add("Sentinel", sentinel);
        }

        public void Spawn(EnemyType type, Vector3 pos, Room room)
        {
            switch (type)
            {
                case EnemyType.Manhack:
                    Enemies.Add(new Manhack(pos, room, spriteSheets["Manhack"]));
                    break;
                case EnemyType.Sentinel:
                    Enemies.Add(new Sentinel(pos, room, spriteSheets["Sentinel"]));
                    break;
            }
        }

        public void Update(GameTime gameTime, Camera gameCamera, Room currentRoom, Hero gameHero, List<Door> doors)
        {
            foreach (Enemy e in Enemies) e.Update(gameTime, currentRoom, gameHero, doors);

            Enemies.RemoveAll(en => !en.Active);

            drawEffect.World = gameCamera.worldMatrix;
            drawEffect.View = gameCamera.viewMatrix;
            drawEffect.Projection = gameCamera.projectionMatrix;
        }

        public void Draw(Camera gameCamera, Room currentRoom)
        {

            foreach (Enemy e in Enemies.Where(en=>en.Room==currentRoom))
            {
                drawEffect.Alpha = 1f;
                drawEffect.World = gameCamera.worldMatrix *
                                       Matrix.CreateRotationX(MathHelper.PiOver2) *
                                       Matrix.CreateRotationZ(-MathHelper.PiOver2) *
                                       Matrix.CreateScale(e.Scale) *
                                       Matrix.CreateTranslation(e.Position);
                foreach (EffectPass pass in drawEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalColor>(PrimitiveType.TriangleList, e.spriteSheet.AnimChunks[e.CurrentFrame].VertexArray, 0, e.spriteSheet.AnimChunks[e.CurrentFrame].VertexArray.Length, e.spriteSheet.AnimChunks[e.CurrentFrame].IndexArray, 0, e.spriteSheet.AnimChunks[e.CurrentFrame].VertexArray.Length / 2);

                }

                drawEffect.Alpha = 0.2f;
                drawEffect.World = gameCamera.worldMatrix *
                                       Matrix.CreateRotationX(MathHelper.PiOver2) *
                                       Matrix.CreateRotationZ(-MathHelper.PiOver2) *
                                       Matrix.CreateTranslation(new Vector3(0, 0, (-(e.spriteSheet.Z_SIZE * SpriteVoxel.HALF_SIZE)) + SpriteVoxel.HALF_SIZE)) *
                                       Matrix.CreateScale(e.Scale) *
                                       Matrix.CreateScale(new Vector3(1f,1f,0.1f)) * 
                                       Matrix.CreateTranslation(new Vector3(e.Position.X,e.Position.Y, e.groundHeight-0.35f));
                foreach (EffectPass pass in drawEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalColor>(PrimitiveType.TriangleList, e.spriteSheet.AnimChunks[e.spriteSheet.AnimChunks.Count - 1].VertexArray, 0, e.spriteSheet.AnimChunks[e.spriteSheet.AnimChunks.Count - 1].VertexArray.Length, e.spriteSheet.AnimChunks[e.spriteSheet.AnimChunks.Count - 1].IndexArray, 0, e.spriteSheet.AnimChunks[e.spriteSheet.AnimChunks.Count - 1].VertexArray.Length / 2);
                }
            }
        }

        
    }
}
