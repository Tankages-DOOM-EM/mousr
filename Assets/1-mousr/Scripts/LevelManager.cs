using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	public int CurrentLevel = 0;
	public int WorldSize = 10;
	public GameObject LevelDisplay;

	public TopDownPlayerController PlayerController;
	
	public GameObject PreFabTitleMessage;
	public GameObject PreFabNextLevelMessage;
	public GameObject PreFabStartMessage;
	public GameObject PreFabGameOverMessage;

	public GameObject Dimmer;

	private WorldGenerator WorldGenerator;
	private Timer Timer;
	private GameObject TitleMessage;
	private GameObject NextLevelMessage;
	private GameObject StartMessage;
	private GameObject GameOverMessage;

	void Start() {
		WorldGenerator = gameObject.GetComponent<WorldGenerator> ();
		PlayerController = gameObject.GetComponent<TopDownPlayerController> ();
		Timer = gameObject.GetComponent<Timer> ();
		
		NextLevelMessage = Instantiate (PreFabNextLevelMessage) as GameObject;
		StartMessage = Instantiate (PreFabStartMessage) as GameObject;
		TitleMessage = Instantiate (PreFabTitleMessage) as GameObject;
		GameOverMessage = Instantiate (PreFabGameOverMessage) as GameObject;

		HideAllMessages ();
		ShowTitleText ();
	}

	void FixedUpdate() {
		if (PlayerController.ControlsLocked && Input.GetButton ("Jump")) {
			StartNextLevel();
			PlayerController.ControlsLocked = false;
		}

		if (Timer.Running && Timer.Expired) {
			PlayerController.ControlsLocked = true;
			GameOver();
		}
	}
	
	public void StartNextLevel() {
		LoadNextLevel();
		HideAllMessages();
		PlayerController.SetPosition(Vector2.zero);
		
		Timer.ResetTimer();
		Timer.StartTimer();
	}

	private void HideAllMessages() {
		TitleMessage.SetActive (false);
		NextLevelMessage.SetActive (false);
		GameOverMessage.SetActive (false);
		StartMessage.SetActive(false);
		Dimmer.SetActive(false);
	}
	
	private void ShowNextLevelText() {
		NextLevelMessage.SetActive (true);
		StartMessage.SetActive(true);
		Dimmer.SetActive(true);
	}

	private void ShowTitleText() {
		TitleMessage.SetActive (true);
		StartMessage.SetActive(true);
		Dimmer.SetActive(true);
	}

	private void ShowGameOverMessage() {
		ShowTitleText ();

		GameOverMessage.SetActive (true);
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
			wc.GoalTip = true;
			wc.Width = 6;
			wc.Height = 4;
			wc.TimeBoostCount = 0;
			wc.CoinCount = 0;
			break;
		case 2:
			wc.CoinTip= true;
			wc.Width = 5;
			wc.Height = 5;
			wc.TimeBoostCount = 0;
			wc.CoinCount = 5;
			break;
		case 3:
			wc.TimeBoostTip = true;
			wc.Width = 8;
			wc.Height = 8;
			break;
		case 4:
			wc.SwitchTip = true;
			wc.BlueDoor = true;
			break;
		case 5:
			wc.Width = 15;
			wc.Height = 5;
			break;
		}

		return wc;
	}

	public void LevelComplete() {
		PlayerController.ControlsLocked = true;
		Timer.StopTimer ();
		ShowNextLevelText ();
	}

	private int GetLevelSeed(int level) {
		switch (level) {
		case 1:
			return MazeRandom.DefaultSeed;
		case 2:
			return 31337;
		case 3:
			return 2255288;
		default:
			return  (MazeRandom.DefaultSeed + level * 5)% int.MaxValue;
		}
	}

	public void LoadLevel(int level) {
		var seed = GetLevelSeed (level);
		MazeRandom.ReSeed (seed);
		CurrentLevel = level;
		WorldConfig config = GetLevelConfig (level);
		WorldGenerator.GenerateWorld (config);

		LevelDisplay.guiText.text = "Level " + CurrentLevel.ToString ();
	}

	public void GameOver() {
		PlayerController.ControlsLocked = true;
		Timer.StopTimer ();
		WorldGenerator.DestroyWorld ();
		ShowGameOverMessage ();
		CurrentLevel = 0;
	}
}

