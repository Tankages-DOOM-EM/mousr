using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Collectable : ICollectable {
	public GameObject GameObject { get; private set; }
	public int Type { get; private set; }
	public float CollectableSize = 0.5f;

	private IRoom _containingRoom = null;

	public IRoom ContainingRoom { 
		get { return _containingRoom;}
		set {
			_containingRoom = value;
			GameObject.transform.parent = _containingRoom.GameObject.transform;
			GameObject.transform.localPosition = GetNextPosition(_containingRoom.ItemCount);
		}
	}

	public Collectable(int type, GameObject gameObject) {
		GameObject = gameObject;
		Type = type;
	}

	private Vector3 GetNextPosition(int numObjects) {
		var xOffset = (numObjects % 2) * CollectableSize;
		var yOffset = Mathf.Floor (numObjects / 2.0f) * CollectableSize;
		
		return new Vector3 (xOffset, yOffset, -1);
	}
}
