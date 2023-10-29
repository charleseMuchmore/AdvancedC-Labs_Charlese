using NUnit.Framework;

using MMABooksProps;
using System;
using System.Linq;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductPropsTests
    {
        ProductProps props;
        [SetUp]
        public void Setup()
        {
            props = new ProductProps();
            props.ProductCode = "A4CS";
        }

        [Test]

        public void TestGetState()
        {
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.ProductCode));
        }

        [Test]
        public void TestSetState()
        {
            string jsonString = props.GetState();
            ProductProps newProps = new ProductProps();
            newProps.SetState(jsonString);
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }

        [Test]
        public void TestClone()
        {
            ProductProps newProps = (ProductProps)props.Clone();
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
    }
}
