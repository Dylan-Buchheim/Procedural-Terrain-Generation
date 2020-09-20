//This is the static class which will be used to serialize all the world data for saving.
//Dylan Buchheim, 2019

using System.Collections;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization {

	//Global Variables--------------------
	public static string saveFolderName = "voxelGameSaves";

	//SaveLocation Function will create the save directory path for the current world save
	public static string SaveLocation(string worldName) {
		string saveLocation = saveFolderName + "/" + worldName + "/";

		if (!Directory.Exists(saveLocation)) {
			Directory.CreateDirectory(saveLocation);
        }

		return saveLocation;
    }
	//FileName Function, this will create the actual file name that the chunk is going to be saved as
	public static string FileName(WorldPos chunkLocation) {
		string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
		return fileName;
    }
	//SaveChunk Function, this will serialize the blocks array of the chunk and write it to file.
	public static void SaveChunk(Chunk chunk) {
        Save save = new Save(chunk);
        if (save.blocks.Count == 0) {
            return;
        }
        string saveFile = SaveLocation(chunk.world.worldName);
		saveFile += FileName(chunk.pos);

		IFormatter formatter = new BinaryFormatter();
		Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
		formatter.Serialize(stream, save);
		stream.Close();
    }

	public static bool Load(Chunk chunk) {
		string saveFile = SaveLocation(chunk.world.worldName);
		saveFile += FileName(chunk.pos);

		if (!File.Exists(saveFile)) {
			return false;
        }

		IFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(saveFile, FileMode.Open);

        Save save = (Save)formatter.Deserialize(stream);
        foreach(var block in save.blocks){
            chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
        }
		stream.Close();
		return true;
    }
}
