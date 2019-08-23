using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HelloWorldConsole.DAL.DataAccess
{
    public interface IDataConnect:IDisposable        
    {
        int ExecuteSqlQuery(SqlCommand objSqlCommand);

        int ExecuteScalarSqlQuery(SqlCommand objSqlCommand);

        DataSet GetDataSet(SqlCommand objCmd);

        DataTable GetDataTable(SqlCommand objCmd);

        DataTable GetDataTableByExecuteReader(SqlCommand objCmd);

		T ExecuteProcedure<T>(string procedureName, dynamic modelParams, bool executeNonQuery = false) where T : class, new();

        DataTable ExecuteDataSet(string procedureName, Dictionary<dynamic, dynamic> parameters);
    }
}
