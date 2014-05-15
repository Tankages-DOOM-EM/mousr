using UnityEngine;
using System.Collections;

public class TopDownPlayerController : MonoBehaviour {
	public float MoveSpeed = 1.0f;
	private bool Loaded = false;
	void FixedUpdate () {
		var h = Input.GetAxis ("Horizontal");
		var v = Input.GetAxis ("Vertical");
		if (Input.GetButtonDown ("Jump") && !Loaded) {
			Loaded = true;
			WorldGenerator.GenerateWorld(8,8);
		}
		rigidbody2D.velocity = new Vector2 (h * MoveSpeed, v * MoveSpeed);
	}
}
