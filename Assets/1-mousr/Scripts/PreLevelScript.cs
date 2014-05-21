using UnityEngine;
using System.Collections;

public class PreLevelScript : MonoBehaviour
{
	private bool Enabled = true;
	private TopDownPlayerController PlayerController;
	private GameObject LevelText;

	void Start() {
		PlayerController = GameObject.Find("Player").GetComponent<TopDownPlayerController>();
		LevelText = GameObject.Find ("LevelText");
		LevelText.SetActive (false);
	}

	private void SetEnabled(bool enabled) {
		Enabled = enabled;
		PlayerController.ControlsLocked = enabled;
		LevelText.SetActive (enabled);
	}

	public void Enable() {
		SetEnabled (true);
	}

	public void Disable() {
		SetEnabled (false);
	}

	// Update is called once per frame
	void Update ()
	{
		if (Enabled && Input.GetButtonDown ("Jump")) {
			Disable ();
		}
	}
}

