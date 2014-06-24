using System;

public class MazeRandom
{
	public static int DefaultSeed = 2062062060;

	private static Random _rand;
	private static Random rand { 
		get {
			if (_rand == null) {
				ReSeed(DefaultSeed);
			}
			return _rand;
		}
	}

	public static void ReSeed(int seed) {
		_rand = new Random (seed);
	}

	public static int Next(int max) {
		return rand.Next(max);
	}

	public static int Next(int min, int max) {
		return rand.Next(min, max);
	}

	public static Point2D Next(Point2D max) {
		return new Point2D (rand.Next (max.X), rand.Next (max.Y));
	}
}


