using UnityEngine;
using System.Collections;

public class Convert : MonoBehaviour {
	public const float WorldCoordPerUnit = 5.125f;
	
	public static Vector3 UnitToWorld(int x, int y) {
		return new Vector3 (x * Convert.WorldCoordPerUnit, y * Convert.WorldCoordPerUnit * -1.0f, 0.0f);
	}

	public static Vector3 UnitToWorld(Point2D p) {
		return UnitToWorld(p.X, p.Y);
	}
	
	public static Point2D WorldToUnit(float x, float y) {
		return new Point2D ((int)System.Math.Floor(x / Convert.WorldCoordPerUnit), (int)System.Math.Floor(y / Convert.WorldCoordPerUnit * -1.0f));
	}
	
	public static Point2D WorldToUnit(Vector3 pos) {
		return WorldToUnit(pos.x, pos.y);
	}
}
