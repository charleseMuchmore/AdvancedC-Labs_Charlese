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
    public class CustomerTests
    {

        [SetUp]
        public void TestResetDatabase()
        {
            CustomerDB db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetStateData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewStateConstructor()
        {
            // not in Data Store - no code
            Customer c = new Customer();
            Assert.AreEqual(string.Empty, c.Id);
            Assert.AreEqual(string.Empty, c.Name);
            Assert.IsTrue(c.IsNew);
            Assert.IsFalse(c.IsValid);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Customer c = new Customer("Lindy Stewart");
            Assert.AreEqual("Lindy Stewart", c.Id);
            Assert.IsTrue(c.Name.Length > 0);
            Assert.IsFalse(c.IsNew);
            Assert.IsTrue(c.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Customer c = new Customer();
            c.Name = "This is a new Name";
            c.Save();
            Customer c2 = new Customer("This is a new Name");
            Assert.AreEqual(c2.Name, c.Name);
        }

        [Test]
        public void TestUpdate()
        {
            Customer c = new Customer("Brian Bird");
            c.Name = "Edited Name";
            c.Save();

            Customer c2 = new Customer("Brian Bird");
            Assert.AreEqual(c2.Id, c.Id);
            Assert.AreEqual(c2.Name, c.Name);
        }

        [Test]
        public void TestDelete()
        {
            Customer c = new Customer("HI");
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer("HI"));
        }

        [Test]
        public void TestGetList()
        {
            Customer c = new Customer();
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(1, customers[0].Id);
            Assert.AreEqual("Molunguri, A", customers[0].Name);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "John Doe";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Customer c = new Customer();
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Name = "hgoaigiogifjpisgj");
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer("Bob Joe");
            Customer c2 = new Customer("Bob Joe");

            c1.Name = "Updated first";
            c1.Save();

            c2.Name = "Updated second";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}