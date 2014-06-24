using UnityEngine;
using System.Collections;

public class DirectionException : System.Exception {
	public DirectionException(string message) : base(message) {}
}

public class Direction {
	public const int North = 1;
	public const int South = 1 << 1;
	public const int East = 1 << 2;
	public const int West = 1 << 3;
	public const int NorthWest = North | West;
	public const int NorthEast = North | East;
	public const int SouthWest = South | West;
	public const int SouthEast = South | East;
	public const int NorthSouth = North | South;
	public const int WestEast = West | East;
	public const int NorthSouthEast = NorthEast | South;
	public const int NorthSouthWest = NorthWest | South;
	public const int NorthWestEast = NorthWest | East;
	public const int SouthWestEast = SouthWest | East;
	public const int NorthSouthWestEast = NorthSouth | WestEast;

	public static int[] Shuffled = null;

	public static void Shuffle() {
		Shuffled = Shuffled != null ? Shuffled : new int[4];
		Shuffled [0] = North;
		Shuffled [1] = East;
		Shuffled [2] = South;
		Shuffled [3] = West;

		for (var i = 0; i < 4; ++i) {
			var i2 = MazeRandom.Next (i, 4);
			var tmp = Shuffled[i];
			Shuffled[i] = Shuffled[i2];
			Shuffled[i2] = tmp;
		}
	}

	public static int Opposite(int direction) {
		switch (direction) {
		case North:
			return South;
		case South:
			return North;
		case East:
			return West;
		case West:
			return East;
		default:
			throw new DirectionException("Delta: Unknown Direction provided " + direction.ToString());
		}
	}

	public static Point2D Delta(int direction) {
		switch (direction) {
		case East:
			return new Point2D(1, 0);
		case West:
			return new Point2D(-1, 0);
		case South:
			return new Point2D(0, 1);
		case North:
			return new Point2D(0, -1);
		default:
			throw new DirectionException("Delta: Unknown Direction provided " + direction.ToString());
		}
	}
}
