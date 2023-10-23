using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using MMABooksBusinessClasses;
using MMABooksDBClasses;

namespace MMABooksTests
{
    public class ProductDBTests
    {
        [Test]
        public void TestGetProduct()
        {
            Product p = ProductDB.GetProduct("CRFC");
            Assert.AreEqual("CRFC", p.ProductCode);
        }

        [Test]
        public void TestAddProduct()
        {
            Product p = new Product();
            p.ProductCode = "ABCD";
            p.Description = "fluffy cat toy round";
            p.UnitPrice = (decimal)4.35;
            p.OnHandQuantity = 100;

            Product prod = ProductDB.GetProduct("ABCD");
            Assert.AreEqual("ABCD", p.ProductCode);
        }

        [Test]
        public void TestDeleteProduct()
        {
            Product p = new Product();
            p.ProductCode = "ABCD";
            p.Description = "fluffy cat toy round";
            p.UnitPrice = (decimal)4.35;
            p.OnHandQuantity = 100;

            bool deletedOrNot = ProductDB.DeleteProduct(p);
            Assert.IsTrue(deletedOrNot);
        }

        [Test]
        public void TestUpdateProduct()
        {
            Product oldP = new Product();
            oldP.ProductCode = "ABCD";
            oldP.Description = "fluffy cat toy round";
            oldP.UnitPrice = (decimal)4.35;
            oldP.OnHandQuantity = 100;

            /*int productCode = ProductDB.AddProduct(oldP);
            oldP = ProductDB.GetProduct(ProductCode);*/

            Product newP = new Product();
            newP.ProductCode = "ABCC";
            newP.Description = "A real cool hat. Blue.";
            newP.UnitPrice = (decimal)44.99;
            newP.OnHandQuantity = 10;

            bool updated = ProductDB.UpdateProduct(oldP, newP);
            Assert.IsTrue(updated);
        }
    }
}
