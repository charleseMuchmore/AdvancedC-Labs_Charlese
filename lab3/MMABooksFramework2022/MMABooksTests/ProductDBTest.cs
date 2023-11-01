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
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            ProductProps c = (ProductProps)db.Retrieve(1);
            Assert.AreEqual(1, c.ProductId);
            Assert.AreEqual("A4CS", c.ProductCode);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll();
            Assert.AreEqual(16, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            ProductProps c = (ProductProps)db.Retrieve(1);
            Assert.True(db.Delete(c));
            Assert.Throws<Exception>(() => db.Retrieve(1));
        }

        [Test]
        public void TestUpdate()
        {
            ProductProps c = (ProductProps)db.Retrieve(1);
            c.ProductCode = "CODE";
            Assert.True(db.Update(c));
            c = (ProductProps)db.Retrieve(1);
            Assert.AreEqual("CODE", c.ProductCode);
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            ProductProps c = (ProductProps)db.Retrieve(1);
            c.ProductCode = "Oregon is the state where Crater Lake National Park is. fffffffffffffffffffffffffffffffffffffffffffffffffffff";
            Assert.Throws<MySqlException>(() => db.Update(c));
        }

        [Test]
        public void TestCreate()
        {
            ProductProps c = new ProductProps();
            c.ProductCode = "HEYO";
            c.Description = "Cool New Kicks";
            c.UnitPrice = 5.0000M;
            c.OnHandQuantity = 5;


            db.Create(c);
            ProductProps c2 = (ProductProps)db.Retrieve(c.ProductId);
            Assert.AreEqual(c.GetState(), c2.GetState());
        }

   /*     [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            ProductProps c = new ProductProps();
            c.ProductId = 1;
            c.ProductCode = "NewC";
            c.Description = "jgiaofjdfs";
            c.UnitPrice = 5;
            c.OnHandQuantity = 5;
            Assert.Throws<MySqlException>(() => db.Create(c));
        }*/

    }
}
