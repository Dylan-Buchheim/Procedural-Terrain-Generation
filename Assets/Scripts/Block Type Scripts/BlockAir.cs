//Script for the air block, contains the data for the "air" block.
//Dylan Buchheim, 2019

using System.Collections;
using System;
using UnityEngine;


[Serializable]
public class BlockAir : Block
{
    //Air Block Constructor ---------------------
    public BlockAir()
        : base()
    {
    }
    //Overriden MeshData for the air block ------
    public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData){
        return meshData;
    }
    //Overriden IsSolid Function, air is never solid.
    public override bool IsSolid(Block.Direction direction){
        return false;
    }
}
