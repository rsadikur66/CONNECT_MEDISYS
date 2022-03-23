using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConnectKsmcDAL
{
    public class DatabaseDAL
    {
        private readonly string connectionString;
        public DatabaseDAL()
        {
            connectionString = "user id=sysdev;password=medisys;data source=175.144.214.85:1521/HISKL;";
        }
        private IDbConnection Connection
        {
            get
            {
                return new OracleConnection(connectionString);
            }
        }
        protected T LoginQuery<T>(string query, object param)
        {
            using var con = Connection;
            return con.Query<T>(query, param).SingleOrDefault();
        }
        protected IEnumerable<T> QueryList<T>(string query)
        {
            using var con = Connection;
            return con.Query<T>(query).ToList();
        }
        protected T QuerySingle<T>(string query)
        {
            using var con = Connection;
            return con.Query<T>(query).FirstOrDefault();
        }
        protected string QueryString(string query)
        {
            try
            {
                using var con = Connection;
                return con.QuerySingle<string>(query);
            }
            catch (InvalidOperationException)
            {
                return "";
            }
        }
        protected DataTable ReportQuery(string query)
        {
            using var con = new OracleConnection(connectionString);
            var com = new OracleCommand(query, con);
            var adp = new OracleDataAdapter(com);
            var data = new DataTable();
            adp.Fill(data);
            return data;
        }
        protected bool Command(string command)
        {
            return Connection.Execute(command) > 0;
        }
        public DataTable ExecuteSelectProcedure(String procedureName, OracleParameter[] parameters)
        {
            var oracleConnection = new OracleConnection(connectionString);
            var oracleCommand = new OracleCommand
            {
                Connection = oracleConnection,
                CommandType = CommandType.StoredProcedure,
                CommandText = procedureName
            };
            oracleCommand.Parameters.AddRange(parameters);
            oracleCommand.Parameters.Add("p_value", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            var oracleDataAdapter = new OracleDataAdapter(oracleCommand);
            var dataTable = new DataTable();
            oracleDataAdapter.Fill(dataTable);
            return dataTable;
        }
    }
}