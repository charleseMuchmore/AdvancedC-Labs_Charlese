using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using MMABooksBusinessClasses;
using System.Runtime.CompilerServices;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerTests
    {
        private Customer def;
        private Customer c;

        [SetUp]
        public void SetUp()
        {
            def = new Customer();
            c = new Customer(5, "Donald, Duck", "101 Main Street", "Orlando", "FL", "10001");
        }

        [Test]
        public void TestConstructor() 
        { 
            Assert.IsNotNull(def);
            Assert.AreEqual(null, def.Name);
            Assert.AreEqual(null, def.Address);
            Assert.AreEqual(null, def.City);
            Assert.AreEqual(null, def.State);
            Assert.AreEqual(null, def.ZipCode);

            Assert.IsNotNull(c);
            Assert.AreNotEqual(null, c.Name);
            Assert.AreEqual("Donald, Duck", c.Name);
            Assert.AreNotEqual(null, c.Address);
            Assert.AreNotEqual(null, c.City);
            Assert.AreNotEqual(null, c.CustomerID);
            Assert.AreNotEqual(null, c.State);
            Assert.AreNotEqual(null, c.ZipCode);
        }

        [Test]

        public void TestNameSetter()
        {
            c.Name = "Dasie, Duck";
            Assert.AreNotEqual("Donald, Duck", c.Name);
            Assert.AreEqual("Dasie, Duck", c.Name);
        }

        [Test]

        public void TestSetterNameTooLong()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Name = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
        }
    }
}
