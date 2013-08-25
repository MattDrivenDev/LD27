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
            Target = new Vector3(room.World.X_SIZE * Voxel.SIZE, room.World.Y_SIZE * Voxel.SIZE, pos.Z);
        }

        public override void Update(GameTime gameTime, Room currentRoom, Hero gameHero, List<Door> doors)
        {
            if (currentRoom != Room) return;

            if (Target.X < Position.X) Speed.X -= 0.01f;
            if (Target.X > Position.X) Speed.X += 0.01f;
            if (Target.Y < Position.Y) Speed.Y -= 0.01f;
            if (Target.Y > Position.Y) Speed.Y += 0.01f;

            base.Update(gameTime, currentRoom, gameHero, doors);
        }

        public override void DoCollide(bool x, bool y, bool z, Vector3 checkPosition, Room currentRoom, Hero gameHero, bool withPlayer)
        {
            Target = new Vector3(Room.World.X_SIZE * Voxel.SIZE, Room.World.Y_SIZE * Voxel.SIZE, Position.Z);

            base.DoCollide(x, y, z, checkPosition, currentRoom, gameHero, withPlayer);
        }
    }
}
