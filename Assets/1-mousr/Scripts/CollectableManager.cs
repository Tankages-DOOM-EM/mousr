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

		return Convert.UnitToWorld(x, y) + new Vector3 (xOffset, yOffset);
	}

	private GameObject CreateCoin(int x, int y, int room) {
		WorldGenerator.SetRoom (x, y, room | CollectableConstants.CoinId);
		var position = GetNextPosition (x, y, room.RoomItemCount ());
		return Instantiate (CoinPrefab, position, Quaternion.identity) as GameObject;
	}

	private GameObject CreateTimeBoost(int x, int y, int room) {
		WorldGenerator.SetRoom (x, y, room | CollectableConstants.TimeBoostId);
		var position = GetNextPosition (x, y, room.RoomItemCount ());
		return Instantiate (TimeBoostPrefab, position, Quaternion.identity) as GameObject;
	}

	public GameObject SpawnCollectable(int x, int y, int type) {
		var room = WorldGenerator.GetRoom (x, y);

		return (type == CollectableConstants.CoinId) 
			? CreateCoin (x, y, room)
			: CreateTimeBoost (x, y, room);
	}

	public GameObject SpawnCollectable(Point2D pos, int type) {
		return SpawnCollectable(pos.X, pos.Y, type);
	}
}

