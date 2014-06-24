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
	
	public void Destroy() {
		GameObject.Destroy (GameObject);
	}

	public static float GetHelpTipSize(int type) {
		switch (type) {
		case CollectableConstants.CoinId:
			return 4.0f;
		case CollectableConstants.TimeBoostId:
			return 7.05f;
		case CollectableConstants.BlueSwitchId:
			return 7.05f;
		case CollectableConstants.GoalId:
			return 7.05f;
		default:
			return 1.0f;
		}
	}

	public static Vector3 GetHelpTipOffset(int type) {
		switch (type) {
		case CollectableConstants.CoinId:
		case CollectableConstants.TimeBoostId:
		case CollectableConstants.BlueSwitchId:
			return new Vector3(0, 0.25f, 0);
		case CollectableConstants.GoalId:
			return new Vector3(0, 0.85f, 0);
		default:
			return Vector3.zero;
		}
	}
	
	public static string GetHelpTipTag(int type) {
		switch (type) {
		case CollectableConstants.CoinId:
			return "Coin";
		case CollectableConstants.TimeBoostId:
			return "TimeBoost";
		case CollectableConstants.BlueSwitchId:
			return "BlueSwitch";
		case CollectableConstants.GoalId:
			return "Goal";
		default:
			return "";
		}
	}

	public static string GetHelpTipText(int type) {
		switch (type) {
		case CollectableConstants.CoinId:
			return CollectableConstants.CoinTip;
		case CollectableConstants.TimeBoostId:
			return CollectableConstants.TimeTip;
		case CollectableConstants.BlueSwitchId:
			return CollectableConstants.SwitchTip;
		case CollectableConstants.GoalId:
			return CollectableConstants.GoalTip;
		default:
			return "Missing message!";
		}
	}

	private Vector3 GetNextPosition(int numObjects) {
		var xOffset = (numObjects % 2) * CollectableSize;
		var yOffset = Mathf.Floor (numObjects / 2.0f) * CollectableSize;
		
		return new Vector3 (xOffset, yOffset, -1);
	}
}
