using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	void OnGUI()
	{
		Rect rect = new Rect(0, 0, 60, 25);

		// Top row.
		GUI.Label(rect, "Menu:");
		rect.y += 20;

		if (GUI.Button(rect, "Load"))
			allGames = GameData.LoadAll(saveDataArchive);
		rect.x += 65;

		if (GUI.Button(rect, "Save"))
			GameData.SaveAll(saveDataArchive, allGames);
		rect.x += 65;

		if (GUI.Button(rect, "Load SD"))
			allGames = GameData.LoadAll(extSaveDataArchive);
		rect.x += 65;

		if (GUI.Button(rect, "Save SD"))
			GameData.SaveAll(extSaveDataArchive, allGames);
		rect.x += 65;

		if (GUI.Button(rect, "Create"))
		{
			string saveName = "game" + nextSaveGameId++;
			GameData gameData = new GameData(saveName);
			gameData.goldCoins = (int)(Random.value * 1000.0f);
			allGames.Add(gameData);
		}

		// List of games.
		rect.x = 5;
		rect.y += 30;
		rect.width = 100;
		GUI.Label(rect, "Games:");
		rect.y += 20;
		rect.width = 65;

		foreach (GameData gameData in allGames)
		{
			if (GUI.Button(rect, gameData.name))
				currentGame = gameData;

			rect.x += 70;
		}

		// Current game details.
		if (currentGame != null)
		{
			rect.x = 5;
			rect.y += 30;
			rect.width = 100;
			GUI.Label(rect, "Current Game:");
			rect.y += 20;

			rect.width = 65;
			if (GUI.Button(rect, "Delete"))
			{
				allGames.Remove(currentGame);
				currentGame = null;
			}
			rect.x += 70;

			rect.width = 120;
			if (currentGame != null)
				GUI.Label(rect, "Gold Coins: " + currentGame.goldCoins);
		}
	}

	// ---- Variables ----

	private List<GameData> allGames = new List<GameData>();

	private GameData currentGame;

	private int nextSaveGameId = 1;

	// Save Data archive.
	private static readonly string saveDataArchive = "data:/";

	// Expanded Save Data archive (requires an SD card to be inserted).
	private static readonly string extSaveDataArchive = "extdata:/";
}
