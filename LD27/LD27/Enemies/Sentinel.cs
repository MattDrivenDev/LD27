using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD27
{
    public class Sentinel : Enemy
    {
        Vector3 Target;

        public Sentinel(Vector3 pos, Room room, VoxelSprite sprite)
            : base(pos, room, sprite)
        {
            animTargetTime = 100;
            numFrames = 5;
            offsetFrame = 1;
            Target = Position;
            Rotation = (float)(Helper.Random.NextDouble() * MathHelper.TwoPi);
        }

        public override void Update(GameTime gameTime, Room currentRoom, Hero gameHero, List<Door> doors)
        {
            if (currentRoom != Room) return;

            Vector3 dir = Target - Position;
            if(dir.Length()>0f)
                dir.Normalize();
            Speed = dir * 0.2f;

            //Speed = Vector3.Clamp(Speed, new Vector3(-0.3f, -0.3f, -0.3f), new Vector3(0.3f, 0.3f, 0.3f));

            if (Vector3.Distance(Position, Target) <= 1f) Target = Position + (new Vector3(Helper.AngleToVector(((Rotation + MathHelper.Pi) - MathHelper.PiOver2) + ((float)Helper.Random.NextDouble() * MathHelper.Pi), 100f), 0f));

            Rotation = Helper.TurnToFace(new Vector2(Position.X, Position.Y), new Vector2(Position.X, Position.Y) + (new Vector2(Speed.X, Speed.Y) * 50f), Rotation, 1f, 1f);

            base.Update(gameTime, currentRoom, gameHero, doors);

            //if(Speed.Length()<0.01f) 
        }

        public override void DoCollide(bool x, bool y, bool z, Vector3 checkPosition, Room currentRoom, Hero gameHero, bool withPlayer)
        {
           // Target = new Vector3(Helper.Random.Next(Room.World.X_SIZE) * Voxel.SIZE, Helper.Random.Next(Room.World.Y_SIZE) * Voxel.SIZE, Position.Z);

            Target = Position + (new Vector3(Helper.AngleToVector(((Rotation+MathHelper.Pi) - MathHelper.PiOver2) + ((float)Helper.Random.NextDouble()*MathHelper.Pi), 100f), 0f)); //new Vector3(Helper.Random.Next(Room.World.X_SIZE) * Voxel.SIZE, Helper.Random.Next(Room.World.Y_SIZE) * Voxel.SIZE, Position.Z);  //Position + (-Speed * 100f);
            Vector3 dir = Target - Position;
            if (dir.Length() > 0f)
                dir.Normalize();
            Speed = dir * 0.2f;

            base.DoCollide(x, y, z, checkPosition, currentRoom, gameHero, withPlayer);
        }
    }
}
