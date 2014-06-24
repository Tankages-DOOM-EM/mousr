using System;
using System.Linq;
using NUnit.Framework;

namespace Mousr.UnitTests
{
	[TestFixture]
	internal class DirectionTests
	{
		[Test]
		[TestCase(Direction.NorthEast, Direction.North | Direction.East)]
		[TestCase(Direction.NorthWest, Direction.North | Direction.West)]
		[TestCase(Direction.SouthEast, Direction.South | Direction.East)]
		[TestCase(Direction.SouthWest, Direction.South | Direction.West)]
		[TestCase(Direction.NorthSouth, Direction.South | Direction.North)]
		[TestCase(Direction.WestEast, Direction.West | Direction.East)]
		[TestCase(Direction.NorthSouthEast, Direction.North | Direction.South | Direction.East)]
		[TestCase(Direction.NorthSouthWest, Direction.North | Direction.South | Direction.West)]
		[TestCase(Direction.SouthWestEast, Direction.West | Direction.South | Direction.East)]
		[TestCase(Direction.NorthWestEast, Direction.West | Direction.North | Direction.East)]
		[TestCase(Direction.NorthSouthWestEast, Direction.South | Direction.West | Direction.North | Direction.East)]
		public void Directions_contain_only_their_respective_components(int compoundDir, int composedDir) {
			Assert.AreEqual (compoundDir, composedDir);
		}

		[Test]
		[TestCase(Direction.North, Direction.South)]
		[TestCase(Direction.South, Direction.North)]
		[TestCase(Direction.West, Direction.East)]
		[TestCase(Direction.East, Direction.West)]
		public void Opposite_should_return_correct_opposite_direction(int dir, int opposite) {
			Assert.AreEqual (opposite, Direction.Opposite (dir));
		}

		[Test]
		public void Shuffle_should_always_have_all_four_directions() {
			Direction.Shuffled = null;
			for(var i = 0; i < 100; ++i) {
				Direction.Shuffle();
				Assert.Contains(Direction.North, Direction.Shuffled);
				Assert.Contains(Direction.West, Direction.Shuffled);
				Assert.Contains(Direction.South, Direction.Shuffled);
				Assert.Contains(Direction.East, Direction.Shuffled);
			}
		}
		
		[Test]
		public void Shuffle_should_change_order() {
			Direction.Shuffled = null;
			Direction.Shuffle ();
			var firstShuffle = new int[4];
			for (var i = 0; i < 4; ++i) {
				firstShuffle [i] = Direction.Shuffled [i];
			}
			Direction.Shuffle ();

			var allEqual = true;
			for (var i = 0; i < 4; ++i) {
				allEqual &= (firstShuffle [i] == Direction.Shuffled [i]);
			}
			Assert.IsFalse (allEqual);
		}

		[Test]
		[TestCase(Direction.North, 0, -1)]
		[TestCase(Direction.South, 0, 1)]
		[TestCase(Direction.West, -1, 0)]
		[TestCase(Direction.East, 1, 0)]
		public void Delta_should_return_correct_value_for_each_direction(int direction, int dx, int dy) {
			Assert.AreEqual(Direction.Delta (direction), new Point2D(dx,dy));
		}
	}
}
