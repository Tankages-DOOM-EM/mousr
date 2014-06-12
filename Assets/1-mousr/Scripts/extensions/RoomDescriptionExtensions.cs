using UnityEngine;
using System.Collections;

public static class RoomDescriptionExtensions
{
	public static bool HasExit(this int roomDescription, int direction) {
		return (direction & roomDescription) == direction;
	}

	public static int ExitCount(this int roomDescription) {
		return (roomDescription & DescriptionMasks.Direction);
	}

	public static bool IsHallway(this int roomDescription) {
		return roomDescription.ExitCount() == 2 && 
			(roomDescription.HasExit (Direction.NorthSouth) || roomDescription.HasExit (Direction.WestEast));
	}
	
	public static bool IsCorner(this int roomDescription) {
		return roomDescription.ExitCount() == 2 && 
			(roomDescription.HasExit (Direction.NorthSouth) || roomDescription.HasExit (Direction.WestEast));
	}
	
	public static bool IsDeadEnd(this int roomDescription) {
		return roomDescription.ExitCount() == 2 
			&& !roomDescription.HasExit (Direction.NorthSouth) 
			&& !roomDescription.HasExit (Direction.WestEast);
	}
}

