using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD27
{
    public class Enemy
    {

        public Vector3 Position;
        public Vector3 Speed;

        public Room Room;

        public bool Active = true;

        public int CurrentFrame = 0;

        public VoxelSprite spriteSheet;

        double animTime = 0;
        double animTargetTime = 500;
        double numFrames = 2;

        public Enemy(Vector3 pos, Room room, VoxelSprite sprite)
        {
            Position = pos;
            Room = room;

            spriteSheet = sprite;
        }

        public virtual void Update(GameTime gameTime, Room currentRoom, Hero gameHero, List<Door> doors)
        {
            CheckCollisions(currentRoom.World, doors);

            Position += Speed;

            if (Speed.Length() > 0)
            {
                animTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (animTime >= animTargetTime)
                {
                    animTime = 0;

                    CurrentFrame++;
                    if (CurrentFrame == numFrames) CurrentFrame = 0;
                }
            }

          
        }

        public virtual void DoCollide(bool x, bool y, bool z)
        {
            if (x) Speed.X = 0;
            if (y) Speed.Y = 0;
            if (z) Speed.Z = 0;
        }

        public virtual void CheckCollisions(VoxelWorld world, List<Door> doors)
        {
            float checkRadius = 3.5f;
            float radiusSweep = 0.75f;
            Vector2 v2Pos = new Vector2(Position.X, Position.Y);
            float checkHeight = Position.Z - 1f;
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
                        DoCollide(false, true, false);
                    }
                    foreach (Door d in doors) { if (d.CollisionBox.Contains(checkPos) == ContainmentType.Contains) DoCollide(false, true, false); ; }
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
                        DoCollide(false, true, false);
                    }
                    foreach (Door d in doors) { if (d.CollisionBox.Contains(checkPos) == ContainmentType.Contains) DoCollide(false, true, false); ; }

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
                        DoCollide(true, false, false);
                    }
                    foreach (Door d in doors) { if (d.IsBlocked && d.CollisionBox.Contains(checkPos) == ContainmentType.Contains) DoCollide(true, false, false); }

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
                        DoCollide(true, false, false);
                    }
                    foreach (Door d in doors) { if (d.IsBlocked && d.CollisionBox.Contains(checkPos) == ContainmentType.Contains) DoCollide(true, false, false); }

                }
            }
        }
    }
}
