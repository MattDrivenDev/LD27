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
                            World.CopySprite(x * Chunk.X_SIZE, y * Chunk.X_SIZE, 14, tileSheet.AnimChunks[0], Helper.Random.Next(4), 0);
                        else
                            World.CopySprite(x * Chunk.X_SIZE, y * Chunk.X_SIZE, 14, tileSheet.AnimChunks[(x==7?2:1)], 0, 0);



            for (int i = 0; i < 3; i++)
            {
                int rx = 1 + Helper.Random.Next(6);
                int ry = 1 + Helper.Random.Next(3);

                int t = 6 + Helper.Random.Next(1);

                World.CopySprite(rx * Chunk.X_SIZE, ry * Chunk.X_SIZE, 14, tileSheet.AnimChunks[t], 0, 1);
                World.CopySprite((14 - rx) * Chunk.X_SIZE, ry * Chunk.X_SIZE, 14, tileSheet.AnimChunks[t], 0, 1);
                World.CopySprite(rx * Chunk.X_SIZE, (8 - ry) * Chunk.X_SIZE, 14, tileSheet.AnimChunks[t], 0, 1);
                World.CopySprite((14 - rx) * Chunk.X_SIZE, (8 - ry) * Chunk.X_SIZE, 14, tileSheet.AnimChunks[t], 0, 1);
            }

            for (int i = 0; i < 1; i++)
            {
                int rx = 2 + Helper.Random.Next(5);
                int t = 4 + Helper.Random.Next(3);

                World.CopySprite(rx * Chunk.X_SIZE, 4 * Chunk.X_SIZE, 14, tileSheet.AnimChunks[t], 0, 1);
                World.CopySprite((14 - rx) * Chunk.X_SIZE, 4 * Chunk.X_SIZE, 14, tileSheet.AnimChunks[t], 0, 1);
            }
            for (int i = 0; i < 1; i++)
            {
                int ry = 2 + Helper.Random.Next(2);
                int t = 4 + Helper.Random.Next(3);

                World.CopySprite(7 * Chunk.X_SIZE, ry * Chunk.X_SIZE, 14, tileSheet.AnimChunks[t], 0, 1);
                World.CopySprite(7 * Chunk.X_SIZE, (8 - ry) * Chunk.X_SIZE, 14, tileSheet.AnimChunks[t], 0, 1);
            }

            //if (!((rx == 1 || rx == 14) && ry == 4) && !((ry == 1 || ry == 8) && rx == 7))
            //{

            //    World.CopySprite(rx * Chunk.X_SIZE, ry * Chunk.X_SIZE, 14, tileSheet.AnimChunks[3], 0);

            //}

            World.UpdateWorldMeshes();
        }
    }
}
