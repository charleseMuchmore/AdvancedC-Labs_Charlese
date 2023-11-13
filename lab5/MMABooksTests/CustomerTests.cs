using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
//using MMABooksEFClasses.MarisModels;
using MMABooksEFClasses.MODELS;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerTests
    {
        MMABOOKSCONTEXT dbContext;
        Customer? c;
        List<Customer>? customers;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABOOKSCONTEXT();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetCustomer1Data()");
        }

        [Test]
        public void GetAllTest()
        {
            customers = dbContext.Customers.OrderBy(c => c.Name).ToList(); //LINQ expression Lambda
            Assert.AreEqual(199, customers.Count);
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
            PrintAll(customers);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            c = dbContext.Customers.Find(1);
            Assert.IsNotNull(c);
            Assert.AreEqual("Molunguri, A", c.Name);
            Console.WriteLine(c);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the customers who live in OR
            customers = dbContext.Customers.Where(c => c.State == "OR").ToList();
            Assert.AreEqual(2, customers.Count);
            Assert.AreEqual("Swenson, Vi", customers[0].Name);
            PrintAll(customers);
        }

        [Test]
        public void GetWithInvoicesTest()
        {
            // get the customer whose id is 20 and all of the invoices for that customer
            c = dbContext.Customers.Include("Invoices").Where(c => c.CustomerId == 20).SingleOrDefault();
            Assert.IsNotNull(c);
            Assert.AreEqual(20, c.CustomerId);
            Assert.AreEqual(3, c.Invoices.Count);
            Console.WriteLine(c);
        }

        [Test]
        public void GetWithJoinTest()
        {
            // get a list of objects that include the customer id, name, statecode and statename
            var customers = dbContext.Customers.Join(
               dbContext.States,
               c => c.State,
               s => s.StateCode,
               (c, s) => new { c.CustomerId, c.Name, c.State, s.StateName }).OrderBy(r => r.StateName).ToList();
            Assert.AreEqual(199, customers.Count);
            // I wouldn't normally print here but this lets you see what each object looks like
            foreach (var c in customers)
            {
                Console.WriteLine(c);
            }
        }

        [Test]
        public void DeleteTest()
        {
            c = dbContext.Customers.Find(1);
            dbContext.Customers.Remove(c);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Customers.Find(1));
        }

        [Test]
        public void CreateTest() //name address city state zip
        {
            Customer p = new Customer();
            p.Name = "Joe";
            p.Address = "123 Quincy Street";
            p.City = "SmallsVille";
            p.State = "NJ";
            p.ZipCode = "12345";
            dbContext.Add(p);
            dbContext.SaveChanges();

            Assert.NotNull(dbContext.Customers.Find(p.CustomerId));
            Customer p2 = dbContext.Customers.Find(p.CustomerId);
            Assert.AreEqual("123 Quincy Street", p2.Address);
            Assert.AreEqual(p.City, p2.City);
        }

        [Test]
        public void UpdateTest()
        {
            c = dbContext.Customers.Find(1);
            c.Name = "New Name";
            c.Address = "123 Blue street";
            c.City = "TheCity";
            c.State = "OR";
            c.ZipCode = "11111";

            dbContext.Update(c);
            dbContext.SaveChanges();

            Assert.NotNull(dbContext.Customers.Find(1));
            Customer p2 = dbContext.Customers.Find(1);
            Assert.AreEqual(p2.Name, c.Name);
        }

        public void PrintAll(List<Customer> customers)
        {
            foreach (Customer c in customers)
            {
                Console.WriteLine(c);
            }
        }

    }
}