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
	
	public static bool operator==(Point2D lhs, Point2D rhs) {
		return lhs.X == rhs.X && lhs.Y == rhs.Y;
	}
	
	public static bool operator!=(Point2D lhs, Point2D rhs) {
		return lhs.X != rhs.X || lhs.Y != rhs.Y;
	}
	
	public bool Equals(Point2D rhs) {
		return rhs != null && this == rhs;
	}
	
	public override bool Equals(System.Object rhs) {

		return rhs != null && this == (Point2D)rhs;
	}

	public override int GetHashCode() {
		return this.X ^ this.Y;
	}

	public override string ToString() {
		return string.Format ("X: {0}, Y: {1}", this.X.ToString("D2"), this.Y.ToString("D2"));
	}
}
