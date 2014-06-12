using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public interface IMazeObject {
	GameObject GameObject { get; }
}

public class MazeObject: IMazeObject {
	public GameObject GameObject { get; private set; }

	public MazeObject(GameObject gameObject) {
		GameObject = gameObject;
	}
}
