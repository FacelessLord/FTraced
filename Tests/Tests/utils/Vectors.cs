using System;
using FluentAssertions;
using GlLib.Utils;
using NUnit.Framework;

namespace Tests.utils
{
    public class VectorsTests
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Vectors Test");
        }

        #region RestrictedVector3D

        #region Other Methods

        [Test]
        public void RestrictedVector3D_Vectors_Rotate()
        {
            //Arrange
            var vector = new RestrictedVector3D(1, 0, 0);
            var expected = new RestrictedVector3D(-1, 0, 0);
            //Act
            //Rotating to U-turn
            //Assert
            Assert.AreEqual(vector.Rotate(Math.PI), expected);
        }

        [Test]
        public void RestrictedVector3D_From_String()
        {
            //Arrange
            var stringVector = "(1,1,0)";
            var expected = new RestrictedVector3D(1, 1, 0);
            //Act
            var result = RestrictedVector3D.FromString(stringVector);
            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RestrictedVector3D_To_String()
        {
            //Arrange
            var vector = new RestrictedVector3D(1, 1, 0);
            var expectedString = "(1,1,0)";
            //Act
            var result = vector.ToString();
            //Assert
            Assert.AreEqual(expectedString, result);
        }

        [Test]
        public void RestrictedVector3D_From_String_And_To_String()
        {
            //Arrange
            var stringVector = "(1,1,0)";
            //Act
            var resultVector = RestrictedVector3D.FromString(stringVector);
            var result = resultVector.ToString();
            //Assert
            Assert.AreEqual(stringVector, result);
        }

        #endregion

        #region Basic

        [Test]
        public void RestrictedVector3D_Vectors_Equal()
        {
            //Arrange
            var v1 = new RestrictedVector3D(45, 31, 56);
            var v2 = new RestrictedVector3D(45, 31, 56);
            //Act

            //Assert
            Assert.AreEqual(v1, v2);
        }


        [Test]
        public void MainCheck()
        {
            Assert.IsTrue(true);
        }

        #endregion

        #region Operators

        [Test]
        public void RestrictedVector3D_Equal()
        {
            //Arrange
            var v1 = new RestrictedVector3D(4, 1, 0);
            var v2 = new RestrictedVector3D(4, 1, 0);
            //Act
            // operator ==
            //Assert
            Assert.True(v1 == v2);
        }

        [Test]
        public void RestrictedVector3D_Not_Equal()
        {
            //Arrange
            var v1 = new RestrictedVector3D(4, 1, 0);
            var v2 = new RestrictedVector3D(-5, 4, 3);
            //Act
            // operator !=
            //Assert
            Assert.True(v1 != v2);
        }

        [Test]
        public void RestrictedVector3D_Sum_Same_Height()
        {
            //Arrange
            var v1 = new RestrictedVector3D(1, 2, 0);
            var v2 = new RestrictedVector3D(2, 1, 0);
            //Act
            var resultVector = v1 + v2;
            //Assert
            Assert.AreEqual(resultVector, new RestrictedVector3D(3, 3, 0));
        }

        [Test]
        public void RestrictedVector3D_Sum_Not_Same_Height()
        {
            //Arrange
            var v1 = new RestrictedVector3D(1, 2, 0);
            var v2 = new RestrictedVector3D(2, 1, 2);
            //Act
            Func<RestrictedVector3D, RestrictedVector3D, RestrictedVector3D> sum = (vector1, vector2) =>
                vector1 + vector2;
            //Assert
            Assert.Throws<ArgumentException>(() => sum(v1, v2));
        }

        [Test]
        public void RestrictedVector3D_Sub_Same_Height()
        {
            //Arrange
            var v1 = new RestrictedVector3D(1, 2, 0);
            var v2 = new RestrictedVector3D(2, 1, 0);
            //Act
            var resultVector = v1 - v2;
            //Assert
            Assert.AreEqual(resultVector, new RestrictedVector3D(-1, 1, 0));
        }

        [Test]
        public void RestrictedVector3D_To_Planar()
        {
            var v1 = new RestrictedVector3D(1, 2, 0);
            v1.ToPlanar().Should()
                .BeEquivalentTo(new PlanarVector(1, 2), "planar vec is a projection of restricted vec");
        }

        [Test]
        public void RestrictedVector3D_Sub_Not_Same_Height()
        {
            //Arrange
            var v1 = new RestrictedVector3D(1, 2, 0);
            var v2 = new RestrictedVector3D(2, 1, 2);
            //Act
            Func<RestrictedVector3D, RestrictedVector3D, RestrictedVector3D> sub = (vector1, vector2) =>
                vector1 + vector2;
            //Assert
            Assert.Throws<ArgumentException>(() => sub(v1, v2));
        }

        [Test]
        public void RestrictedVector3D_Mult_Not_Same_Height()
        {
            //Arrange
            var vector = new RestrictedVector3D(1, 1, 0);
            var expectedVector = new RestrictedVector3D(2, 2, 0);
            //Act
            //Multiplications of two vectors
            //Assert
            Assert.AreEqual(vector * 2, expectedVector);
        }

        #endregion

        #endregion

        #region PlanarVector

        #region Other Methods

        [Test]
        public void Planar_From_String()
        {
            //Arrange
            var stringVector = "(1,1)";
            var expected = new PlanarVector(1, 1);
            //Act
            var result = PlanarVector.FromString(stringVector);
            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Planar_To_String()
        {
            //Arrange
            var vector = new PlanarVector(1, 1);
            var expectedString = "(1,1)";
            //Act
            var result = vector.ToString();
            //Assert
            Assert.AreEqual(expectedString, result);
        }

        [Test]
        public void Planar_From_String_And_To_String()
        {
            //Arrange
            var stringVector = "(1,1)";
            //Act
            var resultVector = PlanarVector.FromString(stringVector);
            var result = resultVector.ToString();
            //Assert
            Assert.AreEqual(stringVector, result);
        }

        #endregion

        #region Basic

        [Test]
        public void Planar_Vectors_Equal()
        {
            //Arrange
            var v1 = new PlanarVector(45, 31);
            var v2 = new PlanarVector(45, 31);
            //Act

            //Assert
            Assert.AreEqual(v1, v2);
        }

        #endregion

        #region Operators

        [Test]
        public void Planar_Vector_Equal()
        {
            //Arrange
            var v1 = new PlanarVector(4, 1);
            var v2 = new PlanarVector(4, 1);
            //Act
            // operator ==
            //Assert
            Assert.True(v1 == v2);
        }

        [Test]
        public void Planar_Vectors_Not_Equal()
        {
            //Arrange
            var v1 = new PlanarVector(4, 1);
            var v2 = new PlanarVector(0, 1);
            //Act
            // operator !=
            //Assert
            Assert.True(v1 != v2);
        }

        [Test]
        public void Planar_Vector_Sum()
        {
            //Arrange
            var v1 = new PlanarVector(1, 2);
            var v2 = new PlanarVector(2, 1);
            //Act
            var resultVector = v1 + v2;
            //Assert
            Assert.AreEqual(resultVector, new PlanarVector(3, 3));
        }


        [Test]
        public void Planar_Vector_Sub()
        {
            //Arrange
            var v1 = new PlanarVector(1, 2);
            var v2 = new PlanarVector(2, 1);
            //Act
            var resultVector = v1 - v2;
            //Assert
            Assert.AreEqual(resultVector, new PlanarVector(-1, 1));
        }


        [Test]
        public void Planar_Vector_Mult()
        {
            //Arrange
            var vector = new PlanarVector(1, 2);
            var expectedVector = new PlanarVector(2, 4);
            //Act
            //Multiplications of two vectors
            //Assert
            Assert.AreEqual(vector * 2, expectedVector);
        }

        #endregion

        #endregion
    }
}