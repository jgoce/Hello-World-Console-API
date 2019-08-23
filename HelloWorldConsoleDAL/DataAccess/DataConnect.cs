using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections;

namespace HelloWorldConsole.DAL.DataAccess
{
	/// <summary>
	/// DB connection class
	/// </summary>
	public class DataConnect : IDataConnect
	{
        //public static readonly string Connection = @"Initial Catalog=Database;Data Source=DatabaseServer;Integrated Security=SSPI;";
        //public static readonly string Connection = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
        public static readonly string Connection = Environment.GetEnvironmentVariable("DBConnection", EnvironmentVariableTarget.Machine);// Creating SqlConnection class object.
		private readonly SqlConnection dbConnection;
		private bool disposed;

		public DataConnect()
		{
			this.dbConnection = new SqlConnection(DataConnect.Connection);
		}

        public DataConnect(string conStr)
        {
            this.dbConnection = new SqlConnection(conStr);
        }

        public SqlConnection getDataConnect()
        {
            return this.dbConnection;
        }

		/// <summary>
		/// Finalizes the object instance.
		/// </summary>
		~DataConnect()
		{
			this.Dispose(false);
		}

		public int ExecuteSqlQuery(SqlCommand objSqlCommand)
		{
			int intReturnValue = 0;

			try
			{
				// Opening database connection.
				if (this.OpenDBConnection() == true)
				{
					// Assigning sql query to command object.
					objSqlCommand.Connection = this.dbConnection;

					// Executing sql query.
					intReturnValue = objSqlCommand.ExecuteNonQuery();
                    
					// Returning record id or rows affected as sql query executed successfully.
					return intReturnValue;
				}
				else
				{
					// Returning 0 as connection to database not established.
					return intReturnValue;
				}
			}
			catch (SqlException)
			{ 
				return intReturnValue;
			}
			catch (Exception)
			{
				// Error.                
				return intReturnValue;
			}
			finally
			{
				this.CloseDBConnection();                
			}
		}

		public int ExecuteScalarSqlQuery(SqlCommand objSqlCommand)
		{
			int intReturnValue = 0;
            
			try
			{
				// Opening database connection.
				if (this.OpenDBConnection() == true)
				{
					// Assigning sql query to command object.
					objSqlCommand.Connection = this.dbConnection;

					// Executing sql query.
					intReturnValue = Convert.ToInt32(objSqlCommand.ExecuteScalar());                  

					// Returning record id or rows affected as sql query executed successfully.
					return intReturnValue;
				}
				else
				{
					// Returning 0 as connection to database not established.
					return intReturnValue;
				}
			}
			catch (SqlException)
			{ 
				return intReturnValue;
			}
			catch (Exception)
			{
				// Error.                                
				return intReturnValue;
			}
			finally
			{
				this.CloseDBConnection(); 
			}
		}

		public DataSet GetDataSet(SqlCommand objCmd)
		{
			// Creating object of DataAdapter class and assigning sql query and 			
			var objDA = new SqlDataAdapter(objCmd); 
			try
			{ 
				// Opening database connection.
				if (this.OpenDBConnection() == true)
				{
					// Creating object of DataSet class.
					var objDS = new DataSet();

					// connection object to DataAdapter object.
					objCmd.Connection = this.dbConnection;
                    
					// Filling records in DataAdapter object.
					objDA.Fill(objDS);

					// returning DataTable.
					return objDS;
				}
				else
				{
					// Returning null as connection to database not established. 
					return null;
				}
			}
			catch (Exception)
			{ 
				return null;
			}
			finally
			{
				this.CloseDBConnection();
				// Destroying DataAdapter object, releasing memory.
				objDA.Dispose();
			}
		}

		public DataTable GetDataTable(SqlCommand objCmd)
		{
			// Creating object of DataAdapter class and assigning sql query and 			
			var objDA = new SqlDataAdapter(objCmd);
			try
			{
				// Opening database connection.
				if (this.OpenDBConnection() == true)
				{
					// Creating object of DataTable class.
					var objDt = new DataTable();

					// connection object to DataAdapter object.
					objCmd.Connection = this.dbConnection;
                    objCmd.CommandTimeout = 0;

					// Filling records in DataAdapter object.
					objDA.Fill(objDt);

					// returning DataTable.
					return objDt;
				}
				else
				{
					// Returning null as connection to database not established. 
					return null;
				}
			}
			catch (Exception ex)
			{
                throw ex;
			}
			finally
			{
				this.CloseDBConnection();
				// Destroying DataAdapter object, releasing memory.
				objDA.Dispose();
			}
		}

		public DataTable GetDataTableByExecuteReader(SqlCommand objCmd)
		{
			try
			{
				// Opening database connection.
				if (this.OpenDBConnection() == true)
				{
					// Creating object of DataSet class.
					var objDT = new DataTable();
					SqlDataReader objDR;

					objCmd.Connection = this.dbConnection;
					objDR = objCmd.ExecuteReader();
					objDT.Load(objDR);

					// returning DataTable.
					return objDT;
				}
				else
				{
					// Returning null as connection to database not established. 
					return null;
				}
			}
			catch (Exception)
			{
				return null;
			}
			finally
			{
				// Closing database connection.
				this.CloseDBConnection();
			}
		}

		public T ExecuteProcedure<T>(string procedureName, dynamic modelParams, bool executeNonQuery = false) where T : class, new()
		{
			var objcmd = new SqlCommand();
			objcmd.CommandType = CommandType.StoredProcedure;
			objcmd.CommandText = procedureName;

			PropertyInfo[] propertyInfos = modelParams.GetType().GetProperties();

			foreach (PropertyInfo property in propertyInfos)
			{
				if (property.Name == "DynamicProperties")
				{
					foreach (var dynamicProperty in modelParams.DynamicProperties)
					{
						objcmd.Parameters.AddWithValue(string.Format("@{0}", dynamicProperty.Key), dynamicProperty.Value);
					}
				}
				else if (property.Name != "StoredProcedureName")
				{
					var outputAttribute = Attribute.GetCustomAttribute(property, typeof(Attribute)) as Attribute;
					if (outputAttribute == null)
					{
						objcmd.Parameters.AddWithValue(string.Format("@{0}", property.Name), property.GetValue(modelParams));
					}
					else
					{
						var parameter = new SqlParameter(string.Format("@{0}", property.Name), property.GetValue(modelParams) == null ? "" : property.GetValue(modelParams));
						parameter.Size = 1000;
						parameter.Direction = ParameterDirection.Output;
						objcmd.Parameters.Add(parameter);
					}
				}
			}

			Type returnTypeName = typeof(T);
			object toReturn;
			if (executeNonQuery)
			{
				toReturn = this.ExecuteSqlQuery(objcmd);
			}
			else if (returnTypeName == typeof(DataSet))
			{
				toReturn = this.GetDataSet(objcmd);
			}
			else
			{
				toReturn = this.GetDataTable(objcmd);
			}
			
			return toReturn as T;
		}


        public T ExecuteFunction<T>(string sqlText, dynamic modelParams, bool executeNonQuery = false) where T : class, new()
        {
            var objcmd = new SqlCommand();
            objcmd.CommandText = sqlText;
            if(modelParams !=null) 
            foreach (KeyValuePair<string, object> property in modelParams) // enumerating over it exposes the Properties and Values as a KeyValuePair
            {
                objcmd.Parameters.AddWithValue(string.Format("@{0}", property.Key), property.Value ?? DBNull.Value);
            }

            Type returnTypeName = typeof(T);
            object toReturn;
            if (executeNonQuery)
            {
                toReturn = this.ExecuteSqlQuery(objcmd);
            }
            else if (returnTypeName == typeof(DataSet))
            {
                toReturn = this.GetDataSet(objcmd);
            }
            else
            {
                toReturn = this.GetDataTable(objcmd);
            }

            if (returnTypeName is IList || returnTypeName.IsGenericType)
            {
                throw new NotImplementedException();
            }

            return toReturn as T;
        }

		public DataTable ExecuteDataSet(string procedureName, Dictionary<dynamic, dynamic> parameters)
		{
			var objcmd = new SqlCommand();
			objcmd.CommandType = CommandType.StoredProcedure;
			objcmd.CommandText = procedureName;
           
			try
			{
				foreach (var pair in parameters) //used string for key instead of dynamic because it doesn't make sense to have a dynamic key
				{
					var parameter = objcmd.CreateParameter();
					parameter.ParameterName = pair.Key;
					parameter.Value = pair.Value;

					objcmd.Parameters.Add(parameter);
				}

				return this.GetDataTable(objcmd);
			}
			catch (SqlException ex)
			{
				//  log the exception
				throw (ex);
			}
		}

		/// <summary>
		/// Disposes of instance state.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes of instance state.
		/// </summary>
		/// <param name="disposing">Determines whether this was called by Dispose or by the finalize.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					// We can safely dispose of .NET resources.
				}
				this.disposed = true;
			}
		}

		private bool OpenDBConnection()
		{
			try
			{ 
				lock (this.dbConnection)
				{
					// Opening connection to database.
					if (this.dbConnection.State == ConnectionState.Closed)
					{
						this.dbConnection.Open();
					}
				}

				// Returning true as connection to database established successfully.
				return true;
			}
			catch (Exception ex)
			{
                throw ex;
			}
			finally 
			{ 
			}
		}

		private bool CloseDBConnection()
		{
			try
			{
				lock (this.dbConnection)
				{
					// Closing connection to database.
					if (this.dbConnection.State == ConnectionState.Open)
					{
						this.dbConnection.Close();
					}
				}
				// Returning true as connection to database closed successfully.
				return true;
			}
			catch (Exception)
			{ 
				return false;
			}
			finally
			{ 
			}
		}
	}
}