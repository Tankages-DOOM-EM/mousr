using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public interface ICollectable : IMazeObject {
	int Type { get; }
	IRoom ContainingRoom { get; set; }
}
