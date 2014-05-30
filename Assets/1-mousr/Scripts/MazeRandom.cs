using System;

public class MazeRandom
{
	public static int Seed = 2062062060;
	private static Random rand = null;

	private static void CheckSeeded() {
		if (rand != null) {
			return;
		}
		rand = new Random (Seed);
	}

	public static int Next(int max) {
		CheckSeeded ();
		return rand.Next(max);
	}

	public static int Next(int min, int max) {
		CheckSeeded ();
		return rand.Next(min, max);
	}

	public static Point2D Next(Point2D max) {
		CheckSeeded ();
		return new Point2D (rand.Next (max.X), rand.Next (max.Y));
	}
}


