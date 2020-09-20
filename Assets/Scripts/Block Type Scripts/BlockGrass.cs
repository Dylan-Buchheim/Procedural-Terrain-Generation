//This is the class which handles the specifics of the grass block, it inherits from the Block class and overrides as necessary.
//Dylan Buchheim, 2019

using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class BlockGrass : Block
{
    //BlockGrass Constructor ----------------
    public BlockGrass()
            : base()
    { 
    }
    /*The overridden version of Texture Position
     *This will assign the correct textures from the texture atlas for the different sides of the grass block*/
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        switch (direction) {
            case Direction.up:
                tile.x = 2;
                tile.y = 0;
                return tile;
            case Direction.down:
                tile.x = 1;
                tile.y = 0;
                return tile;
        }
        tile.x = 3;
        tile.y = 0;
        return tile;
    }

    public override int TextureLayer(Direction direction)
    {
        int layer;
        switch (direction)
        {
            case Direction.up:
                layer = 2; //the index of the grass top texture in the texture array
                return layer;
            case Direction.down:
                layer = 1; //the index of the dirt texture in the texture array
                return layer;
        }
        layer = 3; //the index of the grass block side texture in the texture array
        return layer;
    }
}
