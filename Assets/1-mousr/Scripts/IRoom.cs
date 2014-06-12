using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public interface IRoom : IMazeObject {
	int Description { get; }
	void AddCollectable(ICollectable collectable);
	void AddChildObject(IMazeObject child);
	Point2D GetPosition ();
	void Destroy();
	int ItemCount { get; }
	int CoinCount { get; }
	int TimeBoostCount { get; }
}
