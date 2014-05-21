using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public int CurrentLevel = 0;
	public int WorldSize = 8;
	public GameObject LevelText;

	void Start() {
		LevelText = GameObject.Find ("LevelTextHeading");
	}

	public void LoadNextLevel() {
		LoadLevel(CurrentLevel + 1);
	}

	public void LoadLevel(int level) {
		CurrentLevel = level;

		int width = WorldSize,
			height = WorldSize;

		if(level == 3) {
			width = 15;
			height = 4;
		}

		WorldGenerator.GenerateWorld (width, height);
		LevelText.guiText.text = "Level " + CurrentLevel.ToString ();
	}
}

