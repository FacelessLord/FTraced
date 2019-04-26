using System;
using FluentAssertions;
using GlLib.Client.API;
using NUnit.Framework;

namespace Tests.Client
{
    public class LayoutTests
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Starting layouts tests");
        }

        #region Calculations

        [Test]
        public void Layout_SomeNumbers()
        {

        }

        #endregion

        #region Division Problems

        [Test]
        public void Layout_With_ZeroValues()
        {
            var layout = new Layout(0, 0, 0, 0, 0, 0, 0, 0);
            Action getFrame = () => { layout.GetFrameUvProportions(1); };
            getFrame.Should().NotThrow();
        }

        [Test]
        public void Layout_With_ZeroFrame()
        {
            var layout = new Layout(1, 1, 2, 2, 3, 3, 3, 3);
            Action getFrame = () => { layout.GetFrameUvProportions(0); };
            getFrame.Should().NotThrow();
        }

        [Test]
        public void Layout_With_EqualValues()
        {
            var layout = new Layout(1, 1, 2, 2, 3, 3, 3, 4);
            Action getFrameX = () => { layout.GetFrameUvProportions(3); };
            Action getFrameY = () => { layout.GetFrameUvProportions(4); };

            getFrameX.Should().NotThrow();
            getFrameY.Should().NotThrow();
        }

        #endregion
    }
}