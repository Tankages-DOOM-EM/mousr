using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			other.gameObject.transform.position = new Vector3(0,0,other.gameObject.transform.position.z);
			WorldGenerator.GenerateWorld(10,10);
		}
	}
}

