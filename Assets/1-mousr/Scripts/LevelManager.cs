using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	public int CurrentLevel = 0;
	public int WorldSize = 10;
	public GameObject LevelText;
	private WorldGenerator WorldGenerator;

	void Start() {
		LevelText = GameObject.Find ("LevelText");
		WorldGenerator = gameObject.GetComponent<WorldGenerator> ();
	}

	public void LoadNextLevel() {
		LoadLevel(CurrentLevel + 1);
	}

	public WorldConfig GetLevelConfig (int level) 
	{
		WorldConfig wc = new WorldConfig {
			Width = WorldSize,
			Height = WorldSize,
			CoinCount = 10,
			TimeBoostCount = 2,
			BlueDoor = false
		};
		switch (level) {
		case 1:
			wc.Width = 6;
			wc.Height = 4;
			wc.TimeBoostCount = 0;
			wc.CoinCount = 0;
			break;
		case 2:
			wc.Width = 5;
			wc.Height = 5;
			wc.TimeBoostCount = 0;
			wc.CoinCount = 5;
			break;
		case 3:
			wc.Width = 8;
			wc.Height = 8;
			break;
		case 4:
			wc.BlueDoor = true;
			break;
		case 5:
			wc.Width = 15;
			wc.Height = 5;
			break;
		}

		return wc;
	}

	public void LoadLevel(int level) {
		CurrentLevel = level;

		WorldConfig config = GetLevelConfig (level);
		WorldGenerator.GenerateWorld (config);

		LevelText.guiText.text = "Level " + CurrentLevel.ToString ();

	}
}

