using UnityEngine;
using System.Collections;

public class UnitToWorld : MonoBehaviour {
	public const float WorldCoordPerUnit = 5.125f;

	public static Vector3 Convert(int x, int y) {
		return new Vector3 (x * UnitToWorld.WorldCoordPerUnit, y * UnitToWorld.WorldCoordPerUnit * -1.0f, 0.0f);
	}
}
