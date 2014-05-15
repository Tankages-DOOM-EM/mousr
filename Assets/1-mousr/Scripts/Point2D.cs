using UnityEngine;
using System.Collections;

public class Point2D {
	public int X, Y;

	public Point2D() : this(0,0) {}
	public Point2D(int x, int y) {
		X = x;
		Y = y;
	}

	public Point2D(ref Point2D other): this(other.X, other.Y) {}
	
	public static Point2D operator+(Point2D lhs, Point2D rhs) {
		return new Point2D(lhs.X + rhs.X, lhs.Y + rhs.Y);
	}
	
	public static Point2D operator-(Point2D lhs, Point2D rhs) {
		return new Point2D(lhs.X - rhs.X, lhs.Y - rhs.Y);
	}
	
	public static bool operator<=(Point2D lhs, Point2D rhs) {
		return lhs.X <= rhs.X && lhs.Y <= rhs.Y;
	}
	
	public static bool operator>=(Point2D lhs, Point2D rhs) {
		return lhs.X >= rhs.X && lhs.Y >= rhs.Y;
	}
	
	public static bool operator<(Point2D lhs, Point2D rhs) {
		return lhs.X < rhs.X && lhs.Y < rhs.Y;
	}
	
	public static bool operator>(Point2D lhs, Point2D rhs) {
		return lhs.X > rhs.X && lhs.Y > rhs.Y;
	}
}
