//This class will help facilitate what we actually put in our save files.

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Save
{
    //Global Variables --------------
    public Dictionary<WorldPos, Block> blocks = new Dictionary<WorldPos, Block>();

    //Save Function will add all the blocks from a chunk to the blocks dictionary
    public Save(Chunk chunk) {
        for (int x = 0; x < Chunk.chunksize; x++) {
            for (int y = 0; y < Chunk.chunksize; y++){
                for (int z = 0; z < Chunk.chunksize; z++) {
                    if (!chunk.blocks[x,y,z].changed) {
                        continue;
                    }
                    WorldPos pos = new WorldPos(x, y, z);
                    blocks.Add(pos, chunk.blocks[x,y,z]);
                }
            }
        }
    }
}
