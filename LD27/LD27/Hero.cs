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
    public class Hero
    {
        public Vector3 Position;
        public float Rotation;
        public Vector3 Speed;

        public int RoomX;
        public int RoomY;

        VoxelSprite spriteSheet;

        BasicEffect drawEffect;

        float moveSpeed = 0.5f;

        double frameTime = 0;
        double frameTargetTime = 100;
        int currentFrame = 0;

        public Hero(int startX, int startY, Vector3 pos)
        {
            RoomX = startX;
            RoomY = startY;

            Position = pos;
        }

        public void LoadContent(ContentManager content, GraphicsDevice gd)
        {
            spriteSheet = new VoxelSprite(16, 16, 16);
            LoadVoxels.LoadSprite(Path.Combine(content.RootDirectory, "dude.vxs"), ref spriteSheet);

            drawEffect = new BasicEffect(gd)
            {
                VertexColorEnabled = true
            };
        }

        public void Update(GameTime gameTime, Camera gameCamera, Room currentRoom)
        {
            CheckCollisions(currentRoom.World);
            Position += Speed;

            Vector2 v2pos = new Vector2(Position.X, Position.Y);
            Vector2 v2speed = new Vector2(Speed.X, Speed.Y);
            if (Speed.Length() > 0f)
            {
                frameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameTime >= frameTargetTime)
                {
                    frameTime = 0;
                    currentFrame++;
                    if (currentFrame == 4) currentFrame = 0;
                }
                Rotation = Helper.TurnToFace(v2pos, v2pos + (v2speed * 50f), Rotation, 1f, 0.5f);
            }

            drawEffect.Projection = gameCamera.projectionMatrix;
            drawEffect.View = gameCamera.viewMatrix;
            drawEffect.World = gameCamera.worldMatrix *
                               Matrix.CreateRotationX(MathHelper.PiOver2) *
                               Matrix.CreateRotationZ(Rotation - MathHelper.PiOver2) *
                               Matrix.CreateTranslation(new Vector3(0, 0, (-(spriteSheet.Z_SIZE * SpriteVoxel.HALF_SIZE)) + SpriteVoxel.HALF_SIZE)) *
                               Matrix.CreateScale(0.9f) *
                               Matrix.CreateTranslation(Position);
        }

        public void Draw(GraphicsDevice gd, Camera gameCamera)
        {

            foreach (EffectPass pass in drawEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                AnimChunk c = spriteSheet.AnimChunks[currentFrame];
                gd.DrawUserIndexedPrimitives<VertexPositionNormalColor>(PrimitiveType.TriangleList, c.VertexArray, 0, c.VertexArray.Length, c.IndexArray, 0, c.VertexArray.Length / 2);
            }
        }

        public void Move(Vector2 virtualJoystick)
        {
            Vector3 dir = new Vector3(virtualJoystick, 0f);
            Speed = dir * moveSpeed;
        }

        void CheckCollisions(VoxelWorld world)
        {
            float checkRadius = 4f;
            float radiusSweep = 0.5f;
            Vector2 v2Pos = new Vector2(Position.X, Position.Y);
            float checkHeight = Position.Z - 4f;
            Voxel checkVoxel;
            Vector3 checkPos;

            if (Speed.Y < 0f)
            {
                for (float a = -MathHelper.PiOver2 - radiusSweep; a < -MathHelper.PiOver2 + radiusSweep; a += 0.02f)
                {
                    checkPos = new Vector3(Helper.PointOnCircle(ref v2Pos, checkRadius, a), checkHeight);
                    checkVoxel = world.GetVoxel(checkPos);
                    if ((checkVoxel.Active && world.CanCollideWith(checkVoxel.Type)))
                    {
                        Speed.Y = 0f;
                    }
                }
            }
            if (Speed.Y > 0f)
            {
                for (float a = MathHelper.PiOver2 - radiusSweep; a < MathHelper.PiOver2 + radiusSweep; a += 0.02f)
                {
                    checkPos = new Vector3(Helper.PointOnCircle(ref v2Pos, checkRadius, a), checkHeight);
                    checkVoxel = world.GetVoxel(checkPos);
                    if ((checkVoxel.Active && world.CanCollideWith(checkVoxel.Type)))
                    {
                        Speed.Y = 0f;
                    }
                }
            }
            if (Speed.X < 0f)
            {
                for (float a = -MathHelper.Pi - radiusSweep; a < -MathHelper.Pi + radiusSweep; a += 0.02f)
                {
                    checkPos = new Vector3(Helper.PointOnCircle(ref v2Pos, checkRadius, a), checkHeight);
                    checkVoxel = world.GetVoxel(checkPos);
                    if ((checkVoxel.Active && world.CanCollideWith(checkVoxel.Type)))
                    {
                        Speed.X = 0f;
                    }
                }
            }
            if (Speed.X > 0f)
            {
                for (float a = -radiusSweep; a < radiusSweep; a += 0.02f)
                {
                    checkPos = new Vector3(Helper.PointOnCircle(ref v2Pos, checkRadius, a), checkHeight);
                    checkVoxel = world.GetVoxel(checkPos);
                    if ((checkVoxel.Active && world.CanCollideWith(checkVoxel.Type)))
                    {
                        Speed.X = 0f;
                    }
                }
            }
        }
    }
}
