using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour {
	private ScoreManager ScoreManager;
	private Timer Timer;
	// Use this for initialization
	void Start () {
		ScoreManager = gameObject.GetComponent<ScoreManager> ();
		Timer = GameObject.Find ("Timer").GetComponent<Timer> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		switch (other.tag) {
		case "Coin":
			HandleCoin(other.gameObject);
			return;
		case "Goal":
			HandleGoal(other.gameObject);
			return;
		default:
			break;
		}
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

	// Update is called once per frame
	void Update () {
	
	}
}
