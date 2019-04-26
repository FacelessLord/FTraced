using System;
using System.Collections.Generic;
using FluentAssertions;
using GlLib.Utils;
using NUnit.Framework;

namespace Tests.utils
{
    public class NbtTagTests
    {
        /// <summary>
        ///     In this tests I've think that primitive types are classical primitive types and string.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Now NbtTag is tested");
        }

        #region Gets

        [TestCase(14)]
        [TestCase("Vasya")]
        [TestCase(13.5)]
        [TestCase(long.MaxValue)]
        public void NbtTag_Get_Primitive_Type<T>(T _value)
        {
            var tag = new NbtTag();
            tag.Set("SomeID", _value);

            tag.Get<T>("SomeID").Should().Be(_value, "Work of this func");
            tag.GetErrorNumber.Should().Be(0, "It were primitive types");
        }

        #endregion

        #region Sets

        [TestCase(1)]
        [TestCase("MyChain")]
        [TestCase(true)]
        [TestCase(1.23f)]
        [TestCase(long.MinValue)]
        public void NbtTag_Set_Primitive_Types<T>(T _obj)
        {
            Console.WriteLine("Setting type is:" + typeof(T));
            var tag = new NbtTag();
            tag.Set("Something", _obj);

            tag.Count.Should().Be(1, "We've added primitive type. Count of tags was increased");
            tag.GetErrorNumber.Should().Be(0, "We've added primitive type. There is no Errors");
        }

        [Test]
        public void NbtTag_Set_Bad_Types()
        {
            var expectedBadType = new DateTime();
            var tag = new NbtTag();
            tag.Set("Something", expectedBadType);


            tag.Count.Should().Be(1, "We've added nonprimitive type. Method \"set\" " +
                                     "should try set this type.");
            tag.GetErrorNumber.Should().Be(1, "We've added nonprimitive type.");
        }

        #endregion

        #region Basic

        [Test]
        public void NbtTag_Set_Primitive_Type_Simple()
        {
            var tag = new NbtTag();
            tag.Set("SomeId", 3);

            tag.Count.Should().Be(1, "We've added primitive type: int. Count of tags was increased");
            tag.GetErrorNumber.Should().Be(0, "We've added primitive type: int. There is no Errors");
        }

        [Test]
        public void NbtTag_New_Is_Empty()
        {
            var tag = new NbtTag();
            Assert.AreEqual(tag.Count, 0);
        }

        private const string TagName = "SomeThing";

        [TestCase("Name", TagName + "|SName")]
        [TestCase(12, TagName + "|I12")]
        [TestCase(1.0, TagName + "|D1")]
        public void NbtTag_ToString_Formatter<T>(T _value, string _expectedFormat)
        {
            var tag = new NbtTag();
            tag.Set(TagName, _value);
            tag.ToString().Should().Be(_expectedFormat);
        }


        [Test]
        public void NbtTag_FromString_Without_Errors()
        {
            const string expectedTagString = "SomeID|I23|SomeName|SMask";

            var tag = NbtTag.FromString(expectedTagString);

            tag.Count.Should().Be(2, "There are two items");
            tag.GetErrorNumber.Should().Be(0, "This items are primitive");
            tag.Get<int>("SomeID").Should().Be(23);
            tag.Get<string>("SomeName").Should().Be("Mask");
        }

        [Test]
        public void NbtTag_FromString_With_Only_Errors()
        {
            const string expectedTagString = "Home|Me|Home||Me@#";

            var tag = NbtTag.FromString(expectedTagString);

            tag.Count.Should().Be(0, "There are no items");
            tag.GetErrorNumber.Should().BeGreaterThan(0);
        }

        [Test]
        public void NbtTag_FromString_With_Errors()
        {
            const string expectedTagString = "SomeID|I23||SomeName|SMask|Mo|";

            var tag = NbtTag.FromString(expectedTagString);

            tag.Count.Should().Be(2, "There are two items");
            tag.GetErrorNumber.Should().BeGreaterThan(0);
            tag.Keys().Should().BeEquivalentTo(new List<string> {"SomeID", ""});
            tag.Get<int>("SomeID").Should().Be(23);
            tag.Get<string>("SomeName").Should().Be(null);
        }

        #endregion

        #region Other

        [Test]
        public void NbtTag_Retrieve_Should_Simple()
        {
            const string expectedTagString = "SomeID|I23|SomeName|SMask";
            var tag = NbtTag.FromString(expectedTagString);

            tag.CanRetrieveTag("Some").Should().BeTrue("Some -> SomeID");
            tag.CanRetrieveTag("SomeN").Should().BeTrue("SomeN -> SomeName");

            var someTag = tag.RetrieveTag("Some");
            someTag.Count.Should().Be(2, "tag contains two elements from starting tag");
            someTag.Get<int>("ID").Should().Be(23, "SomeID -> 23");
            someTag.Get<string>("Name").Should().Be("Mask", "SomeName -> Mask");
            tag.Count.Should().Be(0);

            tag = NbtTag.FromString(expectedTagString);
            tag.RetrieveTag("SomeN").ToString().Should().Be("ame|SMask");
            tag.ToString().Should().Be("SomeID|I23");
        }

        [Test]
        public void NbtTag_Retrieve_Empty_String()
        {
            const string expectedTagString = "SomeID|I23|SomeName|SMask";
            var tag = NbtTag.FromString(expectedTagString);

            tag.CanRetrieveTag("").Should().BeTrue(" \"\" -> *");

            tag.RetrieveTag("").ToString().Should().Be(expectedTagString, "Empty prefix " +
                                                                          "reproduce all tag items");
        }

        [Test]
        public void NbtTag_Retrieve_Empty_Tag()
        {
            var tag = new NbtTag();

            tag.CanRetrieveTag("Nam").Should().BeFalse("No items here");
            tag.CanRetrieveTag("").Should().BeFalse("No items here");

            tag.RetrieveTag("Nam").Count.Should().Be(0, "Empty tag");
        }

        #endregion
    }
}