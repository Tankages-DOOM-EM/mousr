using System;
using NUnit.Framework;
namespace Mousr.UnitTests
{
	[TestFixture]
	internal class ConvertTests
	{
		[Test]
		public void Should_convert_positive_x_from_unit_to_world_and_back(
			[Values(1,3,191)]int x) {
			var world = Convert.UnitToWorld (x, 0);
			var unit = Convert.WorldToUnit (world);
			Assert.AreEqual (x, unit.X);
		}

		[Test]
		public void Should_convert_positive_y_from_unit_to_world_and_back(
			[Values(1,3,191)]int y) {
			var world = Convert.UnitToWorld (0, y);
			var unit = Convert.WorldToUnit (world);
			Assert.AreEqual (y, unit.Y);
		}
		
		[Test]
		public void Should_convert_negative_x_from_unit_to_world_and_back(
			[Values(-1,-3,-191)]int x) {
			var world = Convert.UnitToWorld (x, 0);
			var unit = Convert.WorldToUnit (world);
			Assert.AreEqual (x, unit.X);
		}
		
		[Test]
		public void Should_convert_negative_y_from_unit_to_world_and_back(
			[Values(-1,-3,-191)]int y) {
			var world = Convert.UnitToWorld (0, y);
			var unit = Convert.WorldToUnit (world);
			Assert.AreEqual (y, unit.Y);
		}

		[Test]
		public void Should_convert_origin_from_unit_to_world_and_back() {
			var world = Convert.UnitToWorld (0, 0);
			var unit = Convert.WorldToUnit (world);
			Assert.AreEqual (0, unit.X);
			Assert.AreEqual (0, unit.Y);
		}
		
		[Test]
		public void Should_convert_Point2D_the_same_as_two_ints() {
			int x = 3, y = 5;
			var point = new Point2D (x, y);
			
			var world = Convert.UnitToWorld (x, y);
			var pointWorld = Convert.UnitToWorld (point);
			
			Assert.AreEqual (world, pointWorld);
		}
		
		[Test]
		public void Should_convert_Vector3_the_same_as_two_floats() {
			var world = Convert.UnitToWorld (4, -4);
			var pointVec = Convert.WorldToUnit (world);
			var pointFloats = Convert.WorldToUnit (world.x, world.y);
			
			Assert.AreEqual (pointVec, pointFloats);
		}
		
		[Test]
		public void UnitToWorld_should_set_z_component_to_zero() {
			var world = Convert.UnitToWorld (-1, 7);
			Assert.AreEqual (0, world.z);
		}
	}
}
