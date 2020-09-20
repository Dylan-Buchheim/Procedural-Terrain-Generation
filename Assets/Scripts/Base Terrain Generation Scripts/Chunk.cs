//Chunk is a class which stores the data of a certain "chunk" of voxels in the world.
//Dylan Buchheim, 2019
using System.Collections;
using UnityEngine;
//Requires these components to properly render and collide with the world.
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{
    //Global Variables -------------------------------------

    //Three dimentional array which stores all the blocks in the chunk
    public Block[,,] blocks = new Block[chunksize,chunksize,chunksize];
    //Integer which will determine the size of the chunk in all three directions
    public static int chunksize = 16;
    //Boolean which will act as a flag to signal that the chunk needs to be updated at the end of the frame
    public bool update = true;
    //Mesh Filter and Collider component variables
    MeshFilter filter;
    MeshCollider coll;
    //References to the World Object that holds the chunk and the its Position in said World
    public World world;
    public WorldPos pos;

    //Start Function ---------------------------------------
    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }
    // Update Function -------------------------------------
    void Update()
    {
        if (update) {
            update = false;
            UpdateChunk();
        }
    }
    //Get Block Function -----------------------------------
    public Block GetBlock(int x, int y, int z) {
        if (InRange(x) && InRange(y) && InRange(z)) {
            return blocks[x, y, z];
        }
        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
        
    }
    public static bool InRange(int index) {
        if (index < 0 || index >= chunksize) {
            return false;
        }
        return true;
    }
    //Set Block Function -----------------------------------
    public void SetBlock(int x, int y, int z, Block block){
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }
    //SetBlockUnmodified Function, this function will set all the blocks in the generated chunk to be unmodified by the player
    public void SetBlocksUnmodified() {
        foreach (Block block in blocks) {
            block.changed = false;
        }
    }
    //Update Chunk Function --------------------------------
    void UpdateChunk() {
        MeshData meshData = new MeshData();
        //Updates the entire chunk based on its block data
        for (int x = 0; x < chunksize; x++){
            for (int y = 0; y < chunksize; y++){
                for (int z = 0; z < chunksize; z++){
                    meshData = blocks[x, y, z].BlockData(this,x,y,z,meshData);
                }
            }
        }
        //Sends the updated mesh info to the render function
        RenderMesh(meshData);
    }
    //Render Mesh Function ---------------------------------
    void RenderMesh(MeshData meshData) {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.verticies.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();

        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.uv3 = meshData.uv3.ToArray();
        filter.mesh.RecalculateNormals();

        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVerticies.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();
        coll.sharedMesh = mesh;
    }
}
