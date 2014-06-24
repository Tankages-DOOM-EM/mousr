using UnityEngine;
using System.Collections;

public class TopDownPlayerController : MonoBehaviour {
	public float MoveSpeed = 1.0f;
	public int WorldSize = 8;

	public bool ControlsLocked = false;
	
	public GameObject Camera;

	private LevelManager LevelManager;

	void Start() {
		LevelManager = gameObject.GetComponent<LevelManager>();
		Camera = GameObject.Find ("Camera");
	}

	public void SetPosition(Vector2 pos) {
		transform.position = new Vector3(pos.x, pos.y, transform.position.z);
		Camera.transform.position = new Vector3(pos.x, pos.y, Camera.transform.position.z);
	}

	void FixedUpdate () {
		if (ControlsLocked) {
			return;
		}

		var hInput = Input.GetAxis ("Horizontal");
		var vInput = Input.GetAxis ("Vertical");

		rigidbody2D.velocity = new Vector2 (hInput * MoveSpeed, vInput * MoveSpeed);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Goal") {
			LevelManager.LevelComplete ();
		}
	}
}
