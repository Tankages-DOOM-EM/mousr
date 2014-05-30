using UnityEngine;
using System.Collections;

public static class RoomDescriptionExtensions
{
	public static bool HasExit(this int room, int direction) {
		return (direction & room) != 0;
	}
	
	public static bool HasCoin(this int room) {
		return (room & CollectableConstants.CoinId) != 0;
	}
	
	public static bool HasTimeBoost(this int room) {
		return (room & CollectableConstants.TimeBoostId) != 0;
	}
	public static int RoomItemCount(this int room) {
		var items = 0;
		if (room.HasCoin ()) {
			++items;
		}
		if (room.HasTimeBoost ()) {
			++items;
		}
		return items;
	}
}

