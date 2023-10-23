using System;

namespace MMABooksBusinessClasses
{
    public class Product
    {
        public Product() { }

        public Product(string productCode, string description, decimal unitPrice, int onHandQuantity)
        {
            ProductCode = productCode;
            Description = description;
            UnitPrice = unitPrice;
            OnHandQuantity = onHandQuantity;
        }

        private string productCode;
        private string description;
        private decimal unitPrice;
        private int onHandQuantity;

        public string ProductCode 
        { 
            get
            {
                return productCode;
            } 
            set
            {
                if (value.Length > 0 && value.Length <= 10) 
                    productCode = value;
                else
                    throw new ArgumentOutOfRangeException("Must be at least one character and no more than 10 characters");
            }
        }
        public string Description 
        { 
            get
            {
                return description;
            }
            set
            {
                if (value.Length > 0 && value.Length <= 50)
                    description = value;
                else
                    throw new ArgumentOutOfRangeException("Must be at least one character and no more than 50 characters");
            }
        }
        public decimal UnitPrice 
        { 
            get
            {
                return unitPrice;
            }
            set
            {
                if (value > 0)
                    unitPrice = value;
                else
                    throw new ArgumentOutOfRangeException("UnitPrice must be positive decimal value");
            }
        } 
        public int OnHandQuantity 
        { 
            get
            {
                return onHandQuantity;
            }
            set
            {
                if (value >= 0)
                    onHandQuantity = value;
                else
                    throw new ArgumentOutOfRangeException("OnHandQuantity must be non-negative");
            }
        }
    }
}
