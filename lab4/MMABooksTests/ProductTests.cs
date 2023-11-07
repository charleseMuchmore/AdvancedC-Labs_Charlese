using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
/*using MMABooksEFClasses.MarisModels;*/
using MMABooksEFClasses.MODELS;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Pkcs;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        MMABOOKSCONTEXT dbContext;
        Product? p;
        List<Product>? products;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABOOKSCONTEXT();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetProductData()");
        }

        [Test]
        public void GetAllTest()
        {
            products = dbContext.Products.OrderBy(p => p.ProductCode).ToList(); //LINQ expression Lambda
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            p = dbContext.Products.Find("A4CS");
            Assert.IsNotNull(p);
            Assert.AreEqual("A4CS", p.ProductCode);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the products that have a unit price of 56.50
            products = dbContext.Products.Where(p => p.UnitPrice == (decimal)56.50).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetWithCalculatedFieldTest()
        {
            // get a list of objects that include the productcode, unitprice, quantity and inventoryvalue
            var products = dbContext.Products.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }
        }

        [Test]
        public void DeleteTest()
        {
            p = dbContext.Products.Find("A4CS"); 
            dbContext.Products.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Products.Find("A4CS"));

        }

        [Test]
        public void CreateTest()
        {
            Product p = new Product();
            p.ProductCode = "ZA32";
            p.Description = "Coding for Bears";
            p.UnitPrice = 14.99m;
            p.OnHandQuantity = 1;
            dbContext.Add(p);
            dbContext.SaveChanges();

            Assert.NotNull(dbContext.Products.Find("ZA32"));
            Product p2 = dbContext.Products.Find("ZA32");
            Assert.AreEqual("Coding for Bears", p2.Description);
            Assert.AreEqual(p.UnitPrice, p2.UnitPrice);

        }

        [Test]
        public void UpdateTest()
        {
            p = dbContext.Products.Find("A4CS");
            p.Description = "New Edition";
            p.OnHandQuantity = 97401;
            p.UnitPrice = 9.99m;

            dbContext.Update(p);
            dbContext.SaveChanges();

            Assert.NotNull(dbContext.Products.Find("A4CS"));
            Product p2 = dbContext.Products.Find("A4CS");
            Assert.AreEqual(p2.Description, p.Description);


        }

        public void PrintAll(List<Product> products)
        {
            foreach (Product p in products)
            {
                Console.WriteLine(p);
            }
        }

    }
}