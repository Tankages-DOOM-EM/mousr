using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	
	public int CoinPointValue = 10;
	public int GoalPointValue = 17;
	public int TimePointValue = 3;

	public int PlayerScore;

	// Use this for initialization
	void Start () {
		PlayerScore = 0;
	}
	
	public void AddCoinScore() {
		PlayerScore += CoinPointValue;
	}
	
	public void AddGoalScore() {
		PlayerScore += GoalPointValue;
	}
	
	public void AddTimeBonus(float timeRemaining) {
		PlayerScore += Mathf.FloorToInt(TimePointValue* timeRemaining);
	}
}
