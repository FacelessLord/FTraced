using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Utils;
using NUnit.Framework;

namespace Tests.ulits
{
    public class NbtTagTests
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Now NbtTag is testing");
        }

        #region Sets



        #endregion

        #region Gets



        #endregion

        #region Basic

        [Test]

        public void NbtTag_New_Is_Empty()
        {
            var tag = new NbtTag();
            Assert.AreEqual(tag.Count,0);
        }


        [Test]

        public void NbtTag_ToString_Formatter()
        {
            var tag = new NbtTag();
            //tag.c
        }

        #endregion
    }
}
