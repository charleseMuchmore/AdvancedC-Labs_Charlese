using System;

using MMABooksTools;
using MMABooksProps;
using MMABooksDB;

using System.Collections.Generic;

namespace MMABooksBusiness
{
    public class Product : BaseBusiness
    {
        //this Id property needs some more accurate validation...
        public int Id
        {
            get
            {
                return ((ProductProps)mProps).ProductId;
            }

            set
            {
/*                if (!(value == ((ProductProps)mProps).ProductId)) //if value is not a dupe
                {
                    if (value >= 1 && value <= 2)
                    {
                        mRules.RuleBroken("Id", false);
                        ((ProductProps)mProps).ProductId = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("ProductId must be no more than 2 characters long.");
                    }
                }*/
            }
        }

        public string Code
        {
            get
            {
                return ((ProductProps)mProps).ProductCode;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).ProductCode))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 4)
                    {
                        mRules.RuleBroken("Code", false);
                        ((ProductProps)mProps).ProductCode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Code must be no more than 4 characters long.");
                    }
                }
            }
        }

        public override object GetList()
        {
            List<Product> products = new List<Product>();
            List<ProductProps> props = new List<ProductProps>();


            props = (List<ProductProps>)mdbReadable.RetrieveAll();
            foreach (ProductProps prop in props)
            {
                Product p = new Product(prop);
                products.Add(p);
            }

            return products;
        }

        protected override void SetDefaultProperties()
        {
        }

        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("Id", true);
            mRules.RuleBroken("Code", true);
        }

        protected override void SetUp()
        {
            mProps = new ProductProps();
            mOldProps = new ProductProps();

            mdbReadable = new ProductDB();
            mdbWriteable = new ProductDB();
        }

        #region constructors
        public Product() : base()
        {
        }

        public Product(string key)
            : base(key)
        {
        }

        private Product(ProductProps props)
            : base(props)
        {
        }

        #endregion
    }
}
