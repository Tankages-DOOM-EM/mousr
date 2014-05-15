using UnityEngine;
using System.Collections;


public class WorldGenerator : MonoBehaviour {
	public enum Mode {Newest = 1, Middle, Oldest, Random};
	public static Mode CurrentMode = Mode.Newest;

	private static Point2D MaxPoint;
	private static Point2D MinPoint;

	private static int[,] Grid;
	private static ArrayList Cells = new ArrayList ();
	private static ArrayList MazeObjects = new ArrayList();

	private static MazeRandom Rand = new MazeRandom();

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

	private static GameObject GameObjectFromExitDefinition(int exitDefinition) {
		switch (exitDefinition) {

		// all ways
		case Direction.NorthSouthWestEast:
				return Instantiate (GameObject.Find ("allWayRoom")) as GameObject;

		//3 ways
		case Direction.NorthWestEast:
				return Instantiate (GameObject.Find ("allExceptSouthRoom")) as GameObject;
	
		case Direction.NorthSouthWest:
				return Instantiate (GameObject.Find ("allExceptEastRoom")) as GameObject;
	
		case Direction.SouthWestEast:
				return Instantiate (GameObject.Find ("allExceptNorthRoom")) as GameObject;
	
		case Direction.NorthSouthEast:
				return Instantiate (GameObject.Find ("allExceptWestRoom")) as GameObject;


		//2 ways
	
		case Direction.NorthEast:
				return Instantiate (GameObject.Find ("northAndEastRoom")) as GameObject;
	
		case Direction.NorthWest:
				return Instantiate (GameObject.Find ("northAndWestRoom")) as GameObject;
	
		case Direction.SouthEast:
				return Instantiate (GameObject.Find ("southAndEastRoom")) as GameObject;
	
		case Direction.SouthWest:
				return Instantiate (GameObject.Find ("southAndWestRoom")) as GameObject;
	
		case Direction.NorthSouth:
				return Instantiate (GameObject.Find ("northSouthCorridor")) as GameObject;

		case Direction.WestEast:
				return Instantiate (GameObject.Find ("westEastCorridor")) as GameObject;

		//1 way
		case Direction.NorthSouthWestEast:
				return Instantiate (GameObject.Find ("southDeadEnd")) as GameObject;
	
		case Direction.NorthSouthWestEast:
				return Instantiate (GameObject.Find ("northDeadEnd")) as GameObject;
	
		case Direction.NorthSouthWestEast:
				return Instantiate (GameObject.Find ("westDeadEnd")) as GameObject;
	
		case Direction.NorthSouthWestEast:
				return Instantiate (GameObject.Find ("eastDeadEnd")) as GameObject;

		default:
				throw new UnityException ("Unknown Direction in Grid");
		}
	}

	private static void CreateGameObjects() {
		for (var x = 0; x < MaxPoint.X; ++x) {
			for (var y = 0; y < MaxPoint.Y; ++y) {
				var obj = GameObjectFromExitDefinition(Grid[x,y]);
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
		foreach (var obj in MazeObjects) {
			Destroy (obj);
		}
		MazeObjects.Clear ();
		Cells.Clear ();
		Cells.Add (new Point2D (Rand.Next (width), Rand.Next (height)));

		GenerateGrid ();

		CreateGameObjects ();
	}
}
