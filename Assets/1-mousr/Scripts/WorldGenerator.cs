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
	public Mode CurrentMode = Mode.Newest;
	public GameObject GoalPrefab;
	public GameObject Goal = null;

	private Point2D MaxPoint;
	private Point2D MinPoint;

	public int[,] Grid = null;
	private IList<Point2D> Cells = new List<Point2D> ();
	private IList<GameObject> MazeObjects = new List<GameObject>();

	public GameObject AllWayRoom;
	public GameObject AllExceptSouthRoom;
	public GameObject AllExceptNorthRoom;
	public GameObject AllExceptWestRoom;
	public GameObject AllExceptEastRoom;
	public GameObject NorthAndEastRoom;
	public GameObject NorthAndWestRoom;
	public GameObject SouthAndEastRoom;
	public GameObject SouthAndWestRoom;
	public GameObject NorthSouthCorridor;
	public GameObject WestEastCorridor;
	public GameObject SouthDeadEnd;
	public GameObject NorthDeadEnd;
	public GameObject WestDeadEnd;
	public GameObject EastDeadEnd;

	/// <summary>
	/// Creates and initializes a [width] by [height] array which will be used to
	/// create the 
	/// </summary>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	private void InitializeGrid(int width, int height) {
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
	private int GetNextIndex(int max) {
		return  CurrentMode == Mode.Newest ? max - 1 : MazeRandom.Next(max);
	}

	/// <summary>
	/// Perfoms a single iteration on the maze creation algorithm.
	/// </summary>
	private void Iterate() {
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
	private void GenerateGrid() {
		while (Cells.Count > 0) {
			Iterate ();
		}
	}

	/// <summary>
	/// Creates a game object from exit definition.
	/// </summary>
	/// <returns>The new game object.</returns>
	/// <param name="exitDefinition">Integer describing all possible exits from a cell.</param>
	private GameObject CreateGameObjectFromExitDefinition(int exitDefinition) {

		switch (exitDefinition) {

		// all ways out
		case Direction.NorthSouthWestEast:
			return Instantiate (AllWayRoom) as GameObject;

		//3 ways out
		case Direction.NorthWestEast:
			return Instantiate (AllExceptSouthRoom) as GameObject;
	
		case Direction.NorthSouthWest:
			return Instantiate (AllExceptEastRoom) as GameObject;
	
		case Direction.SouthWestEast:
			return Instantiate (AllExceptNorthRoom) as GameObject;
	
		case Direction.NorthSouthEast:
			return Instantiate (AllExceptWestRoom) as GameObject;

		//2 ways out
		case Direction.NorthEast:
			return Instantiate (NorthAndEastRoom) as GameObject;
	
		case Direction.NorthWest:
			return Instantiate (NorthAndWestRoom) as GameObject;
	
		case Direction.SouthEast:
			return Instantiate (SouthAndEastRoom) as GameObject;
	
		case Direction.SouthWest:
			return Instantiate (SouthAndWestRoom) as GameObject;
	
		case Direction.NorthSouth:
			return Instantiate (NorthSouthCorridor) as GameObject;

		case Direction.WestEast:
			return Instantiate (WestEastCorridor) as GameObject;

		//1 way out
		case Direction.North:
			return Instantiate (SouthDeadEnd) as GameObject;

		case Direction.South:
			return Instantiate (NorthDeadEnd) as GameObject;

		case Direction.East:
			return Instantiate (WestDeadEnd) as GameObject;

		case Direction.West:
			return Instantiate (EastDeadEnd) as GameObject;

		default:
			throw new UnityException ("Unknown Direction in Grid: " + exitDefinition);
		}
	}

	public int GetRoom (int x, int y)
	{
		return Grid [x, y];
	}

	public void SetRoom(int x, int y, int newRoomDescription) {
		Grid [x, y] = newRoomDescription;
	}

	/// <summary>
	/// Iterates the Grid and creates game objects from the exit data for each cell.
	/// </summary>
	private void CreateGameObjects() {
		for (var x = 0; x < MaxPoint.X; ++x) {
			for (var y = 0; y < MaxPoint.Y; ++y) {
				var obj = CreateGameObjectFromExitDefinition (GetRoom (x, y));
				obj.transform.position = Convert.UnitToWorld (x, y);
				MazeObjects.Add(obj);
			}
		}
	}

	/// <summary>
	/// Destroy existing maze objects and clears the maze object array.
	/// </summary>
	private void ClearMazeObjects() {
		Destroy (Goal);
		foreach (var obj in MazeObjects) {
			Destroy (obj);
		}
		
		MazeObjects.Clear ();
	}

	/// <summary>
	/// Gets the dead ends.
	/// </summary>
	/// <returns>The dead ends.</returns>
	public IEnumerable<GameObject> GetDeadEnds() {
		return MazeObjects.Where (o => o.name.Contains ("DeadEnd"));
	}

	/// <summary>
	/// Gets the hallways.
	/// </summary>
	/// <returns>The hallways.</returns>
	public IEnumerable<GameObject> GetHallways() {
		return MazeObjects.Where (o => o.name.EndsWith ("Corridor"));
	}

	/// <summary>
	/// Gets the three exit rooms.
	/// </summary>
	/// <returns>The three exit rooms.</returns>
	public IEnumerable<GameObject> GetThreeExitRooms() {
		return MazeObjects.Where (o => o.name.StartsWith ("allExcept"));
	}

	/// <summary>
	/// Gets the corner rooms.
	/// </summary>
	/// <returns>The corner rooms.</returns>
	public IEnumerable<GameObject> GetCornerRooms() {
		return MazeObjects.Where (o => o.name.Contains ("And"));
	}

	/// <summary>
	/// Gets the four exit rooms.
	/// </summary>
	/// <returns>The four exit rooms.</returns>
	public IEnumerable<GameObject> GetFourExitRooms() {
		return MazeObjects.Where (o => o.name.Equals ("allWayRoom"));
	}

	/// <summary>
	/// Chooses the goal room.
	/// </summary>
	private void ChooseGoalRoom() {
		var deadEnds = GetDeadEnds()
			.OrderByDescending (o => o.transform.position.magnitude)
			.Take (3);
		var goalRoom = deadEnds.ElementAt (MazeRandom.Next (0, deadEnds.Count ()));
		Goal = Instantiate (GoalPrefab, goalRoom.transform.position, Quaternion.identity) as GameObject;
	}

	/// <summary>
	/// Generates the Maze. Public entry point for the maze generation.
	/// </summary>
	/// <param name="width">Width of maze in room count.</param>
	/// <param name="height">Height of maze in room count.</param>
	/// <param name="mode">Cell selection mode. See Mode documentation above.</param>
	public void GenerateWorld(int width = 5, int height = 5, Mode mode = Mode.Newest) {
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
