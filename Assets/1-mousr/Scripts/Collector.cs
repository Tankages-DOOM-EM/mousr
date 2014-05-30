using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour {
	private ScoreManager ScoreManager;
	private Timer Timer;

	void Start () {
		ScoreManager = gameObject.GetComponent<ScoreManager> ();
		Timer = GameObject.Find ("Timer").GetComponent<Timer> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		switch (other.tag) {
		case "Coin":
			HandleCoin(other.gameObject);
			return;
		case "TimeBoost":
			HandleTimeBoost(other.gameObject);
			return;
		case "Goal":
			HandleGoal(other.gameObject);
			return;
		default:
			break;
		}
	}

	private void HandleTimeBoost (GameObject timeBoost) {
		Destroy (timeBoost);
		Timer.AddTime (15);
	}

	private void HandleGoal(GameObject goal) {
		Destroy (goal);
		ScoreManager.AddGoalScore ();
		ScoreManager.AddTimeBonus (Timer.TimeRemaining);
	}

	private void HandleCoin(GameObject coin) {
		Destroy (coin);
		ScoreManager.AddCoinScore ();
	}
	
}
