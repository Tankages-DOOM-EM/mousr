using UnityEngine;
using System.Collections;

public class TopDownPlayerController : MonoBehaviour {
	public float MoveSpeed = 1.0f;
	public int WorldSize = 8;

	private bool Loaded = false;
	public bool ControlsLocked = false;
	private GameObject Dimmer;
	private LevelManager LevelManager;
	private GameObject Camera;
	private Timer Timer;
	private GameObject TitleText;
	private GameObject StartMessage;

	void Start() {
		TitleText = GameObject.Find ("TitleText");
		StartMessage = GameObject.Find ("StartMessage");
		Dimmer = GameObject.Find ("Dimmer");
		LevelManager = gameObject.GetComponent<LevelManager>();
		Camera = GameObject.Find ("Camera");
		Timer = GameObject.Find ("Timer").GetComponent<Timer> ();
	}

	void FixedUpdate () {
		if (!Loaded) {
			if (Input.GetButton ("Jump")) {
				Debug.Log ("loaded!");
				Loaded = true;
				TitleText.SetActive(false);
				StartMessage.SetActive(false);
				Dimmer.SetActive(false);
				LevelManager.LoadNextLevel();
				transform.position = new Vector3(0, 0, transform.position.z);
				Camera.transform.position = new Vector3(0, 0, Camera.transform.position.z);
				Timer.ResetTimer();
				Timer.StartTimer();
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
			Timer.StopTimer ();
			Dimmer.SetActive (true);
			TitleText.guiText.text = "Level " + (LevelManager.CurrentLevel + 1).ToString("D2");
			TitleText.SetActive(true);
			StartMessage.SetActive(true);
			Debug.Log ("Goal Collision");
			Loaded = false;
		}
	}
}
