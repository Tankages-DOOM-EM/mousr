using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float LevelSeconds = 60;
	public float TimeRemaining;
	public bool Running = false;
	public string TimeLabel = "Time";

	public void StartTimer() {
		Running = true;
	}

	public void StopTimer(bool reset = false) {
		Running = false;
		if (reset) {
			ResetTimer ();
		}
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
		}
		gameObject.guiText.text = string.Format ("{0}: {1}", TimeLabel, Mathf.FloorToInt(TimeRemaining).ToString("D2"));
	}
}
