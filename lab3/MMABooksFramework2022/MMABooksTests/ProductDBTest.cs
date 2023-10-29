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
    internal class ProductDBTests
    {
        ProductDB db;

        [SetUp]
        public void ResetData()
        {
            db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            ProductProps p = (ProductProps)db.Retrieve(2177);
            Assert.AreEqual(2177, p.ProductId);
            Assert.AreEqual("A4CS", p.ProductCode);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll();
            Assert.AreEqual(696, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            ProductProps p = (ProductProps)db.Retrieve(2177);
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(2177));
        }


        [Test]
        public void TestDeleteForeignKeyConstraint()
        {
            ProductProps p = (ProductProps)db.Retrieve(2177);
            Assert.Throws<MySqlException>(() => db.Delete(p));
        }

        [Test]
        public void TestUpdate()
        {
            ProductProps p = (ProductProps)db.Retrieve(2177);
            p.ProductCode = "A4CS";
            Assert.True(db.Update(p));
            p = (ProductProps)db.Retrieve(2177);
            Assert.AreEqual("A4CS", p.ProductCode);
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            ProductProps p = (ProductProps)db.Retrieve(2177);
            p.ProductCode = "Oregon is the state where Crater Lake National Park is.";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }

        [Test]
        public void TestCreate()
        {
            ProductProps p = new ProductProps();
            p.ProductCode = "ABCD";
            db.Create(p);
            ProductProps p2 = (ProductProps)db.Retrieve(p.ProductCode);
            Assert.AreEqual(p.GetState(), p2.GetState());
        }

        [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            ProductProps p = new ProductProps();
            p.ProductId = 2177;
            p.ProductCode = "A4CS";
            Assert.Throws<MySqlException>(() => db.Create(p));
        }
    }
}
