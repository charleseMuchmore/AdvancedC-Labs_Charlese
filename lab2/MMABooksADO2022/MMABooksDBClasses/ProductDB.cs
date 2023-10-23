using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;
using System.Data;
using MMABooksBusinessClasses;

namespace MMABooksDBClasses
{
    public static class ProductDB
    {
        public static Product GetProduct(string productCode)
        {
            MySqlConnection connection = MMABooksDB.GetConnection();
            string selectStatement
                = "SELECT ProductCode, Description, UnitPrice, OnHandQuantity "
                + "FROM Products "
                + "WHERE ProductCode = @ProductCode";
            MySqlCommand selectCommand =
                new MySqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@ProductCode", productCode);
            try
            {
                connection.Open();
                MySqlDataReader prodReader =
                    selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (prodReader.Read())
                {
                    Product product = new Product();
                    product.ProductCode = prodReader["ProductCode"].ToString();
                    product.Description = prodReader["Description"].ToString();
                    product.UnitPrice = (decimal)prodReader["UnitPrice"];
                    product.OnHandQuantity = (int)prodReader["OnHandQuantity"];
                    return product;
                }
                else
                {
                    return null;
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public static List<Product> GetList()
        {
            MySqlConnection connection = MMABooksDB.GetConnection();
            try
            {
                connection.Open();
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return new List<Product>();
        }
        public static bool AddProduct(Product product)
        {
            MySqlConnection connection = MMABooksDB.GetConnection();
            string insertStatement =
                "INSERT Products " +
                "(ProductCode Description, UnitPrice, OnHandQuantity) " +
                "VALUES (@ProductCode, @Description, @UnitPrice, @OnHandQuantity)";
            MySqlCommand insertCommand =
                new MySqlCommand(insertStatement, connection);
            insertCommand.Parameters.AddWithValue(
                "@ProductCode", product.ProductCode);
            insertCommand.Parameters.AddWithValue(
                "@Description", product.Description);
            insertCommand.Parameters.AddWithValue(
                "@UnitPrice", product.UnitPrice);
            insertCommand.Parameters.AddWithValue(
                "@OnHandQuantity", product.OnHandQuantity);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                // MySQL specific code for getting last pk value
                string selectStatement =
                    "SELECT LAST_INSERT_ID()";
                MySqlCommand selectCommand =
                    new MySqlCommand(selectStatement, connection);
                if (product.ProductCode != null)
                    return true;
                else
                    return false;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return false;
        }

        public static bool DeleteProduct(Product product) 
        {
            MySqlConnection connection = MMABooksDB.GetConnection();
            string deleteStatement =
                "DELETE FROM Products " +
                "WHERE ProductCode = @ProductCode " +
                "AND Description = @Description " +
                "AND UnitPrice = @UnitPrice " +
                "AND OnHandQuantity = @OnHandQuantity ";
            MySqlCommand deleteCommand =
                 new MySqlCommand(deleteStatement, connection);
            deleteCommand.Parameters.AddWithValue("@ProductCode", product.ProductCode);
            deleteCommand.Parameters.AddWithValue("@Description", product.Description);
            deleteCommand.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
            deleteCommand.Parameters.AddWithValue("@OnHandQuantity", product.OnHandQuantity);

            try
            {
                connection.Open();
                MySqlDataReader reader = deleteCommand.ExecuteReader();
                return (reader.RecordsAffected == 1) ? true : false;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool UpdateProduct(Product oldProduct, Product newProduct)
        {
            MySqlConnection connection = MMABooksDB.GetConnection();
            string updateStatement =
                "UPDATE Products SET " +
                "ProductCode = @NewProductCode, " +
                "Description = @NewDescription, " +
                "UnitPrice = @NewUnitPrice, " +
                "OnHandQuantity = @NewOnHandQuantity, " +
                "WHERE ProductCode = @OldProductCode " +
                "AND Description = @OldDescription " +
                "AND UnitPrice = @OldUnitPrice " +
                "AND OnHandQuantity = @OldOnHandQuantity ";
            MySqlCommand updateCommand =
                new MySqlCommand(updateStatement, connection);
            // new customer stuff
            updateCommand.Parameters.AddWithValue(
                "@NewName", newProduct.ProductCode);
            updateCommand.Parameters.AddWithValue(
                "@NewAddress", newProduct.Description);
            updateCommand.Parameters.AddWithValue(
                "@NewCity", newProduct.UnitPrice);
            updateCommand.Parameters.AddWithValue(
                "@NewState", newProduct.OnHandQuantity);
            // old customer stuff 
            updateCommand.Parameters.AddWithValue(
                "@OldCustomerID", oldProduct.ProductCode);
            updateCommand.Parameters.AddWithValue(
                "@OldName", oldProduct.Description);
            updateCommand.Parameters.AddWithValue(
                "@OldAddress", oldProduct.UnitPrice);
            updateCommand.Parameters.AddWithValue(
                "@OldCity", oldProduct.OnHandQuantity);
            try
            {
                connection.Open();
                // execute the command
                int rows = updateCommand.ExecuteNonQuery();

                // if the number of records returned = 1, return true otherwise return false
                if (rows == 1)
                    return true;
                else
                    return false;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return false;
        }
    }
}
