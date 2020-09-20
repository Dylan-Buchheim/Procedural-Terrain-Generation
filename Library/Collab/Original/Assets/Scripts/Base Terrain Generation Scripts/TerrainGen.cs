//This script will implement the simplex noise from the Noise class to generate terrain for the map

using UnityEngine;
using System.Collections;
using SimplexNoise;

public class TerrainGen
{
    //Private Variables which control the terrain generation parameters
    readonly float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.02f;
    float stoneBaseNoiseHeight = 4;
    float stoneMountainHeight = 100;
    float stoneMountainFrequency = 0.006f;
    float stoneMinHeight = -12;
    float dirtBaseHeight = 1;
    float dirtNoise = 0.007f;
    float dirtNoiseHeight = 3;

    float caveFrequency = 0.025f;
    int caveSize = 7;

    public Chunk ChunkGen(Chunk chunk) {
        for (int x = chunk.pos.x; x < chunk.pos.x + Chunk.chunksize; x++) {
            for (int z = chunk.pos.z; z < chunk.pos.z + Chunk.chunksize; z++){
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }
    public Chunk ChunkColumnGen(Chunk chunk, int x, int z) {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight) {
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);
        }

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunksize; y++)
        {
            //Get a Value to base cave generation on
            int caveChance = GetNoise(x,y,z, caveFrequency, 100);
            if (y <= stoneHeight && caveSize < caveChance)
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new Block());
            }
            else if (y <= dirtHeight - 1 && caveSize < caveChance)
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockDirt());
            }
            else if (y <= dirtHeight && caveSize < caveChance)
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockGrass());
            }
            else
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockAir());
            }
        }
        return chunk;
    }
    public static int GetNoise(int x, int y, int z, float scale, int max) {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}
