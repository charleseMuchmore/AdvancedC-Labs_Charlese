using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using MMABooksBusinessClasses;
using MMABooksDBClasses;

namespace MMABooksTests
{
    public class CustomerDBTests
    {

        [Test]
        public void TestGetCustomer()
        {
            Customer c = CustomerDB.GetCustomer(1);
            Assert.AreEqual(1, c.CustomerID);
        }

        public void TestGetCustomerList()
        {
            Customer c = CustomerDB.GetCustomer(1);
            Assert.AreEqual(1, c.CustomerID);
        }

        [Test]
        public void TestAddCustomer()
        {
            Customer c = new Customer();
            c.Name = "Mickey Mouse";
            c.Address = "101 Main Street";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10101";

            int customerID = CustomerDB.AddCustomer(c);
            c = CustomerDB.GetCustomer(customerID);
            Assert.AreEqual("Mickey Mouse", c.Name);
        }

        [Test]
        public void TestDeleteCustomer()
        {
            Customer c = new Customer();
            c.Name = "Mickey Mouse";
            c.Address = "101 Main Street";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10101";

            int customerID = CustomerDB.AddCustomer(c);
            c = CustomerDB.GetCustomer(customerID);
            bool deletedOrNot = CustomerDB.DeleteCustomer(c);
            Assert.IsTrue(deletedOrNot);
        }

        [Test]
        public void TestUpdateCustomer()
        {
            Customer oldC = new Customer();
            oldC.Name = "Mickey Mouse";
            oldC.Address = "101 Main Street";
            oldC.City = "Orlando";
            oldC.State = "FL";
            oldC.ZipCode = "10101";

            int customerID = CustomerDB.AddCustomer(oldC);
            oldC = CustomerDB.GetCustomer(customerID);

            Customer newC = new Customer();
            newC.Name = "Mickey Mouse";
            newC.Address = "101 Main Street";
            newC.City = "Orlando";
            newC.State = "FL";
            newC.ZipCode = "10101";

            bool updated = CustomerDB.UpdateCustomer(oldC, newC);
            Assert.IsTrue(updated);
        }
    }
}
