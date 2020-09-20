//This is the class which handles the specifics of the dirt block, it inherits from the Block class and overrides as necessary.
//Dylan Buchheim, 2019

using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class BlockDirt : Block
{
    //BlockGrass Constructor ----------------
    public BlockDirt()
            : base()
    {
    }
    /*The overridden version of Texture Position
     *This will assign the correct textures from the texture atlas for the different sides of the grass block*/
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 1;
        tile.y = 0;
        return tile;
    }

    public override int TextureLayer(Direction direction)
    {
        int layer;
        layer = 1; //index of the dirt texture in the TextureArray
        return layer;
    }
}
