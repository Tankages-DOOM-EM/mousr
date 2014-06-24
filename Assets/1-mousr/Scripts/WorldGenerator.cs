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
	public Point2D GoalPos;
	public Vector3 RotationAxis = new Vector3 (0, 0, -1);

	private Point2D MaxPoint;
	private Point2D MinPoint;

	public int[,] Grid = null;
	private IList<Point2D> Cells = new List<Point2D> ();
	private IDictionary<int,IRoom> Rooms = new Dictionary<int,IRoom>();

	// prototypes:
	
	// dead end
	//
	// +--+
	// |  |
	// +  +
	public GameObject DeadEndPrefab;
	
	// hallway
	//
	// +  +
	// |  |
	// +  +
	public GameObject HallwayPrefab;
	
	// corner
	//
	// +  +
	// |
	// +--+
	public GameObject CornerPrefab;
	
	// three way
	//
	// +  +
	// 
	// +--+
	public GameObject ThreeWayPrefab;
	
	// all way
	//
	// +  +
	//
	// +  +
	public GameObject AllWayPrefab;

	public GameObject CoinPrefab;
	public GameObject TimeBoostPrefab;
	public GameObject BlueSwitchPrefab;
	public GameObject BlueDoorPrefab;
	public GameObject HelpTipPrefab;

	private IRoom GoalRoom;

	void Start() {
		if (Direction.SouthEast.HasExit (Direction.North)) {
			Debug.LogError("South East should not have North!");
		}
		if (Direction.SouthEast.HasExit (Direction.NorthSouth)) {
			Debug.LogError("South East should not have NorthSouth!");
		}
		if (Direction.SouthEast.HasExit (Direction.WestEast)) {
			Debug.LogError("South East should not have WestEast!");
		}
	}

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

		Cells.Clear ();

		//seed algorithm
		Cells.Add (MazeRandom.Next (MaxPoint));

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
		var exitCount = exitDefinition.ExitCount ();

		switch (exitCount) {
		case 1:
			return Instantiate(DeadEndPrefab) as GameObject;

		case 2:
			if(exitDefinition.HasExit(Direction.NorthSouth) || exitDefinition.HasExit (Direction.WestEast)) {
				return Instantiate(HallwayPrefab) as GameObject;
			}
			return Instantiate(CornerPrefab) as GameObject;

		case 3:
			return Instantiate(ThreeWayPrefab) as GameObject;

		case 4:
			return Instantiate(AllWayPrefab) as GameObject;

		default:
			throw new UnityException ("Unknown Direction in Grid: " + exitDefinition);
		}
	}

	public int GetRoomDescription (int x, int y)
	{
		return Grid [x, y];
	}

	public void SetRoom(int x, int y, int newRoomDescription) {
		Grid [x, y] = newRoomDescription;
	}

	private Quaternion GetOneExitOrientation(int directions) {

		if (directions.HasExit(Direction.South)) {
			return Quaternion.identity;
		}
		if (directions.HasExit(Direction.West)) {
			return Quaternion.AngleAxis(90, RotationAxis);
		}
		if (directions.HasExit(Direction.North)) {
			return Quaternion.AngleAxis(180, RotationAxis);
		}
		
		return Quaternion.AngleAxis(270, RotationAxis);
	}
	
	private Quaternion GetTwoExitOrientation(int directions) {

		// hallways
		if (directions.HasExit (Direction.NorthSouth)) {
			return Quaternion.identity;
		} else if (directions.HasExit (Direction.WestEast)) {
			return Quaternion.AngleAxis (90, RotationAxis);
		}

		// corners
		if (directions.HasExit (Direction.NorthEast)) {
			return Quaternion.identity;
		} else if (directions.HasExit (Direction.SouthEast)) {
			return Quaternion.AngleAxis (90, RotationAxis);
		} else if (directions.HasExit (Direction.SouthWest)) {
			return Quaternion.AngleAxis (180, RotationAxis);
		} else { //Direction.NorthWest
			return Quaternion.AngleAxis (270, RotationAxis);
		}
	}
	
	private Quaternion GetThreeExitOrientation(int directions) {

		if (!directions.HasExit(Direction.South)) {
			return Quaternion.identity;
		}
		if (!directions.HasExit(Direction.West)) {
			return Quaternion.AngleAxis(90, RotationAxis);
		}
		if (!directions.HasExit (Direction.North)) {
			return Quaternion.AngleAxis(180, RotationAxis);
		}
		
		return Quaternion.AngleAxis(270, RotationAxis);
	}

	private Quaternion RotationFromDirection(int roomDescription) {
		int directions = roomDescription & DescriptionMasks.Direction;
		var rotation = Quaternion.identity;

		switch (directions.SumBits32 ()) {
		case 1:
			rotation = GetOneExitOrientation (directions);
			break;
		case 2:
			rotation = GetTwoExitOrientation (directions);
			break;
		case 3:
			rotation = GetThreeExitOrientation (directions);
			break;
		}

		return rotation;
	}

	/// <summary>
	/// Iterates the Grid and creates game objects from the exit data for each cell.
	/// </summary>
	private void CreateRoomGameObjects() {
		for (var x = 0; x < MaxPoint.X; ++x) {
			for (var y = 0; y < MaxPoint.Y; ++y) {
				var roomDescription = GetRoomDescription (x, y);
				var obj = CreateGameObjectFromExitDefinition (roomDescription);
				roomDescription |= RoomId (x,y);
				var room = new Room(obj, x, y, roomDescription);
				obj.transform.position = Convert.UnitToWorld (x, y);
				obj.transform.rotation = RotationFromDirection(roomDescription);
				AddRoom(x,y,room);
			}
		}
	}

	/// <summary>
	/// Destroy existing maze objects and clears the maze object array.
	/// </summary>
	private void ClearMazeObjects() {
		Rooms.Select(p => p.Value).ToList ().ForEach(r => r.Destroy ());
		
		Rooms.Clear ();
	}

	/// <summary>
	/// Gets the dead ends.
	/// </summary>
	/// <returns>The dead ends.</returns>
	public IEnumerable<IRoom> GetDeadEnds() {
		return Rooms.Select (kvp => kvp.Value).Where (o => o.Description.IsDeadEnd ());//o.GameObject.name.Equals("DeadEndRoom"));
	}

	/// <summary>
	/// Gets the hallways.
	/// </summary>
	/// <returns>The hallways.</returns>
	public IEnumerable<IRoom> GetHallways() {
		return Rooms.Select (kvp => kvp.Value).Where (o => o.Description.IsHallway());
	}

	/// <summary>
	/// Gets the three exit rooms.
	/// </summary>
	/// <returns>The three exit rooms.</returns>
	public IEnumerable<IRoom> GetThreeExitRooms() {
		return Rooms.Select (kvp => kvp.Value).Where (o => o.Description.ExitCount () == 3);
	}

	/// <summary>
	/// Gets the corner rooms.
	/// </summary>
	/// <returns>The corner rooms.</returns>
	public IEnumerable<IRoom> GetCornerRooms() {
		return Rooms.Select (kvp => kvp.Value).Where (o => o.Description.IsCorner());
	}

	/// <summary>
	/// Gets the four exit rooms.
	/// </summary>
	/// <returns>The four exit rooms.</returns>
	public IEnumerable<IRoom> GetFourExitRooms() {
		return Rooms.Select (kvp => kvp.Value).Where (o => o.Description.ExitCount () == 4);
	}

	private IEnumerable<IRoom> GetPossibleGoals() {
		var rooms = GetDeadEnds ();
		if (rooms.Count () > 0) {
			return rooms;
		}

		rooms = GetCornerRooms ();
		if (rooms.Count () > 0) {
			return rooms;
		}

		rooms = GetThreeExitRooms ();
		if (rooms.Count () > 0) {
			return rooms;
		}
		
		rooms = GetHallways ();
		if (rooms.Count () > 0) {
			return rooms;
		}

		return GetFourExitRooms ();
	}

	/// <summary>
	/// Chooses the goal room.
	/// </summary>
	private void CreateGoal(bool showTip) {
		var possibleGoals = GetPossibleGoals ()
			.OrderByDescending (o => o.GameObject.transform.position.magnitude)
			.Take (3);
		GoalRoom = possibleGoals.ElementAt (MazeRandom.Next (0, possibleGoals.Count ()));

		GoalPos = Convert.WorldToUnit(GoalRoom.GameObject.transform.position);
		var goal = new Collectable (CollectableConstants.GoalId, Instantiate (GoalPrefab) as GameObject);
		if(showTip) {
			InitHelpTipForCollectable(CollectableConstants.GoalId);
		}
		GoalRoom.AddCollectable (goal);

	}

	private int RoomId(int x, int y) {
		var xpart = (x << DescriptionMasks.XShift) & DescriptionMasks.PositionX;
		var ypart = (y << DescriptionMasks.YShift) & DescriptionMasks.PositionY;
		return xpart + ypart;
	}

	private int RoomId(Point2D p) {
		return RoomId (p.X, p.Y);
	}
	
	private void AddRoom(int x, int y, IRoom room) {
		Rooms [RoomId (x,y)] = room;
	}
	private IRoom GetRoom(Point2D pos) {
		return Rooms [RoomId (pos.X, pos.Y)];
	}

	private void CreateCollectables(int count, int type, GameObject prefab) {
		for (var i = 0; i < count; ++i) {
			var pos = MazeRandom.Next (MaxPoint);
			while(pos == GoalPos) {
				pos = MazeRandom.Next (MaxPoint);
			}

			var collectable = new Collectable(type, Instantiate (prefab) as GameObject);
			GetRoom (pos).AddCollectable(collectable);
		}
	}

	private void InitHelpTipForCollectable(int type) {
		var helpTip = GameObject.Find ("HelpTipDetector").GetComponent<HelpTip2> ();
		helpTip.Text = Collectable.GetHelpTipText (type);
		helpTip.Offset = Collectable.GetHelpTipOffset (type);
		helpTip.Size = Collectable.GetHelpTipSize (type);
		helpTip.ObjectTagToTip = Collectable.GetHelpTipTag (type);
	}

	private void CreateCoins(int count, bool showTip) {
		CreateCollectables(count,CollectableConstants.CoinId, CoinPrefab);
		if (showTip) {
			InitHelpTipForCollectable (CollectableConstants.CoinId);
		}
	}

	private void CreateTimeBoosts(int count, bool showTip) {
		CreateCollectables(count,CollectableConstants.TimeBoostId, TimeBoostPrefab);
		if (showTip) {
			InitHelpTipForCollectable (CollectableConstants.TimeBoostId);
		}
	}

	private void CreateBlueDoor(bool showTip) {
		GoalRoom.AddChildObject (new MazeObject (Instantiate (BlueDoorPrefab) as GameObject));
		CreateCollectables (1, CollectableConstants.BlueSwitchId, BlueSwitchPrefab);
		
		if (showTip) {
			InitHelpTipForCollectable (CollectableConstants.BlueSwitchId);
		}
	}

	/// <summary>
	/// Generates the Maze. Public entry point for the maze generation.
	/// </summary>
	/// <param name="width">Width of maze in room count.</param>
	/// <param name="height">Height of maze in room count.</param>
	public void GenerateWorld(int width = 5, int height = 5) {
		GenerateWorld (new WorldConfig {
			Width = width,
			Height = height, 
			CoinCount = 10,
			TimeBoostCount = 2
		});
	}

	public void DestroyWorld() {
		ClearMazeObjects ();
	}

	public void GenerateWorld(WorldConfig config) {
		MaxPoint = new Point2D (config.Width, config.Height);
		MinPoint = new Point2D (); // (0,0)
		
		CurrentMode = Mode.Newest;
		
		InitializeGrid (config.Width, config.Height);
		
		ClearMazeObjects ();
		
		GenerateGrid ();
		
		CreateRoomGameObjects ();
		
		CreateGoal (config.GoalTip);
		
		CreateCoins (config.CoinCount, config.CoinTip);
		
		CreateTimeBoosts (config.TimeBoostCount, config.TimeBoostTip);

		if (config.BlueDoor) {
			CreateBlueDoor (config.SwitchTip);
		}
	}

}
