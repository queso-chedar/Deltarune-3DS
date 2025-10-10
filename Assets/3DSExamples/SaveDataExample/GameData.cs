using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class GameData
{
	// ---- Lifetime ----

	public GameData(string name)
	{
		this.name = name;
	}

	// ---- Methods ----

	public static List<GameData> LoadAll(string rootDir)
	{
		List<GameData> result = new List<GameData>();

		string fileName = rootDir + "games.bin";
		if (File.Exists(fileName))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.Open(fileName, FileMode.Open, FileAccess.Read);
			result = (List<GameData>)binaryFormatter.Deserialize(file);
			file.Close();
		}

		return result;
	}

	// This method illustrates how to save a single file.
	public static void SaveAll(string rootDir, List<GameData> games)
	{
#if UNITY_N3DS
		// It is not necessary to mount/unmount manually, but if you do then the unmount will happen immediately.
		// If you don't, then the file system is mounted as required, then unmounted at the end of the first engine tick where it's no longer being used.
		UnityEngine.N3DS.FileSystemSave.Mount();
#endif
		
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		string fileName = rootDir + "games.bin";
		FileStream file = File.Create(fileName);
		binaryFormatter.Serialize(file, games);
		file.Close();

#if UNITY_N3DS
		// This calls "nn::fs::CommitSaveData()", so the save operation is guaranteed safe and atomic.
		// If power is lost during the save operation, or some similar error occurs, then the save will not be corrupted.
		UnityEngine.N3DS.FileSystemSave.Unmount();
#endif
	}

	// This method illustrates how to save multiple files with only a single "nn::fs::CommitSaveData" call.
	// It is provided for reference only, it's not hooked up to the UI.
	public static void SaveMulti(string rootDir, List<GameData> games)
	{
		for (int index = 0; index < games.Count; index++)
		{
			GameData game = games[index];
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			string fileName = rootDir + "game" + index + ".bin";
			FileStream file = File.Create(fileName);					// <-- The file system will automatically be mounted here.
			binaryFormatter.Serialize(file, game);
			file.Close();
		}

		// The file system will automatically be unmounted at the end of the engine tick, because no File objects refer to it any more.
	}

	// ---- Variables ----

	// The name of this save.
	public string name;

	// Some data.
	public int goldCoins;
}
