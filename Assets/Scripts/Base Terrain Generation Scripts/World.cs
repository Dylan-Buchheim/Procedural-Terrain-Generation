//The world class handles the greater dictionary of voxel chunks which make up the world.
//Dylan Buchheim, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    //Global Variables ---------------
    public string worldName = "world";
    public int seed = 1;
    public Vector3 worldSize;

    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public GameObject chunkPrefab;

    public int newChunkX;
    public int newChunkY;
    public int newChunkZ;

    public bool genChunk;

    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld();
    }
    //GenerateWorld Function, used to generate the world
    public void GenerateWorld() {
        SimplexNoise.Noise.RandomizePerm(seed);
        for (int x = -((int)worldSize.x/2); x < ((int)worldSize.x / 2); x++)
        {
            for (int y = -((int)worldSize.y / 2); y < ((int)worldSize.y / 2); y++)
            {
                for (int z = -((int)worldSize.z / 2); z < ((int)worldSize.z / 2); z++)
                {
                    CreateChunk(x * 16, y * 16, z * 16);
                }
            }
        }
    }

    public void ClearWorld() {
        for (int x = -((int)worldSize.x / 2); x < ((int)worldSize.x / 2); x++)
        {
            for (int y = -((int)worldSize.y / 2); y < ((int)worldSize.y / 2); y++)
            {
                for (int z = -((int)worldSize.z / 2); z < ((int)worldSize.z / 2); z++)
                {
                    DestroyChunk(x * 16, y * 16, z * 16);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (genChunk) {
            genChunk = false;
            WorldPos chunkPos = new WorldPos(newChunkX, newChunkY, newChunkZ);
            Chunk chunk = null;

            if (chunks.TryGetValue(chunkPos, out chunk))
            {
                DestroyChunk(chunkPos.x, chunkPos.y, chunkPos.z);
            }
            else
            {
                CreateChunk(chunkPos.x, chunkPos.y, chunkPos.z);
            }
        }
    }
    //CreateChunk Function, will be used to generate Chunk Objects in the World.
    public void CreateChunk(int x, int y, int z) {
        WorldPos worldPos = new WorldPos(x, y, z);
        //Now we Instantiate the Chunk Prefab at the worldPos
        GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(x,y,z),Quaternion.Euler(Vector3.zero)) as GameObject;

        Chunk newChunk = newChunkObject.GetComponent<Chunk>();
        newChunk.pos = worldPos;
        newChunk.world = this;

        chunks.Add(worldPos, newChunk);

        var terrainGen = new TerrainGen();
        newChunk = terrainGen.ChunkGen(newChunk);
        newChunk.SetBlocksUnmodified();
        bool loaded = Serialization.Load(newChunk);
    }
    //DestroyChunk Function, this will allow us to get rid of chunks so we can optimize.
    public void DestroyChunk(int x, int y, int z) {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x,y,z), out chunk)) {
            Serialization.SaveChunk(chunk);
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }
    //GetChunk Function, when passed a world coordinate it will return the chunk which contains that coordinate.
    public Chunk GetChunk(int x, int y, int z) {
        WorldPos pos = new WorldPos();
        float multiple = Chunk.chunksize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunksize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunksize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunksize;
        Chunk containerChunk = null;
        chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }
    //GetBlock Function, will use the Chunk returned by GetChunk to access its blocks and return the specific block at the world coordinate.
    public Block GetBlock(int x, int y, int z) {
        Chunk containerChunk = GetChunk(x,y,z);
        if (containerChunk != null){
            Block block = containerChunk.GetBlock(x - containerChunk.pos.x, y - containerChunk.pos.y, z - containerChunk.pos.z);
            return block;
        }
        else {
            return new BlockAir();
        }
    }
    //SetBlock Function, this will change the block in the current space to the block passed into the parameter.
    public void SetBlock(int x, int y, int z, Block block) {
        Chunk chunk = GetChunk(x,y,z);

        if (chunk != null) {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            chunk.update = true;

            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x-1,y,z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunksize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunksize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z -1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunksize - 1, new WorldPos(x, y, z + 1));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos) {
        if (value1 == value2) {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null) {
                chunk.update = true;
            }
        }
    }
}
