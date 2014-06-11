using UnityEngine;
using System.Collections;

public static class RoomDescriptionExtensions
{
	public static bool HasExit(this int room, int direction) {
		return (direction & room) == direction;
	}
}

