using System;
using GlLib.Utils;
using NUnit.Framework;

namespace Tests.ulits
{
    public class AxisAlignedBbTests
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Now AxisAlignedBb is testing");
        }

        #region AxisAlignedBb

        [Test]
        public void AxisAlignedBb_Equal_Same_Boxes()
        {
            //Arrange
            var bb1 = new AxisAlignedBb(0, 0, 15, 15);
            var bb2 = new AxisAlignedBb(0, 0, 15, 15);
            //Act

            //Assert
            Assert.AreEqual(bb1, bb2);
        }

        [Test]
        public void AxisAlignedBb_Equal_With_Vectors()
        {
            //Arrange
            var v1 = new PlanarVector(5, 5);
            var v2 = new PlanarVector(15, 15);
            var expectedBb = new AxisAlignedBb(5, 5, 15, 15);
            //Act
            var bb = new AxisAlignedBb(v1, v2);
            //Assert
            Assert.AreEqual(bb, expectedBb);
        }


        [Test]
        public void AxisAlignedBb_Equal()
        {
            //Arrange
            var bb1 = new AxisAlignedBb(0, 0, 15, 15);
            var bb2 = new AxisAlignedBb(15, 15, 0, 0);
            //Act

            //Assert
            Assert.AreEqual(bb1, bb2);
        }

        private static void CheckHeights(AxisAlignedBb _bb1, AxisAlignedBb _bb2)
        {
            var height1 = _bb1.Height;
            var height2 = _bb2.Height;

            Assert.AreEqual(height1, height2);
        }

        private static void CheckWidths(AxisAlignedBb _bb1, AxisAlignedBb _bb2)
        {
            var height1 = _bb1.Width;
            var height2 = _bb2.Width;

            Assert.AreEqual(height1, height2);
        }

        [TestCase]
        public void AxisAlignedBb_Get_Heights()
        {
            var arrangePairs = new[]
            {
                (new AxisAlignedBb(0, 0, 15, 15),
                    new AxisAlignedBb(0, 0, -15, -15)),
                (new AxisAlignedBb(0, 0, 0, 0),
                    new AxisAlignedBb(0, 0, -0, 0)),
                (new AxisAlignedBb(0, 0, 42, 42),
                    new AxisAlignedBb(42, 42, 84, 84))
            };

            foreach (var pair in arrangePairs)
                CheckHeights(pair.Item1, pair.Item2);
        }

        [TestCase]
        public void AxisAlignedBb_Get_Widths()
        {
            var arrangePairs = new[]
            {
                (new AxisAlignedBb(0, 0, 15, 15),
                    new AxisAlignedBb(0, 0, -15, -15)),
                (new AxisAlignedBb(0, 0, 0, 0),
                    new AxisAlignedBb(0, 0, -0, 0)),
                (new AxisAlignedBb(0, 0, 42, 42),
                    new AxisAlignedBb(42, 42, 84, 84))
            };

            foreach (var pair in arrangePairs)
                CheckWidths(pair.Item1, pair.Item2);
        }

        private static void CheckIfIntersectsIsTrue(AxisAlignedBb _bb1, AxisAlignedBb _bb2)
        {
            Assert.IsTrue(_bb1.IntersectsWith(_bb2),
                $"{_bb1} should have intersection with {_bb2}");
        }

        [TestCase]
        public void AxisAlignedBb_IntersectsWith()
        {
            var arrangePairs = new[]
            {
                (new AxisAlignedBb(0, 0, 42, 42),
                    new AxisAlignedBb(13, 13, 84, 84)),
                (new AxisAlignedBb(0, 0, 42, 42),
                    new AxisAlignedBb(13, 13, 24, 24))
            };

            foreach (var pair in arrangePairs)
                CheckIfIntersectsIsTrue(pair.Item1, pair.Item2);
        }


        private static void CheckIfIntersectsIsFalse(AxisAlignedBb _bb1, AxisAlignedBb _bb2)
        {
            Assert.IsFalse(_bb1.IntersectsWith(_bb2),
                $"{_bb1} shouldn't have intersection with {_bb2}");
        }

        [TestCase]
        public void AxisAlignedBb_Not_IntersectsWith()
        {
            var arrangePairs = new[]
            {
                (new AxisAlignedBb(0, 0, 0, 0),
                    new AxisAlignedBb(0, 0, 4, 4)),
                (new AxisAlignedBb(0, 0, 0, 0),
                    new AxisAlignedBb(0, 0, 0, 0)),
                (new AxisAlignedBb(0, 0, 0, 0),
                    new AxisAlignedBb(1, 1, 4, 4))
            };

            foreach (var pair in arrangePairs)
                CheckIfIntersectsIsFalse(pair.Item1, pair.Item2);
        }

        #endregion
    }
}