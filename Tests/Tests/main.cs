using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using GlLib;
using GlLib.Common;

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
        public void GameStart()
        {
            Assert.DoesNotThrow(() => Core.Main(new string[0]));
        }
        [Test]
        public void MainCheck()
        {
            Assert.IsTrue(true);
        }
    }
}