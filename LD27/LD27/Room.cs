using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD27
{
    public class Room
    {
        public VoxelWorld World;

        public bool IsGap = false;

        public Room(VoxelSprite tileSheet, bool isGap)
        {
            IsGap = isGap;

            if (!isGap)
            {
                World = new VoxelWorld(15, 9, 1, true);

                CreateMap(tileSheet);
            }
        }

        void CreateMap(VoxelSprite tileSheet)
        {
            for (int x = 0; x < 16; x++)
                for (int y = 0; y < 10; y++)
                    if (x == 0 || x == 14 || y == 0 || y == 8)
                        if(x!=7 && y!=4)
                            World.CopySprite(x * Chunk.X_SIZE, y * Chunk.X_SIZE, 14, tileSheet.AnimChunks[0], Helper.Random.Next(4));
                        else
                            World.CopySprite(x * Chunk.X_SIZE, y * Chunk.X_SIZE, 14, tileSheet.AnimChunks[(x==7?2:1)], 0);



            //for (int i = 0; i < 40; i++)
            //    World.CopySprite(Helper.Random.Next(16) * Chunk.X_SIZE, Helper.Random.Next(16) * Chunk.X_SIZE, 14, tileSheet.AnimChunks[0]);

            World.UpdateWorldMeshes();
        }
    }
}
