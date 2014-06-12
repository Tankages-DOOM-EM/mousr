using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Room : IRoom {
	public int DescribePosition(int x, int y) {
		var pos = 0;
		pos |= System.Math.Min (31, System.Math.Max (0, x)) << DescriptionMasks.XShift;
		pos |= System.Math.Min (31, System.Math.Max (0, y)) << DescriptionMasks.YShift;
		return pos;
	}
	
	// room game object
	public GameObject GameObject { get; private set; }
	
	// Description format is as follows:
	// dddd xxxxx yyyyy rr cc tt
	// dir - 4bits
	// x grid pos - 5bits -- max: 31
	// y grid pos - 5bits -- max: 31
	// rotation   - 2bits -- max: 3   0- none 1- 90'CW 2- 180'CW 3- 270'CW
	// coin collectables (count) - 2bits -- max 3
	// time boost collectables (count) - 2bits -- max 3
	//
	// TOTAL BITS IN USE: 20bits
	public int Description { get; private set; }

	public List<IMazeObject> Children;

	public Room(GameObject prefab, int x, int y, int direction) {
		Children = new List<IMazeObject> ();
		GameObject = prefab;
		Description = direction;
		Description |= DescribePosition (x, y);
	}

	public void Destroy() {
		foreach(var child in Children) {
			GameObject.Destroy(child.GameObject);
		}
		Children.Clear ();
		GameObject.Destroy (GameObject);
	}

	public Point2D GetPosition() {
		var x = (Description & DescriptionMasks.PositionX) >> DescriptionMasks.XShift;
		var y = (Description & DescriptionMasks.PositionY) >> DescriptionMasks.YShift;
		return new Point2D (x, y);
	}

	public int CoinCount {
		get { return (Description & DescriptionMasks.Coin) >> DescriptionMasks.CoinShift; }
	}

	private void IncrementCoins() {
		var newCount = CoinCount + 1;
		SetCoinCount (newCount);
	}
	
	private void DecrementCoins() {
		var newCount = CoinCount - 1;
		SetCoinCount (newCount);
	}

	private void SetCoinCount(int newCount) {
		if (newCount > (DescriptionMasks.Coin >> DescriptionMasks.CoinShift)) {
			Debug.LogWarning("Tried to set too many coins");
			return;
		}
		else if (newCount < 0) {
			Debug.LogWarning("Tried to set negative coins");
			return;
		}
		Description += ((newCount) << DescriptionMasks.CoinShift) - (Description & DescriptionMasks.Coin);
	}

	public int TimeBoostCount {
		get { return (Description & DescriptionMasks.TimeBoost) >> DescriptionMasks.TBShift; }
	}

	public int ItemCount {
		get { return TimeBoostCount + CoinCount; }
	}

	private void IncrementTimeBoosts() {
		var newCount = TimeBoostCount + 1;
		SetTimeBoostCount (newCount);
	}
	
	private void DecrementTimeBoosts() {
		var newCount = TimeBoostCount - 1;
		SetTimeBoostCount (newCount);
	}
	
	private void SetTimeBoostCount(int newCount) {
		if (newCount > (DescriptionMasks.TimeBoost >> DescriptionMasks.TBShift)) {
			Debug.LogWarning("Tried to set too many time boosts");
			return;
		}
		else if (newCount < 0) {
			Debug.LogWarning("Tried to set negative time boosts");
			return;
		}
		Description += ((newCount) << DescriptionMasks.TBShift) - (Description & DescriptionMasks.TimeBoost);
	}

	public void AddCollectable(ICollectable collectable) {
		collectable.ContainingRoom = this;
		switch (collectable.Type) {
		case CollectableConstants.CoinId:
			IncrementCoins();
			break;
		case CollectableConstants.TimeBoostId:
			IncrementTimeBoosts();
			break;
		}
	}

	public void AddChildObject(IMazeObject child) {
		child.GameObject.transform.parent = GameObject.transform;
		child.GameObject.transform.localPosition = Vector3.zero;
		Children.Add (child);
	}
}
