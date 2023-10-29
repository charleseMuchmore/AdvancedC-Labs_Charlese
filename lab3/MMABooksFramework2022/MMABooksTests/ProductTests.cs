using NUnit.Framework;

using MMABooksBusiness;
using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {

        [SetUp]
        public void TestResetDatabase()
        {
            ProductDB db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetStateData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewStateConstructor()
        {
            // not in Data Store - no code
            Product c = new Product();
            Assert.AreEqual(string.Empty, c.Id);
            Assert.AreEqual(string.Empty, c.Code);
            Assert.IsTrue(c.IsNew);
            Assert.IsFalse(c.IsValid);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Product c = new Product("Shiny Red Skillet");
            Assert.AreEqual("Shiny Red Skillet", c.Id);
            Assert.IsTrue(c.Code.Length > 0);
            Assert.IsFalse(c.IsNew);
            Assert.IsTrue(c.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Product c = new Product();
            c.Code = "BBZZ";
            c.Save();
            Product c2 = new Product("HHHH");
            Assert.AreEqual(c2.Code, c.Code);
        }

        [Test]
        public void TestUpdate()
        {
            Product c = new Product("1234");
            c.Code = "Edited Code";
            c.Save();

            Product c2 = new Product("1234");
            Assert.AreEqual(c2.Id, c.Id);
            Assert.AreEqual(c2.Code, c.Code);
        }

        [Test]
        public void TestDelete()
        {
            Product c = new Product("HI");
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Product("HI"));
        }

        [Test]
        public void TestGetList()
        {
            Product c = new Product();
            List<Product> products = (List<Product>)c.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual(2177, products[0].Id);
            Assert.AreEqual("A4CS", products[0].Code);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Product c = new Product();
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Product c = new Product();
            Assert.Throws<Exception>(() => c.Save());
            c.Code = "John Doe";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Product c = new Product();
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Code = "hgoaigiogifjpisgj");
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Product p1 = new Product("HEHE");
            Product p2 = new Product("HEHE");

            p1.Code = "0001";
            p1.Save();

            p2.Code = "0002";
            Assert.Throws<Exception>(() => p2.Save());
        }
    }
}