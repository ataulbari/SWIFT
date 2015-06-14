using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SWIFTReader
{
    public class DML
    {
        protected static string CONNECTION_STRING;       
        private static SqlConnection _oConnection;
        private static SqlTransaction _oTransaciton;
        StreamWriter _streamWriter;
        StreamReader _streamReader;

        public DML()
        {
            CONNECTION_STRING = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        #region Prepare Command

        private SqlCommand PrepareCommandForQuery(string sQuery, params SqlParameter[] oParameters)
        {
            SqlCommand oCommand;
            try
            {

                if ((_oConnection == null) || (_oConnection.ConnectionString != CONNECTION_STRING))
                {
                    _oConnection = new SqlConnection(CONNECTION_STRING);
                }
                if (_oConnection.State == ConnectionState.Closed)
                {
                    _oConnection.Open();
                }
                oCommand = new SqlCommand(sQuery, _oConnection);
                if (oParameters != null)
                {
                    for (int i = 0; i < oParameters.Length; i++)
                    {
                        oCommand.Parameters.Add(oParameters[i]);
                    }
                }
                oCommand.CommandText = sQuery;   ///////////////////////////////
                                
                oCommand.CommandType = CommandType.Text;
                if (_oTransaciton != null)
                {
                    if (_oTransaciton.Connection != null)
                    {
                        oCommand.Transaction = _oTransaciton;
                    }
                    else
                    {
                        throw new Exception("Invalid Transaction");
                    }
                }
                oCommand.CommandTimeout = 1800;
                return oCommand;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
                oCommand = new SqlCommand();
                return oCommand;
            }

        }
       
        private SqlCommand PrepareCommand(string sSPName, string sCommandType,params SqlParameter[] oParameters)
        {
            SqlCommand oCommand;
            try
            {

                if ((_oConnection == null) || (_oConnection.ConnectionString != CONNECTION_STRING))
                {
                    _oConnection = new SqlConnection(CONNECTION_STRING);
                }
                if (_oConnection.State == ConnectionState.Closed)
                {
                    _oConnection.Open();
                }
                oCommand = new SqlCommand(sSPName, _oConnection);
                for (int i = 0; i < oParameters.Length; i++)
                {
                    oCommand.Parameters.Add(oParameters[i]);
                }
                oCommand.CommandText = sSPName;   ///////////////////////////////

                oCommand.CommandType = CommandType.StoredProcedure;
                
                if (_oTransaciton != null)
                {
                    if (_oTransaciton.Connection != null)
                    {
                        //oCommand.Transaction = _oTransaciton;
                    }
                    else
                    {
                        throw new Exception("Invalid Transaction");
                    }
                }
                oCommand.CommandTimeout = 1800;
                return oCommand;
            }
            catch (Exception ex)
            {
                File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
                oCommand = new SqlCommand();
                return oCommand;
            }

        }

        private SqlCommand PrepareCommand(string sSPName)
        {
            SqlCommand oCommand;

            try
            {
                if (_oConnection == null)
                {
                    _oConnection = new SqlConnection(CONNECTION_STRING);
                }
                if (_oConnection.State == ConnectionState.Closed)
                {
                    _oConnection.Open();
                }
                oCommand = new SqlCommand(sSPName, _oConnection);
                oCommand.CommandType = CommandType.StoredProcedure;
                if (_oTransaciton != null)
                {
                    if (_oTransaciton.Connection != null)
                    {
                        //oCommand.Transaction = _oTransaciton;
                    }
                    else
                    {
                        throw new Exception("Invalid Transaction");
                    }
                }
                oCommand.CommandTimeout = 1800;
            }
            catch (SqlException ex)
            {
                File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
                oCommand = new SqlCommand();
            }
            return oCommand;
        }

        #endregion

        #region ExecuteReader
        /// <summary>
        /// Executes a Oracle and returns row(s) in OracleDataReader
        /// </summary>
        /// <param name="sOracle">Cotains the T-Oracle</param>
        /// <returns>Returns a OracleDataReader that contains the result of the query string</returns>
        protected SqlDataReader ExecuteReader(string sOracle)
        {
            SqlCommand oCommand = new SqlCommand();
            string sErrorMessage;
            int nErrCount = 0;
            SqlDataReader oReader = null;
            try
            {
                oCommand = PrepareCommand(sOracle);
                oReader = oCommand.ExecuteReader();
            }
            catch (SqlException oOracleException)
            {

                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }
                throw new Exception("Oracle ERROR: " + sErrorMessage);
            }            
            if (_oConnection != null)
            {
                if (_oConnection.State == ConnectionState.Open)
                {
                    _oConnection.Close();
                }
            }

            oCommand.Dispose();
            return oReader;
        }        

        /// <summary>
        /// Executes a Stored Procedure and returns row(s) in OracleDataReader
        /// </summary>
        /// <param name="sSPName">Contains The name of Stored Procedure to be executed</param>
        /// <param name="oParameters">Cotains the parameters of Stored Procedure</param>
        /// <returns>Returns a OracleDataReader that contains the result of the query string</returns>
        protected SqlDataReader ExecuteReader(string sSPName,string sCommandType, params SqlParameter[] oParameters)
        {
            SqlCommand oCommand = new SqlCommand();
            string sErrorMessage;
            int nErrCount = 0;
            SqlDataReader oReader = null;
            try
            {
                oCommand = PrepareCommand(sSPName, sCommandType,oParameters);
                oReader = oCommand.ExecuteReader();
            }
            catch (SqlException oOracleException)
            {

                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }
                throw new Exception("Oracle ERROR: " + sErrorMessage);
            }
            if (_oConnection != null)
            {
                if (_oConnection.State == ConnectionState.Open)
                {
                    _oConnection.Close();
                }
            }

            oCommand.Dispose();
            return oReader;
        }
        #endregion

        #region FillDataSet & FillDataTable
        /// <summary>
        /// Executes a Oracle and returns a dataset
        /// </summary>
        /// <param name="sOracle">Cotains the T-Oracle</param>
        /// <param name="sTableName">Contains The Table name</param>
        /// <returns>Returns a OracleDataSet that contains the result of the query string</returns>
        protected DataSet FillDataSet(string sOracle, string sTableName)
        {
            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            DataSet oSet = new DataSet();
            try
            {
                oAdapter.SelectCommand = PrepareCommand(sOracle);
                oAdapter.Fill(oSet, sTableName);
            }
            catch (SqlException oOracleException)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }
                throw new Exception("Oracle ERROR: " + sErrorMessage);
            }
            oAdapter.Dispose();
            return oSet;
        }
        
        /// <summary>
        /// Executes a Stored Procedure and returns a dataset
        /// </summary>        
        /// <param name="sTableName">Contains The Table name</param>
        /// <param name="sSPName">Contains The name of Stored Procedure</param>
        /// <param name="oParameters">Contains The Parameters of Stored Procedure</param>
        /// <returns>Returns a OracleDataSet that contains the result of the query string</returns>
        protected DataSet FillDataSet(string sTableName, string sSPName, params SqlParameter[] oParameters)
        {
            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            DataSet oSet = new DataSet();
            try
            {
                oAdapter.SelectCommand = PrepareCommand(sSPName, "SP",oParameters);
                oAdapter.Fill(oSet, sTableName);
            }
            catch (SqlException oOracleException)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }
                throw new Exception("Oracle ERROR: " + sErrorMessage);
            }
            oAdapter.Dispose();
            return oSet;
        }


        /// <summary>
        /// Executes a Stored Procedure and returns a DataTable
        /// </summary>        
        /// <param name="sTableName">Contains The Table name</param>
        /// <param name="sSPName">Contains The name of Stored Procedure</param>
        /// <param name="oParameters">Contains The Parameters of Stored Procedure</param>
        /// <returns>Returns a DataTable that contains the result of the query string</returns>
        protected DataTable FillDataTable(string sTableName, string sSPName, params SqlParameter[] oParameters)
        {
            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            DataTable oTable = new DataTable(sTableName);
            try
            {
                oAdapter.SelectCommand = PrepareCommand(sSPName,"SP", oParameters);
                oAdapter.Fill(oTable);
            }
            catch (SqlException oOracleException)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }
                throw new Exception("Oracle ERROR: " + sErrorMessage);
            }
            oAdapter.Dispose();
            return oTable;
        }

        protected DataTable ExecuteCommand(string strSql)
        {

            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            SqlCommand oSqlCommand;
            DataTable oDataTable = new DataTable();
            try
            {

                if (_oConnection == null)
                {
                    _oConnection = new SqlConnection(CONNECTION_STRING);
                }
                if (_oConnection.State == ConnectionState.Closed)
                {
                    _oConnection.Open();
                }
                oSqlCommand = new SqlCommand(strSql, _oConnection);
               
                if (_oTransaciton != null)
                {
                    if (_oTransaciton.Connection != null)
                    {
                        if (_oTransaciton.Connection.State == ConnectionState.Closed)
                            _oTransaciton.Connection.Open();

                        oSqlCommand.Transaction = _oTransaciton;
                    }
                    else
                    {
                        throw new Exception("Invalid Transaction");
                    }
                }
                oAdapter.SelectCommand = oSqlCommand;
                oAdapter.Fill(oDataTable);
            }
            catch (SqlException oOracleException)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }

                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }
                throw new Exception("Oracle ERROR: " + sErrorMessage);
            }
            if (_oConnection != null)
            {
                if (_oConnection.State == ConnectionState.Open && _oTransaciton == null)
                {
                    _oConnection.Close();
                }
            }
            oAdapter.Dispose();
            return oDataTable;
        }

        protected DataTable ExecuteSelectQuery(string strQuery, SqlParameter[] oParameters)
        {

            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            DataTable oDataTable = new DataTable();
            try
            {
                oAdapter.SelectCommand = PrepareCommandForQuery(strQuery, oParameters);
                oAdapter.Fill(oDataTable);
            }
            catch (SqlException ex)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
                File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
            }

            if (_oConnection != null)
            {
                if (_oConnection.State == ConnectionState.Open)
                {
                    _oConnection.Close();
                }
            }
            oAdapter.Dispose();
            return oDataTable;
        }

        protected DataTable ExecuteSelectCommand(string strProcedureName, SqlParameter[] oParameters)
        {

            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            DataTable oDataTable = new DataTable();
            try
            {
                oAdapter.SelectCommand = PrepareCommand(strProcedureName,"SP", oParameters);
                oAdapter.Fill(oDataTable);
            }
            catch (SqlException ex)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
                File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
            }

            if (_oConnection != null)
            {
                if (_oConnection.State == ConnectionState.Open)
                {
                    _oConnection.Close();
                }
            }
            oAdapter.Dispose();
            return oDataTable;
        }

        protected DataTable ExecuteSelectCommand(string strProcedureName)
        {

            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            DataTable oDataTable = new DataTable();
            try
            {
                oAdapter.SelectCommand = PrepareCommand(strProcedureName);
                oAdapter.Fill(oDataTable);
            }
            catch (SqlException ex)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
                File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
            }

            if (_oConnection != null)
            {
                if (_oConnection.State == ConnectionState.Open)
                {
                    _oConnection.Close();
                }
            }
            oAdapter.Dispose();
            return oDataTable;
        }

        protected int ExecuteScalar(string sQuery, params SqlParameter[] oParameters)
        {
            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            int nIsSuccess = 0;

            try
            {
                oAdapter.InsertCommand = PrepareCommandForQuery(sQuery, oParameters);
                nIsSuccess =int.Parse( oAdapter.InsertCommand.ExecuteScalar().ToString());

                //nIsSuccess = 1;
            }
            catch (SqlException ex)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
               
                File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
                nIsSuccess = -1;
            }
            oAdapter.Dispose();
            return nIsSuccess;
        }

        /// <summary>
        /// Executes a Stored Procedure and returns a DataTable
        /// </summary>        
        /// <param name="sTableName">Contains The Table name</param>
        /// <param name="sSPName">Contains The name of Stored Procedure</param>
        /// <param name="oParameters">Contains The Parameters of Stored Procedure</param>
        /// <returns>Returns a integer value that contains wheather the command was executed or not</returns>
        
        protected int InsertDataSP(string sTableName, string sSPName, params SqlParameter[] oParameters)
        {
            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            int nIsSuccess = 0;
            DataTable oTable = new DataTable(sTableName);
            try
            {
                oAdapter.SelectCommand = PrepareCommand(sSPName,"SP" , oParameters);
                oAdapter.SelectCommand.ExecuteNonQuery();
                nIsSuccess = 1;
            }
            catch (SqlException oOracleException)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }

                //LIASLOG.Debug("DML:: " + "InsertData(string sTableName, string sSPName, params SqlParameter[] oParameters):: " + oOracleException.Message);
                nIsSuccess = -1;
                //throw new Exception("Oracle ERROR: " + sErrorMessage);
            }
            oAdapter.Dispose();
            return nIsSuccess;
        }

        /// <summary>
        /// Insert/Update/Delete using direct query
        /// </summary>        
        /// <param name="sQuery">Contains The Query</param>        
        /// <param name="oParameters">Contains The Parameters of The Query</param>
        /// <returns>Returns a integer value that contains wheather the command was executed or not</returns>

        protected int InsertData(string sQuery, params SqlParameter[] oParameters)
        {
            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            int nIsSuccess = 0;
            
            try
            {                
                oAdapter.InsertCommand = PrepareCommandForQuery(sQuery, oParameters);
                nIsSuccess = oAdapter.InsertCommand.ExecuteNonQuery();        
            }
            catch (SqlException ex)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
                File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
                nIsSuccess = -1;
            }
            oAdapter.Dispose();
            return nIsSuccess;
        }

        /// <summary>
        /// Executes a Stored Procedure and returns a DataTable
        /// </summary>        
        /// <param name="sTableName">Contains The Table name</param>
        /// <param name="sSPName">Contains The name of Stored Procedure</param>
        /// <returns>Returns a DataTable that contains the result of the query string</returns>
        protected DataTable FillDataTable(string sTableName, string sSPName)
        {
            SqlDataAdapter oAdapter = new SqlDataAdapter();
            string sErrorMessage;
            int nErrCount = 0;
            DataTable oTable = new DataTable(sTableName);
            try
            {
                oAdapter.SelectCommand = PrepareCommand(sSPName);
                oAdapter.Fill(oTable);
            }
            catch (SqlException oOracleException)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }
                throw new Exception("Oracle ERROR: " + sErrorMessage);
            }
            oAdapter.Dispose();
            return oTable;
        }



        /// <summary>
        /// Executes a Stored Procedure and returns a DataReader
        /// </summary>        
        /// <param name="sTableName">Contains The Table name</param>
        /// <param name="sSPName">Contains The name of Stored Procedure</param>
        /// <returns>Returns a DataReader that contains the result of the query string</returns>
        protected SqlDataReader FillList(string sTableName, string sSPName)
        {
            SqlDataAdapter oAdapter = new SqlDataAdapter();
            SqlCommand objCommand = new SqlCommand();
            string sErrorMessage;
            int nErrCount = 0;
            SqlDataReader oDataReader;
            try
            {
                objCommand = PrepareCommand(sSPName);
                oDataReader = objCommand.ExecuteReader();
                oDataReader.Close();
                //oAdapter.Fill(oTable);
            }
            catch (SqlException oOracleException)
            {
                if (_oConnection != null)
                {
                    if (_oConnection.State == ConnectionState.Open)
                    {
                        _oConnection.Close();
                    }
                }
                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }
                throw new Exception("Oracle ERROR: " + sErrorMessage);
            }
            oAdapter.Dispose();
            return oDataReader;
        }
        #endregion

        #region Transaction

        #region Begin Tran
        /// <summary>
        /// It checks for an already open transaction. if there exist an open connection, then
        /// the function throws an exception. It opens a connection, if it is already closed
        /// If the connection property of the transaction is null then the transaction is no
        /// longer valid and it opens a new connection and a transaction
        /// </summary>
        public static void BeginTran()
        {
            if (_oTransaciton == null)
            {
                if (_oConnection == null)
                {
                    _oConnection = new SqlConnection(CONNECTION_STRING);
                }
                if (_oConnection.State == ConnectionState.Closed)
                {
                    _oConnection.Open();
                }
                _oTransaciton = _oConnection.BeginTransaction();
            }
            else
            {
                if (_oTransaciton.Connection != null)
                {
                    if (_oTransaciton.Connection.State == ConnectionState.Open)
                    {
                        _oTransaciton.Connection.Close();
                        _oTransaciton = null;
                        throw new Exception("Cannot start a new transaction, already when a new transaction is open");
                    }
                }
                else
                {
                    _oConnection = new SqlConnection(CONNECTION_STRING);
                    _oConnection.Open();
                    _oTransaciton = _oConnection.BeginTransaction();
                }
            }
        }
        #endregion

        #region CommitTran
        /// <summary>
        /// At first it checks whether there is any transaction (that is not null). If there is
        /// none then it throws an exception.If the connection object of the transaction 
        /// is null then transaction is no longer valid.
        /// </summary>
        public static void CommitTran()
        {
            if (_oTransaciton == null)
            {
                throw new Exception("Nothing to COMMIT. No transaction exist");
            }
            else
            {
                if (_oTransaciton.Connection == null)
                {
                    throw new Exception("No Connection Open");
                }
                else
                {
                    _oTransaciton.Commit();
                    _oTransaciton.Dispose();
                    _oTransaciton = null;
                }
            }
            if (_oConnection.State != ConnectionState.Closed)
            {
                _oConnection.Close();

            }
        }
        #endregion

        #region RollBack Tran
        public static void RollBackTran()
        {
            if (_oTransaciton != null)
            {
                if (_oTransaciton.Connection != null)
                {
                    _oTransaciton.Rollback();
                    _oTransaciton = null;
                }
            }
            if (_oConnection.State != ConnectionState.Closed)
            {
                _oConnection.Close();
            }
            _oTransaciton = null;
        }
        #endregion

        #endregion

        #region Execute StoreProcedure

        /// <summary>
        /// Executes Stored Procedure
        /// </summary>
        /// <param name="sSPName">Name of The Stored Procedure</param>
        /// <param name="oParameters">Parameters of The Stored Procedure</param>
        protected int ExecuteStoreProcedure(string sSPName, params SqlParameter[] oParameters)
        {
            SqlCommand oCommand = new SqlCommand();
            string sErrorMessage;
            int nErrCount = 0;
            int nIsSuccess = 0;

            try
            {
                oCommand = PrepareCommand(sSPName,"SP", oParameters);
                oCommand.ExecuteNonQuery();
                nIsSuccess = oParameters[0].Direction == ParameterDirection.Output ? Convert.ToInt32(oParameters[0].Value) : 1;
            }
            catch (SqlException oOracleException)
            {
                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }

                //LIASLOG.Debug("DML:: " + "ExecuteStoreProcedure(string sSPName, params SqlParameter[] oParameters):  " + oOracleException.Message);
                throw new Exception("Oracle ERROR: " + sErrorMessage);
            }

            oCommand.Dispose();
            return nIsSuccess;
        }

        protected double ExecuteStoreProcedureDouble(string sSPName, params SqlParameter[] oParameters)
        {
            SqlCommand oCommand = new SqlCommand();
            string sErrorMessage;
            double nErrCount = 0;
            double nIsSuccess = 0;

            try
            {
                oCommand = PrepareCommand(sSPName, "SP", oParameters);
                oCommand.ExecuteNonQuery();
                nIsSuccess = oParameters[0].Direction == ParameterDirection.Output ? Convert.ToDouble(oParameters[0].Value) : 1;
            }
            catch (SqlException oOracleException)
            {
                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }

               // LIASLOG.Debug("DML:: " + "ExecuteStoreProcedureDouble(string sSPName, params SqlParameter[] oParameters):  " + oOracleException.Message);
                throw new Exception("Oracle ERROR: " + sErrorMessage);
            }

            oCommand.Dispose();
            return nIsSuccess;
        }

        protected bool ExecuteStoreProcedureWithoutReturn(string sSPName, params SqlParameter[] oParameters)
        {
            SqlCommand oCommand = new SqlCommand();
            string sErrorMessage;
            int nErrCount = 0;
            bool nIsSuccess = false;

            try
            {
                oCommand = PrepareCommand(sSPName, "SP", oParameters);
                oCommand.ExecuteNonQuery();
                nIsSuccess = true;
            }
            catch (SqlException oOracleException)
            {
                nIsSuccess = false;
                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }

                //LIASLOG.Debug("DML:: " + "ExecuteStoreProcedure(string sSPName, params SqlParameter[] oParameters):  " + oOracleException.Message);

            }
            oCommand.Dispose();
            return nIsSuccess;
        }

        protected string ExecuteStoreProcedureReturnString(string sSPName, params SqlParameter[] oParameters)
        {
            SqlCommand oCommand = new SqlCommand();
            string sErrorMessage;
            int nErrCount = 0;
            string nIsSuccess = "";

            try
            {
                oCommand = PrepareCommand(sSPName, "SP", oParameters);
                oCommand.ExecuteNonQuery();

                nIsSuccess = oParameters[0].Direction == ParameterDirection.Output ? oParameters[0].Value.ToString() : "";
            }
            catch (SqlException oOracleException)
            {
                sErrorMessage = "";
                foreach (SqlError oError in oOracleException.Errors)
                {
                    nErrCount++;
                    sErrorMessage = sErrorMessage + nErrCount.ToString() + ". " + oError.ToString();
                }

                //LIASLOG.Debug("DML:: " + "ExecuteStoreProcedureReturnString(string sSPName, params SqlParameter[] oParameters):  " + oOracleException.Message);

            }

            oCommand.Dispose();
            return nIsSuccess;
        }

        #endregion
    }
}
