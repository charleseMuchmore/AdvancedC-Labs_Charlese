using NUnit.Framework;

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
    internal class CustomerDBTests
    {
        CustomerDB db;

        [SetUp]
        public void ResetData()
        {
            db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            CustomerProps c = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual(1, c.CustomerId);
            Assert.AreEqual("Molunguri, A", c.Name);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<CustomerProps> list = (List<CustomerProps>)db.RetrieveAll();
            Assert.AreEqual(696, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            CustomerProps c = (CustomerProps)db.Retrieve(1);
            Assert.True(db.Delete(c));
            Assert.Throws<Exception>(() => db.Retrieve(1));
        }

/*
        [Test]
        public void TestDeleteForeignKeyConstraint()
        {
            CustomerProps c = (CustomerProps)db.Retrieve(99);
            Assert.Throws<MySqlException>(() => db.Delete(c));
        }*/

        [Test]
        public void TestUpdate()
        {
            CustomerProps c = (CustomerProps)db.Retrieve(1);
            c.Name = "Molunguri, A";
            Assert.True(db.Update(c));
            c = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual("Molunguri, A", c.Name);
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            CustomerProps c = (CustomerProps)db.Retrieve(1);
            c.Name = "Oregon is the state where Crater Lake National Park is. fffffffffffffffffffffffffffffffffffffffffffffffffffff";
            Assert.Throws<MySqlException>(() => db.Update(c));
        }

        [Test]
        public void TestCreate()
        {
            CustomerProps c = new CustomerProps();
            c.Name = "Minnie Mouse";
            c.Address = "101 Main Street";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10001";


            db.Create(c);
            CustomerProps c2 = (CustomerProps)db.Retrieve(c.CustomerId);
            Assert.AreEqual(c.GetState(), c2.GetState());
        }

        [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            CustomerProps c = new CustomerProps();
            c.CustomerId = 1;
            c.Name = "Molunguri, A";
            Assert.Throws<MySqlException>(() => db.Create(c));
        }
    }
}
