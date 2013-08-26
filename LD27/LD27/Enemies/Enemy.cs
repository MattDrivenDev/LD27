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
        public float Rotation;
        public float Health = 100f;

        public float Scale = 1f;

        public Room Room;

        public bool Active = true;

        public int CurrentFrame = 0;

        public VoxelSprite spriteSheet;

        public double animTime = 0;
        public double animTargetTime = 500;
        public int numFrames = 2;
        public int offsetFrame = 0;

        public float groundHeight;

        public BoundingSphere boundingSphere = new BoundingSphere();

        public double knockbackTime = 0;

        public bool attacking = false;
        public double attackTime = 0;
        public double attackTargetTime = 50;
        public int numAttackFrames;
        public int attackDir=1;
        public int attackFrame = 0;

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

            if (Health <= 0f) Die();
        }

        public virtual void DoHit(Vector3 attackPos, Vector3 speed, float damage)
        {
            Health -= damage;
        }

        public virtual void DoCollide(bool x, bool y, bool z, Vector3 checkPosition, Room currentRoom, Hero gameHero, bool withPlayer)
        {
            if (x) Speed.X = 0;
            if (y) Speed.Y = 0;
            if (z) Speed.Z = 0;

            
        }

        public virtual void Die()
        {
            
           
            for (int x = 0; x < spriteSheet.X_SIZE; x++)
                for (int y = 0; y < spriteSheet.Y_SIZE; y++)
                    for (int z = 0; z < spriteSheet.Z_SIZE; z++)
                    {
                        SpriteVoxel v = spriteSheet.AnimChunks[CurrentFrame].Voxels[x, y, z];
                        if (!v.Active) continue;
                        Vector3 pos = (- new Vector3(spriteSheet.X_SIZE * Voxel.HALF_SIZE, spriteSheet.Y_SIZE * Voxel.HALF_SIZE, spriteSheet.Z_SIZE * Voxel.HALF_SIZE) * Scale) + (new Vector3(x * Voxel.SIZE, y * Voxel.SIZE, z * Voxel.SIZE) * Scale);
                        pos = Position + Vector3.Transform(pos, Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationZ(Rotation));
                        ParticleController.Instance.Spawn(pos, -(Vector3.One * 0.1f) + (Vector3.One * ((float)Helper.Random.NextDouble()*0.2f)) , 0.3f, v.Color, 3000, true);
                    }

            Active = false;
            
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
                    if (knockbackTime <= 0) foreach (Enemy e in EnemyController.Instance.Enemies.Where(en => en.Room == Room && en!=this)) { if (e.boundingSphere.Contains(checkPos) == ContainmentType.Contains) DoCollide(false, true, false, checkPos, currentRoom, gameHero, false); break; }
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
                    if (knockbackTime <= 0) foreach (Enemy e in EnemyController.Instance.Enemies.Where(en => en.Room == Room && en != this)) { if (e.boundingSphere.Contains(checkPos) == ContainmentType.Contains) DoCollide(false, true, false, checkPos, currentRoom, gameHero, false); break; }
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
                    if (knockbackTime <= 0) foreach (Enemy e in EnemyController.Instance.Enemies.Where(en => en.Room == Room && en != this)) { if (e.boundingSphere.Contains(checkPos) == ContainmentType.Contains) DoCollide(true, false, false, checkPos, currentRoom, gameHero, false); break; }
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
                    if (knockbackTime <= 0) foreach (Enemy e in EnemyController.Instance.Enemies.Where(en => en.Room == Room && en != this)) { if (e.boundingSphere.Contains(checkPos) == ContainmentType.Contains) DoCollide(true, false, false, checkPos, currentRoom, gameHero, false); break; }
                    if (gameHero.boundingSphere.Contains(checkPos) == ContainmentType.Contains) { DoCollide(true, false, false, checkPos, currentRoom, gameHero, true); break;}

                }
            }
        }

        
    }
}
