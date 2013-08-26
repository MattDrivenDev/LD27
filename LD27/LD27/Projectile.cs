using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD27
{
    public enum ProjectileType
    {
        Laserbolt,
    }

    public class Projectile
    {
        const float GRAVITY = 0.03f;

        public ProjectileType Type;

        public Room Room;

        public bool Active;
        public Vector3 Position;
        public Vector3 Speed;
        public double Life;
        public double Time;
        public Matrix Rotation;
        public Vector3 rotSpeed;
        public bool affectedByGravity;

        public bool Deflected = false;

        float rotX;
        float rotY;

        public Projectile()
        {

        }

        public void Update(GameTime gameTime, Room currentRoom, Hero gameHero)
        {
            Time += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (affectedByGravity) Speed.Z += GRAVITY;

            CheckCollisions(currentRoom, gameHero);

            Position += Speed;

            switch (Type)
            {
                case ProjectileType.Laserbolt:
                    
                    break;
            }

            if (Time >= Life)
            {
                //if (Type == ProjectileType.Grenade || Type == ProjectileType.Rocket)
                //{
                //    ParticleController.Instance.SpawnExplosion(Position);
                //    gameWorld.Explode(Position + new Vector3(0,0,-2f), 5f);
                //}
                Active = false;
            }

            
        }

        void CheckCollisions(Room currentRoom, Hero gameHero)
        {
            Vector3 worldSpace; 
            switch (Type)
            {
                case ProjectileType.Laserbolt:
                    for (float d = 0f; d < 1f; d += 0.25f)
                    {
                        worldSpace = VoxelWorld.FromScreenSpace(Position + (d * ((Position + Speed) - Position)));
                        Voxel v = Room.World.GetVoxel(Position + (d*((Position+Speed)-Position)));

                        if (v.Active && Active)
                        {
                            if (v.Destructable == 1)
                            {
                                Room.World.SetVoxelActive((int)worldSpace.X, (int)worldSpace.Y, (int)worldSpace.Z, false);
                                if(Room == currentRoom) for (int i = 0; i < 4; i++) ParticleController.Instance.Spawn(Position, new Vector3(-0.05f + ((float)Helper.Random.NextDouble() * 0.1f), -0.05f + ((float)Helper.Random.NextDouble() * 0.1f), -((float)Helper.Random.NextDouble() * 0.5f)), 0.25f, new Color(v.SR, v.SG, v.SB), 1000, true);
                               
                            }
                            Active = false;
                        }
                        if (gameHero.boundingSphere.Contains(Position + (d * ((Position + Speed) - Position))) == ContainmentType.Contains)
                        {
                            if (!gameHero.DoHit(Position + (d * ((Position + Speed) - Position)), Speed, 5f))
                            {
                                Speed = -Speed;
                                Deflected = true;
                                Rotation = Matrix.CreateRotationZ(Helper.V2ToAngle(new Vector2(Speed.X,Speed.Y)));
                            }
                            else Active = false;
                        }
                        if (Deflected) foreach (Enemy e in EnemyController.Instance.Enemies.Where(en => en.Room == Room)) { if (e.boundingSphere.Contains(Position + (d * ((Position + Speed) - Position))) == ContainmentType.Contains) { e.DoHit(Position + (d * ((Position + Speed) - Position)), Speed, 5f); Active = false; } }

                    }
                    break;
                //case ProjectileType.Grenade:
                //    float checkRadius = 1f;
                //    float radiusSweep = 0.5f;
                //    Vector2 v2Pos = new Vector2(Position.X,Position.Y);
                //    Voxel checkVoxel;
                //    Vector3 checkPos;
                //    if (Speed.Z > 0f)
                //    {
                //        for (float z = 0f; z < 2f; z+=1f)
                //        {
                //            Voxel v = gameWorld.GetVoxel(Position + new Vector3(0f, 0f, z));
                //            if (v.Active && gameWorld.CanCollideWith(v.Type)) Speed = new Vector3(Speed.X * 0.6f, Speed.Y * 0.6f, -(Speed.Z / 2f));
                //        }
                //    }
                //    if (Speed.Y < 0f)
                //    {
                //        for (float r = checkRadius; r > 0f; r -= 1f)
                //        {
                //            for (float a = -MathHelper.PiOver2 - radiusSweep; a < -MathHelper.PiOver2 + radiusSweep; a += 0.02f)
                //            {
                //                checkPos = new Vector3(Helper.PointOnCircle(ref v2Pos, r, a), Position.Z);
                //                checkVoxel = gameWorld.GetVoxel(checkPos);
                //                if ((checkVoxel.Active && gameWorld.CanCollideWith(checkVoxel.Type)))
                //                {
                //                    Speed.Y = 0f;
                //                }
                //            }
                //        }
                //    }
                //    if (Speed.Y > 0f)
                //    {
                //        for (float r = checkRadius; r > 0f; r -= 1f)
                //        {
                //            for (float a = MathHelper.PiOver2 - radiusSweep; a < MathHelper.PiOver2 + radiusSweep; a += 0.02f)
                //            {
                //                checkPos = new Vector3(Helper.PointOnCircle(ref v2Pos, r, a), Position.Z);
                //                checkVoxel = gameWorld.GetVoxel(checkPos);
                //                if ((checkVoxel.Active && gameWorld.CanCollideWith(checkVoxel.Type)))
                //                {
                //                    Speed.Y = 0f;
                //                }
                //            }
                //        }
                //    }
                //    if (Speed.X < 0f)
                //    {
                //        for (float r = checkRadius; r > 0f; r -= 1f)
                //        {
                //            for (float a = -MathHelper.Pi - radiusSweep; a < -MathHelper.Pi + radiusSweep; a += 0.02f)
                //            {
                //                checkPos = new Vector3(Helper.PointOnCircle(ref v2Pos, r, a), Position.Z);
                //                checkVoxel = gameWorld.GetVoxel(checkPos);
                //                if ((checkVoxel.Active && gameWorld.CanCollideWith(checkVoxel.Type)))
                //                {
                //                    Speed.X = 0f;
                //                }
                //            }
                //        }
                //    }
                //    if (Speed.X > 0f)
                //    {
                //        for (float r = checkRadius; r > 0f; r -= 1f)
                //        {
                //            for (float a = -radiusSweep; a < radiusSweep; a += 0.02f)
                //            {
                //                checkPos = new Vector3(Helper.PointOnCircle(ref v2Pos, r, a), Position.Z);
                //                checkVoxel = gameWorld.GetVoxel(checkPos);
                //                if ((checkVoxel.Active && gameWorld.CanCollideWith(checkVoxel.Type)))
                //                {
                //                    Speed.X = 0f;
                //                }
                //            }
                //        }
                //    }
                //    break;
            }
        }

        
    }
}
