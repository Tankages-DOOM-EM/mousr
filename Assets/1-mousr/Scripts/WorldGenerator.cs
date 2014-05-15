using UnityEngine;
using System.Collections;

public class Point2D {
	public int X, Y;

	public Point2D() : this(0,0) {}
	public Point2D(int x, int y) {
		X = x;
		Y = y;
	}

	public Point2D(ref Point2D other): this(other.X, other.Y) {}
	
	public static Point2D operator+(Point2D lhs, Point2D rhs) {
		return new Point2D(lhs.X + rhs.X, lhs.Y + rhs.Y);
	}
	
	public static Point2D operator-(Point2D lhs, Point2D rhs) {
		return new Point2D(lhs.X - rhs.X, lhs.Y - rhs.Y);
	}
	
	public static bool operator<=(Point2D lhs, Point2D rhs) {
		return lhs.X <= rhs.X && lhs.Y <= rhs.Y;
	}
	
	public static bool operator>=(Point2D lhs, Point2D rhs) {
		return lhs.X >= rhs.X && lhs.Y >= rhs.Y;
	}
	
	public static bool operator<(Point2D lhs, Point2D rhs) {
		return lhs.X < rhs.X && lhs.Y < rhs.Y;
	}
	
	public static bool operator>(Point2D lhs, Point2D rhs) {
		return lhs.X > rhs.X && lhs.Y > rhs.Y;
	}
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

	public static int[] Shuffled = null;

	public static void Shuffle() {
		Shuffled = Shuffled != null ? Shuffled : new int[4]{North, East,South, West};
		Shuffled [0] = North;
		Shuffled [1] = East;
		Shuffled [2] = South;
		Shuffled [3] = West;

		for (var i = 0; i < 4; ++i) {
			var i2 = Random.Range (i, 4);
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
			return North;
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
		default:
			return new Point2D(0, -1);
		}
	}
}
public class WorldGenerator : MonoBehaviour {
	public enum Mode {Newest = 1, Middle, Oldest, Random};

	private const int Seed = 129834756;
	private const int MinSize = 4;

	private static Point2D MaxPoint;
	private static Point2D MinPoint;

	private static int[,] Grid;
	private static ArrayList Cells = new ArrayList ();
	private static ArrayList MazeObjects = new ArrayList();
	private static System.Random Rand = new System.Random (Seed);
	private static Mode CurrentMode = Mode.Newest;

	private static void CreateGrid(int width, int height) {
		Grid = new int[width,height];

		for (int x = 0; x < width; ++x) {
			for(int y = 0; y < height; ++y) {
				Grid[x, y] = 0;
			}
		}
	}

	private static int GetNextIndex(int max) {
		switch (CurrentMode) {
		case Mode.Newest: return max - 1;
		case Mode.Middle: return max / 2;
		case Mode.Oldest: return 0;
		default: return Rand.Next(max);
		}
	}
	private static void Iterate() {
		var index = GetNextIndex(Cells.Count);
		var cell = Cells[index] as Point2D;

		Direction.Shuffle ();
		foreach (var dir in Direction.Shuffled) {
			var pt = cell + Direction.Delta (dir);
			if (pt >= MinPoint && pt < MaxPoint && Grid[pt.X, pt.Y] == 0) {
				Grid[cell.X, cell.Y] |= dir;
				Grid[pt.X, pt.Y] |= Direction.Opposite(dir);
				Cells.Add(pt);
				index = -1;
				break;
			}
		}

		if (index >= 0) {
			Cells.RemoveAt (index);
		}
	}

	private static void GenerateGrid() {
		while (Cells.Count > 0) {
			Iterate ();
		}
	}

	private static GameObject GameObjectFromGrid(int grid) {
		// all ways
		if (grid == (Direction.NorthWest | Direction.SouthEast)) {
			return Instantiate(GameObject.Find("allWayRoom")) as GameObject;
		}

		//3 ways
		if (grid == (Direction.NorthWest | Direction.East)) {
			return Instantiate(GameObject.Find("allExceptSouthRoom")) as GameObject;
		}
		if (grid == (Direction.NorthWest | Direction.South)) {
			return Instantiate(GameObject.Find("allExceptEastRoom")) as GameObject;
		}
		if (grid == (Direction.SouthEast | Direction.West)) {
			return Instantiate(GameObject.Find("allExceptNorthRoom")) as GameObject;
		}
		if (grid == (Direction.SouthEast | Direction.North)) {
			return Instantiate(GameObject.Find("allExceptWestRoom")) as GameObject;
		}

		//2 ways
		if (grid == Direction.NorthEast) {
			return Instantiate(GameObject.Find("northAndEastRoom")) as GameObject;
		}
		if (grid == Direction.NorthWest) {
			return Instantiate(GameObject.Find("northAndWestRoom")) as GameObject;
		}
		if (grid == Direction.SouthEast) {
			return Instantiate(GameObject.Find("southAndEastRoom")) as GameObject;
		}
		if (grid == Direction.SouthWest) {
			return Instantiate(GameObject.Find("southAndWestRoom")) as GameObject;
		}
		if (grid == (Direction.North | Direction.South)) {
			return Instantiate(GameObject.Find("northSouthCorridor")) as GameObject;
		}
		if (grid == (Direction.East | Direction.West)) {
			return Instantiate(GameObject.Find("westEastCorridor")) as GameObject;
		}

		//1 way
		
		if ((grid & Direction.North) == Direction.North) {
			return Instantiate(GameObject.Find("southDeadEnd")) as GameObject;
		}
		if ((grid & Direction.South) == Direction.South) {
			return Instantiate(GameObject.Find("northDeadEnd")) as GameObject;
		}
		if ((grid & Direction.East) == Direction.East) {
			return Instantiate(GameObject.Find("westDeadEnd")) as GameObject;
		}
		return Instantiate(GameObject.Find("eastDeadEnd")) as GameObject;
	}

	private static void CreateGameObjects() {

		for (var x = 0; x < MaxPoint.X; ++x) {
			for (var y = 0; y < MaxPoint.Y; ++y) {
				var obj = GameObjectFromGrid(Grid[x,y]);
				obj.transform.position = UnitToWorld.Convert (x, y);
				MazeObjects.Add(obj);
			}
		}
	}

	public static void GenerateWorld(int width = 5, int height = 5, Mode mode = Mode.Newest) {
		MaxPoint = new Point2D (width, height);
		MinPoint = new Point2D ();
		CurrentMode = mode;
		CreateGrid (width, height);
		MazeObjects.Clear ();
		Cells.Clear ();
		Cells.Add (new Point2D (Rand.Next (width), Rand.Next (height)));

		GenerateGrid ();

		CreateGameObjects ();
	}
}
