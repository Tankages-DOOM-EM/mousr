using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldConfig {
	public int Width { get; set; }
	public int Height { get; set; }

	public int CoinCount { get; set; }
	public int TimeBoostCount { get; set; }
	public bool BlueDoor { get; set; }

	public bool CoinTip { get; set; }
	public bool TimeBoostTip { get; set; }
	public bool SwitchTip { get; set; }
	public bool GoalTip { get; set; }
}
