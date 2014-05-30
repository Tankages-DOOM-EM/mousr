using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	public int CurrentLevel = 0;
	public int WorldSize = 8;
	public GameObject LevelText;
	public GameObject Coin;
	public GameObject TimeBoost;
	public IList<GameObject> WorldCoins = new List<GameObject>();
	private WorldGenerator WorldGenerator;
	private CollectableManager CollectableManager;

	void Start() {
		LevelText = GameObject.Find ("LevelText");
		WorldGenerator = gameObject.GetComponent<WorldGenerator> ();
		CollectableManager = gameObject.GetComponent<CollectableManager> ();
	}

	public void LoadNextLevel() {
		LoadLevel(CurrentLevel + 1);
	}

	public void LoadLevel(int level) {
		DestroyWorldCoins ();
		CurrentLevel = level;

		int width = WorldSize,
			height = WorldSize;

		if(level == 3) {
			width = 15;
			height = 4;
		}

		WorldGenerator.GenerateWorld (width, height);
		LevelText.guiText.text = "Level " + CurrentLevel.ToString ();
		CreateCoins (width, height, 10);
		CreateTimeBoosts (width, height, 2);
	}

	private void DestroyWorldCoins() {
		foreach (var coin in WorldCoins) {
			if(coin) {
				Destroy (coin);
			}
		}
		WorldCoins.Clear ();
	}

	private void CreateCoins(int width, int height, int count) {
		var goalPos = Convert.WorldToUnit (WorldGenerator.Goal.transform.position);
		for (var i = 0; i < count; ++i) {
			var x = MazeRandom.Next (0, width);
			var y = MazeRandom.Next(0, height);
			while(goalPos.X == x && goalPos.Y == y) {
				x = MazeRandom.Next (0, width);
				y = MazeRandom.Next(0, height);
			}
			WorldCoins.Add (CollectableManager.SpawnCollectable(x,y,CollectableConstants.CoinId));
		}

	}

	private void CreateTimeBoosts(int width, int height, int count) {
		var goalPos = Convert.WorldToUnit (WorldGenerator.Goal.transform.position);
		for (var i = 0; i < count; ++i) {
			var x = MazeRandom.Next (0, width);
			var y = MazeRandom.Next(0, height);
			while(goalPos.X == x && goalPos.Y == y) {
				x = MazeRandom.Next (0, width);
				y = MazeRandom.Next(0, height);
			}
			WorldCoins.Add (CollectableManager.SpawnCollectable(x,y,CollectableConstants.TimeBoostId));
		}
	}
}

