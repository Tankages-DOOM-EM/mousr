using System;
using NUnit.Framework;
namespace Mousr.UnitTests.Extensions
{
	[TestFixture]
	internal class RoomDescriptionExtensionsTests
	{
		[Test]
		[TestCase(Direction.North)]
		[TestCase(Direction.NorthEast)]
		[TestCase(Direction.NorthSouth)]
		[TestCase(Direction.NorthSouthEast)]
		[TestCase(Direction.NorthSouthWest)]
		[TestCase(Direction.NorthSouthWestEast)]
		[TestCase(Direction.NorthWest)]
		[TestCase(Direction.NorthWestEast)]
		public void HasExit_North_should_return_true_for_rooms_with_North_exit(int roomDescription) {
			Assert.That (roomDescription.HasExit (Direction.North));
		}
		
		[Test]
		[TestCase(Direction.West)]
		[TestCase(Direction.WestEast)]
		[TestCase(Direction.SouthWest)]
		[TestCase(Direction.SouthWestEast)]
		[TestCase(Direction.NorthWestEast)]
		[TestCase(Direction.NorthWest)]
		[TestCase(Direction.NorthSouthWestEast)]
		[TestCase(Direction.NorthSouthWest)]
		public void HasExit_West_should_return_true_for_rooms_with_West_exit(int roomDescription) {
			Assert.That (roomDescription.HasExit (Direction.West));
		}
		
		[Test]
		[TestCase(Direction.East)]
		[TestCase(Direction.NorthEast)]
		[TestCase(Direction.NorthSouthEast)]
		[TestCase(Direction.NorthSouthWestEast)]
		[TestCase(Direction.NorthWestEast)]
		[TestCase(Direction.SouthEast)]
		[TestCase(Direction.SouthWestEast)]
		[TestCase(Direction.WestEast)]
		public void HasExit_East_should_return_true_for_rooms_with_East_exit(int roomDescription) {
			Assert.That (roomDescription.HasExit (Direction.East));
		}
		
		[Test]
		[TestCase(Direction.South)]
		[TestCase(Direction.SouthEast)]
		[TestCase(Direction.SouthWest)]
		[TestCase(Direction.SouthWestEast)]
		[TestCase(Direction.NorthSouthWestEast)]
		[TestCase(Direction.NorthSouthWest)]
		[TestCase(Direction.NorthSouthEast)]
		[TestCase(Direction.NorthSouth)]
		public void HasExit_South_should_return_true_for_rooms_with_South_exit(int roomDescription) {
			Assert.That (roomDescription.HasExit (Direction.South));
		}
		
		[Test]
		[TestCase(Direction.South)]
		[TestCase(Direction.East)]
		[TestCase(Direction.West)]
		[TestCase(Direction.SouthWest )]
		[TestCase(Direction.SouthEast )]
		[TestCase(Direction.WestEast)]
		[TestCase(Direction.SouthWestEast )]
		public void HasExit_North_should_return_false_for_rooms_not_containing_North_exit(int roomDescription) {
			Assert.IsFalse (roomDescription.HasExit (Direction.North));
		}
		
		[Test]
		[TestCase(Direction.North)]
		[TestCase(Direction.East)]
		[TestCase(Direction.West)]
		[TestCase(Direction.NorthWest)]
		[TestCase(Direction.NorthEast )]
		[TestCase(Direction.WestEast)]
		[TestCase(Direction.NorthWestEast )]
		public void HasExit_South_should_return_false_for_rooms_not_containing_South_exit(int roomDescription) {
			Assert.IsFalse (roomDescription.HasExit (Direction.South));
		}
		
		[Test]
		[TestCase(Direction.North)]
		[TestCase(Direction.South)]
		[TestCase(Direction.East)]
		[TestCase(Direction.NorthEast )]
		[TestCase(Direction.SouthEast )]
		[TestCase(Direction.NorthSouth)]
		[TestCase(Direction.NorthSouthEast)]
		public void HasExit_West_should_return_false_for_rooms_not_containing_West_exit(int roomDescription) {
			Assert.IsFalse (roomDescription.HasExit (Direction.West));
		}
		
		[Test]
		[TestCase(Direction.North)]
		[TestCase(Direction.South)]
		[TestCase(Direction.West)]
		[TestCase(Direction.NorthWest)]
		[TestCase(Direction.SouthWest )]
		[TestCase(Direction.NorthSouth)]
		[TestCase(Direction.NorthSouthWest)]
		public void HasExit_East_should_return_false_for_rooms_not_containing_East_exit(int roomDescription) {
			Assert.IsFalse (roomDescription.HasExit (Direction.East));
		}

		/*
			[TestCase(Direction.North)]
			[TestCase(Direction.South)]
			[TestCase(Direction.East)]
			[TestCase(Direction.West)]
			[TestCase(Direction.NorthWest)]
			[TestCase(Direction.NorthEast )]
			[TestCase(Direction.SouthWest )]
			[TestCase(Direction.SouthEast )]
			[TestCase(Direction.NorthSouth)]
			[TestCase(Direction.WestEast)]
			[TestCase(Direction.NorthSouthEast)]
			[TestCase(Direction.NorthSouthWest)]
			[TestCase(Direction.NorthWestEast )]
			[TestCase(Direction.SouthWestEast )]
			[TestCase(Direction.NorthSouthWestEast)]
		 */

		[Test]
		[TestCase(Direction.NorthWest)]
		[TestCase(Direction.NorthEast )]
		[TestCase(Direction.SouthWest )]
		[TestCase(Direction.SouthEast )]
		public void IsCorner_should_return_true_for_corner_rooms(int roomDescription) {
			Assert.That (roomDescription.IsCorner ());
		}

		[Test]
		[TestCase(Direction.North)]
		[TestCase(Direction.South)]
		[TestCase(Direction.East)]
		[TestCase(Direction.West)]
		[TestCase(Direction.NorthSouth)]
		[TestCase(Direction.WestEast)]
		[TestCase(Direction.NorthSouthEast)]
		[TestCase(Direction.NorthSouthWest)]
		[TestCase(Direction.NorthWestEast )]
		[TestCase(Direction.SouthWestEast )]
		[TestCase(Direction.NorthSouthWestEast)]
		public void IsCorner_should_return_false_for_non_corner_rooms(int roomDescription) {
			Assert.IsFalse (roomDescription.IsCorner ());
		}
	}
}
