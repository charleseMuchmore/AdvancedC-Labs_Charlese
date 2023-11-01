using System;

using MMABooksTools;
using MMABooksProps;

using System.Data;

// *** I use an "alias" for the ado.net classes throughout my code
// When I switch to an oracle database, I ONLY have to change the actual classes here
using DBBase = MMABooksTools.BaseSQLDB;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBParameter = MySql.Data.MySqlClient.MySqlParameter;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBDataAdapter = MySql.Data.MySqlClient.MySqlDataAdapter;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;

namespace MMABooksDB
{
    public class ProductDB : DBBase, IReadDB, IWriteDB
    {
        public ProductDB() : base() { }
        public ProductDB(DBConnection cn) : base(cn) { }

        public IBaseProps Create(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductCreate";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("prodID", DBDbType.Int32);
            command.Parameters.Add("prodCode", DBDbType.VarChar);
            command.Parameters.Add("description", DBDbType.VarChar);
            command.Parameters.Add("unit_p", DBDbType.Float);
            command.Parameters.Add("onhand_q", DBDbType.Int32);

            command.Parameters[0].Direction = ParameterDirection.Output;
            command.Parameters["prodCode"].Value = props.ProductCode;
            command.Parameters["description"].Value = props.Description;
            command.Parameters["unit_p"].Value = props.UnitPrice;
            command.Parameters["onhand_q"].Value = props.OnHandQuantity;


            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ProductId = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;
                    return props;
                }
                else
                    throw new Exception("Unable to insert record. " + props.GetState());
            }
            catch (Exception e)
            {
                // log this error
                throw e;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }

        public bool Delete(IBaseProps p)
        {
            ProductProps props = (ProductProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("prodID", DBDbType.Int32);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["prodID"].Value = props.ProductId;
            command.Parameters["conCurrId"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    string message = "Record cannot be deleted. It has been edited by another user.";
                    throw new Exception(message);
                }

            }
            catch (Exception e)
            {
                // log this exception
                throw e;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }

        public IBaseProps Retrieve(object key)
        {
            DBDataReader data = null;
            ProductProps props = new ProductProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_ProductSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("prodID", DBDbType.Int32);
            command.Parameters["prodID"].Value = (int)key;

            try
            {
                data = RunProcedure(command);
                if (!data.IsClosed)
                {
                    if (data.Read())
                    {
                        props.SetState(data);
                    }
                    else
                        throw new Exception("Record does not exist in the database.");
                }
                return props;
            }
            catch (Exception e)
            {
                // log this exception
                throw e;
            }
            finally
            {
                if (data != null)
                {
                    if (!data.IsClosed)
                        data.Close();
                }
            }
        }

        public object RetrieveAll()
        {
            List<ProductProps> list = new List<ProductProps>();
            DBDataReader reader = null;
            ProductProps props;

            try
            {
                reader = RunProcedure("usp_ProductSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new ProductProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                // log this exception
                throw e;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }

        public bool Update(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductUpdate";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("prodID", DBDbType.Int32);
            command.Parameters.Add("prodCode", DBDbType.VarChar);
            command.Parameters.Add("description", DBDbType.VarChar);
            command.Parameters.Add("unit_p", DBDbType.Decimal);
            command.Parameters.Add("onhand_q", DBDbType.Int32);
            command.Parameters.Add("conCurrId", DBDbType.Int32);

            command.Parameters["prodID"].Value = props.ProductId;
            command.Parameters["prodCode"].Value = props.ProductCode;
            command.Parameters["description"].Value = props.Description;
            command.Parameters["unit_p"].Value = props.UnitPrice;
            command.Parameters["onhand_q"].Value = props.OnHandQuantity;
            command.Parameters["conCurrId"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ProductId = (int)command.Parameters[0].Value;
                    props.ConcurrencyID++;
                    return true;
                }
                else
                {
                    string message = "Record cannot be updated. It has been edited by another user.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                // log this exception
                throw e;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }
    }
}
