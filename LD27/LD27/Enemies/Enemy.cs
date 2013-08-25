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

        public float Scale = 1f;

        public Room Room;

        public bool Active = true;

        public int CurrentFrame = 0;

        public VoxelSprite spriteSheet;

        public double animTime = 0;
        public double animTargetTime = 500;
        public double numFrames = 2;

        public float groundHeight;

        public BoundingSphere boundingSphere = new BoundingSphere();

        public double knockbackTime = 0;

        public Enemy(Vector3 pos, Room room, VoxelSprite sprite)
        {
            Position = pos;
            Room = room;

            spriteSheet = sprite;
        }

        public virtual void Update(GameTime gameTime, Room currentRoom, Hero gameHero, List<Door> doors)
        {
            CheckCollisions(currentRoom.World, doors, currentRoom, gameHero);

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

            for (float z = Position.Z; z < 25f;z+=0.1f)
            {
                if (Room.World.GetVoxel(new Vector3(Position.X, Position.Y, z)).Active) { groundHeight = z; break; }
            }

            boundingSphere = new BoundingSphere(Position, 4f);

            if (knockbackTime > 0) knockbackTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public virtual void DoHit(Vector3 attackPos, Vector3 vector3, float p)
        {

        }

        public virtual void DoCollide(bool x, bool y, bool z, Vector3 checkPosition, Room currentRoom, Hero gameHero, bool withPlayer)
        {
            if (x) Speed.X = 0;
            if (y) Speed.Y = 0;
            if (z) Speed.Z = 0;
        }

        public virtual void CheckCollisions(VoxelWorld world, List<Door> doors, Room currentRoom, Hero gameHero)
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
                        DoCollide(false, true, false, checkPos, currentRoom, gameHero, false);
                    }
                    foreach (Door d in doors) { if (d.CollisionBox.Contains(checkPos) == ContainmentType.Contains) DoCollide(false, true, false, checkPos, currentRoom, gameHero, false); break; }
                    if (knockbackTime <= 0) foreach (Enemy e in EnemyController.Instance.Enemies.Where(en => en.Room == Room)) { if (e.boundingSphere.Contains(checkPos) == ContainmentType.Contains) DoCollide(false, true, false, checkPos, currentRoom, gameHero, false); break; }
                    if (gameHero.boundingSphere.Contains(checkPos) == ContainmentType.Contains) { DoCollide(false, true, false, checkPos, currentRoom, gameHero, true); break; }
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
                        DoCollide(false, true, false, checkPos, currentRoom, gameHero, false);
                    }
                    foreach (Door d in doors) { if (d.CollisionBox.Contains(checkPos) == ContainmentType.Contains) DoCollide(false, true, false, checkPos, currentRoom, gameHero, false); break; }
                    if (knockbackTime <= 0) foreach (Enemy e in EnemyController.Instance.Enemies.Where(en => en.Room == Room)) { if (e.boundingSphere.Contains(checkPos) == ContainmentType.Contains) DoCollide(false, true, false, checkPos, currentRoom, gameHero, false); break; }
                    if (gameHero.boundingSphere.Contains(checkPos) == ContainmentType.Contains) { DoCollide(false, true, false, checkPos, currentRoom, gameHero, true); break; }

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
                        DoCollide(true, false, false, checkPos, currentRoom, gameHero, false);
                    }
                    foreach (Door d in doors) { if (d.CollisionBox.Contains(checkPos) == ContainmentType.Contains) DoCollide(true, false, false, checkPos, currentRoom, gameHero, false); break; }
                    if (knockbackTime <= 0) foreach (Enemy e in EnemyController.Instance.Enemies.Where(en => en.Room == Room)) { if (e.boundingSphere.Contains(checkPos) == ContainmentType.Contains) DoCollide(true, false, false, checkPos, currentRoom, gameHero, false); break; }
                    if (gameHero.boundingSphere.Contains(checkPos) == ContainmentType.Contains) { DoCollide(true, false, false, checkPos, currentRoom, gameHero, true); break; }

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
                        DoCollide(true, false, false, checkPos, currentRoom, gameHero, false);
                    }
                    foreach (Door d in doors) { if (d.CollisionBox.Contains(checkPos) == ContainmentType.Contains) DoCollide(true, false, false, checkPos, currentRoom, gameHero, false); break; }
                    if (knockbackTime <= 0) foreach (Enemy e in EnemyController.Instance.Enemies.Where(en => en.Room == Room)) { if (e.boundingSphere.Contains(checkPos) == ContainmentType.Contains) DoCollide(true, false, false, checkPos, currentRoom, gameHero, false); break; }
                    if (gameHero.boundingSphere.Contains(checkPos) == ContainmentType.Contains) { DoCollide(true, false, false, checkPos, currentRoom, gameHero, true); break;}

                }
            }
        }

        
    }
}
