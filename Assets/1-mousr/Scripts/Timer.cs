using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float LevelSeconds = 8;
	public float TimeRemaining;
	public bool Running = false;
	public bool Expired = false;
	public string TimeLabel = "Time";
	public GameObject TimerText;

	public void StartTimer() {
		Running = true;
		Expired = false;
	}

	public void StopTimer() {
		Running = false;
		Expired = false;
	}

	public void ResetTimer() {
		TimeRemaining = LevelSeconds;
	}

	public void AddTime (float seconds) {
		TimeRemaining += seconds;
	}

	// Update is called once per frame
	void Update () {
		if (Running) {
			TimeRemaining = Mathf.Max (TimeRemaining - Time.deltaTime, 0);

			Expired = Expired || TimeRemaining == 0;
		}
		if (TimeRemaining < 4 && TimeRemaining > 0) {
			TimerText.guiText.text = string.Format ("{0}: {1}", TimeLabel, TimeRemaining.ToString ("0.0"));
		} else {
			TimerText.guiText.text = string.Format ("{0}: {1}", TimeLabel, Mathf.FloorToInt (TimeRemaining).ToString ("D2"));
		}
	}
}
