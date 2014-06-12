using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	public int CurrentLevel = 0;
	public int WorldSize = 8;
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
		if (level == 1) {
			wc.BlueDoor = true;
		}
		if(level == 3) {
			wc.Width = 15;
			wc.Height = 4;
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

