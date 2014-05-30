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
		int width = WorldSize,
			height = WorldSize,
			coinCount = 10,
			timeBoostCount = 2;
		
		if(level == 3) {
			width = 15;
			height = 4;
		}

		return new WorldConfig {
			Width = width,
			Height = height,
			CoinCount = coinCount,
			TimeBoostCount = timeBoostCount
		};
	}

	public void LoadLevel(int level) {
		CurrentLevel = level;

		WorldConfig config = GetLevelConfig (level);
		WorldGenerator.GenerateWorld (config);

		LevelText.guiText.text = "Level " + CurrentLevel.ToString ();

	}
}

