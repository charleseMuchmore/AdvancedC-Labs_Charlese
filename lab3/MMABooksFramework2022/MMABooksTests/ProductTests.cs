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
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewStateConstructor()
        {
            // not in Data Store - no code
            Product s = new Product();
            Assert.AreEqual(string.Empty, s.Description);
            Assert.IsTrue(s.IsNew);
            Assert.IsFalse(s.IsValid);
        }

        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Product s = new Product(1);
            Assert.AreEqual("A4CS", s.ProductCode);
            Assert.IsTrue(s.ProductCode.Length > 0);
            Assert.IsFalse(s.IsNew);
            Assert.IsTrue(s.IsValid);
        }


        [Test]
        public void TestSaveToDataStore()
        {
            Product s = new Product();
            s.ProductCode = "ABCD";
            s.Description = "Where am I";
            s.UnitPrice = 5M;
            s.OnHandQuantity = 6;
            s.Save();
            Product s2 = new Product(s.ProductId);
            Assert.AreEqual(s2.ProductCode, s.ProductCode);
            Assert.AreEqual(s2.Description, s.Description);
        }

        [Test]
        public void TestUpdate()
        {
            Product s = new Product(1);
            s.ProductCode = "OROR";
            s.Description = "blah blah lbah";
            s.UnitPrice = 5M;
            s.OnHandQuantity = 5;
            s.Save();

            Product s2 = new Product(1);
            Assert.AreEqual(s2.ProductId, s.ProductId);
            Assert.AreEqual(s2.ProductCode, s.ProductCode);
        }


        [Test]
        public void TestDelete()
        {
            Product s = new Product();
            s.ProductCode = "ABCC";
            s.Description = "This is a description";
            s.UnitPrice = 5M;
            s.OnHandQuantity = 6;
            s.Delete();
            s.Save();
            Assert.Throws<Exception>(() => new Product(s.ProductId));
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Product c1 = new Product(1);
            Product c2 = new Product(1);

            c1.ProductCode = "UPD1";
            c1.Save();

            c2.ProductCode = "UPD2";
            Assert.Throws<Exception>(() => c2.Save());
        }

        [Test]
        public void TestGetList()
        {
            Product p = new Product();
            List<Product> products = (List<Product>)p.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual(1, products[0].ProductId);
            Assert.AreEqual("A4CS", products[0].ProductCode);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
            p.ProductCode = "John Doe";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Product p = new Product();
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = "hgoaigiogifjpisgj");
        }

    }

}