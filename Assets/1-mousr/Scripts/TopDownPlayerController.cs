using UnityEngine;
using System.Collections;

public class TopDownPlayerController : MonoBehaviour {
	public float MoveSpeed = 1.0f;
	public int WorldSize = 8;

	private bool Loaded = false;
	public bool ControlsLocked = false;
	private GameObject IntroText;
	private GameObject Dimmer;
	private LevelManager LevelManager;

	void Start() {
		IntroText = GameObject.Find ("IntroText");
		Dimmer = GameObject.Find ("Dimmer");
		LevelManager = gameObject.GetComponent<LevelManager>();
	}

	void FixedUpdate () {

		if (!Loaded) {
			if (Input.GetButtonDown ("Jump")) {
				Debug.Log ("loaded!");
				Loaded = true;
				IntroText.SetActive(false);
				Dimmer.SetActive(false);
				LevelManager.LoadNextLevel();
			}
			return;
		}

		if (ControlsLocked) {
			return;
		}

		var hInput = Input.GetAxis ("Horizontal");
		var vInput = Input.GetAxis ("Vertical");

		rigidbody2D.velocity = new Vector2 (hInput * MoveSpeed, vInput * MoveSpeed);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Goal") {
			LevelManager.LoadNextLevel();
			transform.position = new Vector3(0, 0, transform.position.z);
		}
	}
}
