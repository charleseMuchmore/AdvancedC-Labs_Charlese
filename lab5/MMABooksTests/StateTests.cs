using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
//using MMABooksEFClasses.MarisModels;
using MMABooksEFClasses.MODELS;
using Microsoft.EntityFrameworkCore;
using MMABooksEFClasses.MODELS;

namespace MMABooksTests
{
    [TestFixture]
    public class StateTests
    {
        MMABOOKSCONTEXT dbContext;
        State? s;
        List<State>? states;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABOOKSCONTEXT();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetStateData()");
        }

        [Test]
        public void GetAllTest()
        {
            states = dbContext.States.OrderBy(s => s.StateName).ToList(); //LINQ expression Lambda
            Assert.AreEqual(53, states.Count);
            Assert.AreEqual("Alabama", states[0].StateName);
            PrintAll(states);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            s = dbContext.States.Find("OR");
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s.StateName);
            Console.WriteLine(s);
        }

        [Test]
        public void GetUsingWhere()
        {
            states = dbContext.States.Where(s => s.StateName.StartsWith("A")).OrderBy(s => s.StateName).ToList();
            Assert.AreEqual(4, states.Count);
            Assert.AreEqual("Alabama", states[0].StateName);
            PrintAll(states);
        }

        [Test]
        public void GetWithCustomersTest()
        {
            s = dbContext.States.Include("Customers").Where(s => s.StateCode == "OR").SingleOrDefault();
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s.StateName);
            Assert.AreEqual(3, s.Customers.Count);
            Console.WriteLine(s);
        }

        [Test]
        public void DeleteTest()
        {
            s = dbContext.States.Find("HI");
            dbContext.States.Remove(s);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.States.Find("HI"));
        }

        [Test]
        public void CreateTest()
        {
            State p = new State();
            p.StateCode = "ZA";
            p.StateName = "Zamingan";
            dbContext.Add(p);
            dbContext.SaveChanges();

            Assert.NotNull(dbContext.States.Find("ZA"));
            State p2 = dbContext.States.Find("ZA");
            Assert.AreEqual("Zamingan", p2.StateName);
            Assert.AreEqual(p.StateCode, p2.StateCode);
        }

        [Test]
        public void UpdateTest()
        {
            s = dbContext.States.Find("OR");
            s.StateName = "Origon";

            dbContext.Update(s);
            dbContext.SaveChanges();

            Assert.NotNull(dbContext.States.Find("OR"));
            State p2 = dbContext.States.Find("OR");
            Assert.AreEqual(p2.StateName, s.StateName);
        }

        public void PrintAll(List<State> states)
        {
            foreach (State s in states)
            {
                Console.WriteLine(s);
            }
        }
    }
}