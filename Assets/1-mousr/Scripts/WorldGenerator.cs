using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldGenerator : MonoBehaviour {
	/// <summary>
	/// New cell selection mode.
	/// Each mode changes the way the algorithm behaves.
	/// 
	/// Newest - Mimics the Recursive Backtracking algorithm
	/// Random - Mimics Prim's Algorithm
	/// </summary>
	public enum Mode {Newest = 1, Random};
	public static Mode CurrentMode = Mode.Newest;
	public static GameObject Goal = null;

	private static Point2D MaxPoint;
	private static Point2D MinPoint;

	private static int[,] Grid = null;
	private static IList<Point2D> Cells = new List<Point2D> ();
	private static IList<GameObject> MazeObjects = new List<GameObject>();

	/// <summary>
	/// Creates and initializes a [width] by [height] array which will be used to
	/// create the 
	/// </summary>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	private static void InitializeGrid(int width, int height) {
		Grid = new int[width,height];

		for (int x = 0; x < width; ++x) {
			for(int y = 0; y < height; ++y) {
				Grid[x, y] = 0;
			}
		}
	}

	/// <summary>
	/// Gets the index of the next cell to use based on the current selection mode.
	/// </summary>
	/// <returns>The next index.</returns>
	/// <param name="max">current length of the Cell list.</param>
	private static int GetNextIndex(int max) {
		return  CurrentMode == Mode.Newest ? max - 1 : MazeRandom.Next(max);
	}

	/// <summary>
	/// Perfoms a single iteration on the maze creation algorithm.
	/// </summary>
	private static void Iterate() {
		// get the index for the next cell
		var index = GetNextIndex(Cells.Count);
		var cell = Cells[index];

		// randomize direction selection
		Direction.Shuffle ();

		foreach (var dir in Direction.Shuffled) {

			// select next randomized direction and get new cell by applying direction to current cell
			var newCell = cell + Direction.Delta (dir);

			if (newCell >= MinPoint && newCell < MaxPoint && Grid[newCell.X, newCell.Y] == 0) {
				// mark passage from current cell to new cell
				Grid[cell.X, cell.Y] |= dir;

				// mark passage from new cell to current cell
				Grid[newCell.X, newCell.Y] |= Direction.Opposite(dir);
				Cells.Add(newCell);

				// do not remove this cell yet, break out of loop.
				index = -1;
				break;
			}
		}

		// if no new cells processed, remove the current cel from the list
		if (index >= 0) {
			Cells.RemoveAt (index);
		}
	}

	/// <summary>
	/// Generates the maze grid. Grid is a 2 dimensional int array which describes
	/// all possible exits from a given cell. The maze is generated using the Growing Tree
	/// algorithm. The default mode for this algorithm is Newest which mimics the behavior
	/// of the Recursive Backtracking algorithm.
	/// </summary>
	private static void GenerateGrid() {
		while (Cells.Count > 0) {
			Iterate ();
		}
	}

	/// <summary>
	/// Creates a game object from exit definition.
	/// </summary>
	/// <returns>The new game object.</returns>
	/// <param name="exitDefinition">Integer describing all possible exits from a cell.</param>
	private static GameObject CreateGameObjectFromExitDefinition(int exitDefinition) {
		switch (exitDefinition) {

		// all ways out
		case Direction.NorthSouthWestEast:
			return Instantiate (GameObject.Find ("allWayRoom")) as GameObject;

		//3 ways out
		case Direction.NorthWestEast:
			return Instantiate (GameObject.Find ("allExceptSouthRoom")) as GameObject;
	
		case Direction.NorthSouthWest:
			return Instantiate (GameObject.Find ("allExceptEastRoom")) as GameObject;
	
		case Direction.SouthWestEast:
			return Instantiate (GameObject.Find ("allExceptNorthRoom")) as GameObject;
	
		case Direction.NorthSouthEast:
			return Instantiate (GameObject.Find ("allExceptWestRoom")) as GameObject;

		//2 ways out
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

		//1 way out
		case Direction.North:
			return Instantiate (GameObject.Find ("southDeadEnd")) as GameObject;

		case Direction.South:
			return Instantiate (GameObject.Find ("northDeadEnd")) as GameObject;

		case Direction.East:
			return Instantiate (GameObject.Find ("westDeadEnd")) as GameObject;

		case Direction.West:
			return Instantiate (GameObject.Find ("eastDeadEnd")) as GameObject;

		default:
			throw new UnityException ("Unknown Direction in Grid: " + exitDefinition);
		}
	}

	/// <summary>
	/// Iterates the Grid and creates game objects from the exit data for each cell.
	/// </summary>
	private static void CreateGameObjects() {
		for (var x = 0; x < MaxPoint.X; ++x) {
			for (var y = 0; y < MaxPoint.Y; ++y) {
				var obj = CreateGameObjectFromExitDefinition(Grid[x,y]);
				obj.transform.position = Convert.UnitToWorld (x, y);
				MazeObjects.Add(obj);
			}
		}
	}

	/// <summary>
	/// Destroy existing maze objects and clears the maze object array.
	/// </summary>
	private static void ClearMazeObjects() {
		Destroy (Goal);
		foreach (var obj in MazeObjects) {
			Destroy (obj);
		}
		
		MazeObjects.Clear ();
	}

	public static IEnumerable<GameObject> GetDeadEnds() {
		return MazeObjects.Where (o => o.name.Contains ("DeadEnd"));
	}
	
	public static IEnumerable<GameObject> GetHallways() {
		return MazeObjects.Where (o => o.name.EndsWith ("Corridor"));
	}
	
	public static IEnumerable<GameObject> GetThreeExitRooms() {
		return MazeObjects.Where (o => o.name.StartsWith ("allExcept"));
	}
	
	public static IEnumerable<GameObject> GetCornerRooms() {
		return MazeObjects.Where (o => o.name.Contains ("And"));
	}
	
	public static IEnumerable<GameObject> GetFourExitRooms() {
		return MazeObjects.Where (o => o.name.Equals ("allWayRoom"));
	}

	private static void ChooseGoalRoom() {
		var deadEnds = GetDeadEnds()
			.OrderByDescending (o => o.transform.position.magnitude)
			.Take (3);
		var goalRoom = deadEnds.ElementAt (MazeRandom.Next (0, deadEnds.Count ()));
		Goal = Instantiate (GameObject.FindGameObjectWithTag ("Goal"), goalRoom.transform.position, Quaternion.identity) as GameObject;
	}

	/// <summary>
	/// Generates the Maze. Public entry point for the maze generation.
	/// </summary>
	/// <param name="width">Width of maze in room count.</param>
	/// <param name="height">Height of maze in room count.</param>
	/// <param name="mode">Cell selection mode. See Mode documentation above.</param>
	public static void GenerateWorld(int width = 5, int height = 5, Mode mode = Mode.Newest) {
		MaxPoint = new Point2D (width, height);
		MinPoint = new Point2D (); // (0,0)

		CurrentMode = mode;

		InitializeGrid (width, height);

		ClearMazeObjects ();

		Cells.Clear ();

		Cells.Add (new Point2D (MazeRandom.Next (width), MazeRandom.Next (height)));

		GenerateGrid ();

		CreateGameObjects ();

		ChooseGoalRoom ();
	}
}
