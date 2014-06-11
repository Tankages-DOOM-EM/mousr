using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CollectableManager : MonoBehaviour
{
	public WorldGenerator WorldGenerator;
	public GameObject CoinPrefab;
	public GameObject TimeBoostPrefab;
	public float CollectableSize;
	
	// Use this for initialization
	void Start ()
	{
		WorldGenerator = gameObject.GetComponent<WorldGenerator> ();
	}

	public Vector3 GetNextPosition(int x, int y, int numObjects) {
		var xOffset = (numObjects % 2) * CollectableSize;
		var yOffset = Mathf.Floor (numObjects / 2.0f) * CollectableSize;

		return new Vector3 (xOffset, yOffset);// + Convert.UnitToWorld(x, y);
	}

	private GameObject CreateCoin() {
		return Instantiate (CoinPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
	}

	private GameObject CreateTimeBoost() {
		return Instantiate (TimeBoostPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
	}

	public ICollectable SpawnCollectable(int type) {																				
		switch (type) {
		case CollectableConstants.CoinId:
			return new Collectable (type, CreateCoin ());
		case CollectableConstants.TimeBoostId:
			return new Collectable (type, CreateTimeBoost());
		default:
			return null;
		}
	}
}

