using System;

using MMABooksTools;
using MMABooksProps;
using MMABooksDB;

using System.Collections.Generic;

namespace MMABooksBusiness
{
    public class Customer : BaseBusiness
    {
        //this property setter needs more accurate validation...
        public int Id
        {
            get
            {
                return ((CustomerProps)mProps).CustomerId;
            }

            /*set
            {
                if (!(value == ((CustomerProps)mProps).CustomerId))
                {
                    if (value >= 1 && value <= 2)
                    {
                        mRules.RuleBroken("Id", false);
                        ((CustomerProps)mProps).CustomerId = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("CustomerId must be no more than 2 characters long.");
                    }
                }
            }*/
        }

        public string Name
        {
            get
            {
                return ((CustomerProps)mProps).Name;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).Name))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 20)
                    {
                        mRules.RuleBroken("Name", false);
                        ((CustomerProps)mProps).Name = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Name must be no more than 20 characters long.");
                    }
                }
            }
        }

        public override object GetList()
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerProps> props = new List<CustomerProps>();


            props = (List<CustomerProps>)mdbReadable.RetrieveAll();
            foreach (CustomerProps prop in props)
            {
                Customer c = new Customer(prop);
                customers.Add(c);
            }

            return customers;
        }

        protected override void SetDefaultProperties()
        {
        }

        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("Id", true);
            mRules.RuleBroken("Name", true);
        }

        protected override void SetUp()
        {
            mProps = new CustomerProps();
            mOldProps = new CustomerProps();

            mdbReadable = new CustomerDB();
            mdbWriteable = new CustomerDB();
        }

        #region constructors
        public Customer() : base()
        {
        }

        public Customer(string key)
            : base(key)
        {
        }

        private Customer(CustomerProps props)
            : base(props)
        {
        }

        #endregion
    }
}

