using System;
using GlLib.Common;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Testing starts");
        }

        [Test]
        public void MainCheck()
        {
            Assert.IsTrue(true);
        }
    }
}