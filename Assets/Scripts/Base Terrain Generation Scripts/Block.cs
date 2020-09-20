//The block class will be the default class from which all future block types will inherit and override.
//Dylan Buchheim, 2019

using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class Block 
{
    //Global Variables --------------------------------
    public enum Direction {north,east,south,west,up,down};
    const float tileSize = 1f;

    public bool changed = true;

    //Block Constructor -------------------------------
    public Block() {

    }
    //Tile Struct -------------------------------------
    public struct Tile {
        public int x;
        public int y;
    }

    //Block Mesh Data ---------------------------------
    public virtual MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.useRenderDataForCol = true;
        if (!chunk.GetBlock(x, y+1, z).IsSolid(Direction.down)) {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }
        if (!chunk.GetBlock(x, y-1, z).IsSolid(Direction.up))
        {
            meshData = FaceDataDown(chunk, x, y, z, meshData);
        }
        if (!chunk.GetBlock(x, y, z+1).IsSolid(Direction.south))
        {
            meshData = FaceDataNorth(chunk, x, y, z, meshData);
        }
        if (!chunk.GetBlock(x+1, y, z).IsSolid(Direction.west))
        {
            meshData = FaceDataEast(chunk, x, y, z, meshData);
        }
        if (!chunk.GetBlock(x, y, z-1).IsSolid(Direction.north))
        {
            meshData = FaceDataSouth(chunk, x, y, z, meshData);
        }
        if (!chunk.GetBlock(x-1, y, z).IsSolid(Direction.east))
        {
            meshData = FaceDataWest(chunk, x, y, z, meshData);
        }
        return meshData;
    }
    //Block Face Mesh Data Functions
    protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVsArray(Direction.up));
        meshData.uv3.AddRange(TerrainUVs(Direction.up));
        return meshData;
    }
    protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVsArray(Direction.down));
        meshData.uv3.AddRange(TerrainUVs(Direction.down));
        return meshData;
    }
    protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVsArray(Direction.north));
        meshData.uv3.AddRange(TerrainUVs(Direction.north));
        return meshData;
    }
    protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVsArray(Direction.east));
        meshData.uv3.AddRange(TerrainUVs(Direction.east));
        return meshData;
    }
    protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVsArray(Direction.south));
        meshData.uv3.AddRange(TerrainUVs(Direction.south));
        return meshData;
    }
    protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVsArray(Direction.west));
        meshData.uv3.AddRange(TerrainUVs(Direction.west));
        return meshData;
    }
    //IsSolid Function, will return the solidity of a blocks face.
    public virtual bool IsSolid(Direction direction) {
        switch (direction){
            case Direction.north:
                return true;
            case Direction.east:
                return true;
            case Direction.south:
                return true;
            case Direction.west:
                return true;
            case Direction.up:
                return true;
            case Direction.down:
                return true;
        }
        return false;
    }
    /*Texture Position Function, used to return the tile position in the sprite atlas
     * based on the type of block and the direction of the face being textured*/
    public virtual Tile TexturePosition(Direction direction) {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }

    public virtual int TextureLayer(Direction direction) {
        int layer;
        layer = 0;
        return layer;
    }
    //Face UVs function will generate the uvs for the texture of the block
    public virtual Vector2[] FaceUVs(Direction direction) {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(direction);
        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y);
        return UVs;
    }

    public virtual Vector2[] FaceUVsArray(Direction direction) {
        Vector2[] UVs = new Vector2[4];
        UVs[0] = new Vector2(tileSize, 0);
        UVs[1] = new Vector2(tileSize, tileSize);
        UVs[2] = new Vector2(0, tileSize);
        UVs[3] = new Vector2(0, 0);
        return UVs;
    }

    public virtual Vector2[] TerrainUVs(Direction direction) {
        int layer = TextureLayer(direction);
        Vector2[] UVs = new Vector2[4];
        UVs[0] = new Vector2(layer,0);
        UVs[1] = new Vector2(layer,0);
        UVs[2] = new Vector2(layer,0);
        UVs[3] = new Vector2(layer,0);
        return UVs;
    }
}
