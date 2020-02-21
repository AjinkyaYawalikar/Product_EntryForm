
#region LIBRARIES
using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Configuration;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
#endregion

namespace Product_Entry_Demo.Library.DAL
{
    public class DalBase
    {
        #region VARS
        protected MySqlConnection _dataConnection;
        protected MySqlCommand _dataCommand;

        protected string _connectString;

        private ArrayList Parms = new ArrayList();
        //private int _currentUserID = -1;

        //public int _userID;
        // private Logger _logger = Logger.getInstance();
        public readonly System.Data.SqlTypes.SqlDateTime DateTimeNull = System.Data.SqlTypes.SqlDateTime.Null;
        #endregion

        #region CONSTRUCTORS
        public DalBase()
        {
            try
            {
                _connectString = GetConnectionStr();  //ConfigHelper.Get("ConnectionString");

                _dataConnection = new MySqlConnection(_connectString);    // Create a connection to the database
                _dataCommand = _dataConnection.CreateCommand();         // Create sql Command and attach to connection

                _dataCommand.CommandTimeout = 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["dbproduct_entry"].ToString();// ConfigurationSettings.AppSettings.Get("ConnectionString");
        }

        #endregion

        #region TRANSACTIONS

        public void TransactionStart()

        // Starts a new transaction block
        {
            //TransactionScope txnScope = new TransactionScope();
        }


        public void TransactionComplete()
        {
            //txnScope.Complete();
            //txnScope.Dispose();
        }


        public void TransactionRollback()

        // Called from a catch block to release the transaction with out commit.
        {
            //txnScope.Dispose();
        }
        #endregion

        #region PARAMETERS

        protected void AddParameter(string ParmName, MySqlDbType A_DbType, object oValue)
        {
            if (oValue == null)
                oValue = DBNull.Value;


            MySqlParameter oPm = new MySqlParameter(ParmName, A_DbType);
            oPm.Value = oValue;

            Parms.Add(oPm);
        }

        
        protected void AddOutputParameter(string ParmName, SqlDbType A_dbtype, object oValue)
        {
            if (oValue == null)
                oValue = DBNull.Value;

            MySqlParameter oPm = new MySqlParameter(ParmName, A_dbtype);
            oPm.Value = oValue;
            oPm.Direction = ParameterDirection.Output;

            Parms.Add(oPm);
        }


        /// <summary>
        /// Allows for direction parameter
        /// </summary>
        /// <param name="ParmName"></param>
        /// <param name="A_DbType"></param>
        /// <param name="oValue"></param>
        /// <param name="pd"></param>
        protected void AddParameter(string ParmName, SqlDbType A_DbType, object oValue, ParameterDirection pd, int size)
        {
            if (oValue == null)
                oValue = DBNull.Value;

            MySqlParameter oPm = new MySqlParameter(ParmName, A_DbType);
            oPm.Value = oValue;
            oPm.Direction = ParameterDirection.Input;
            oPm.Direction = pd;
            oPm.Size = size;
            Parms.Add(oPm);
        }

        #endregion

        #region DATA_ACCESS
        public DataSet ExecuteForData(string sStoredProcedureName)
        {
            DataSet ds = new DataSet();

            try
            {
                _dataConnection.Open();
                //Call Stored Procedure
                _dataCommand.CommandText = sStoredProcedureName;
                _dataCommand.CommandType = CommandType.StoredProcedure;

                // Add in the parameters
                for (int i = 0; i < Parms.Count; i++)
                    _dataCommand.Parameters.Add((MySqlParameter)Parms[i]);

                MySqlDataAdapter adapter = new MySqlDataAdapter(_dataCommand);
                adapter.Fill(ds);

                // string z = ds.GetXml();
                //                    txnScope.Complete();
                //                }
            }
            catch (Exception ex)
            {
                ds = null;
                throw ex;
            }
            finally
            {
                //close the connection
                _dataConnection.Close();


                _dataCommand.Parameters.Clear();            // Remove saved parms
                Parms.Clear();
            }

            return ds;
        }
        protected SmartDataReader ExecuteForDataReader(string StoredProcedureName)
        {
            MySqlDataReader sdr = null;

            try
            {
                _dataConnection.Open();
                _dataCommand.CommandText = StoredProcedureName;
                _dataCommand.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < Parms.Count; i++)
                {
                    _dataCommand.Parameters.Add((MySqlParameter)Parms[i]);
                }

                sdr = _dataCommand.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ClearParms();
            }

            return new SmartDataReader(sdr);
        }

        protected MySqlDataReader ExecuteDataReader(string StoredProcedureName)
        {
            MySqlDataReader sdr = null;

            try
            {
                _dataConnection.Open();

                _dataCommand.CommandText = StoredProcedureName;
                _dataCommand.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < Parms.Count; i++)
                {
                    _dataCommand.Parameters.Add((MySqlParameter)Parms[i]);
                }

                sdr = _dataCommand.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ClearParms();
            }

            return sdr;
        }
        
        public void CloseConnection()
        {
            if (_dataConnection != null)
                _dataConnection.Close();
        }
        
        internal object ExecuteScalar(string sStoredProcedureName)
        {
            //  Output Parameter should be last parameter entered.
            //  Returns Integer type.
            object oRetVal = 0;

            try
            {
              
                _dataConnection.Open();

                //Call Stored Procedure
                _dataCommand.CommandText = sStoredProcedureName;
                _dataCommand.CommandType = CommandType.StoredProcedure;

                // Add in the parameters
                for (int i = 0; i < Parms.Count; i++)
                {
                    _dataCommand.Parameters.Add((MySqlParameter)Parms[i]);
                }

                oRetVal = _dataCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //close the connection
                _dataConnection.Close();

                ClearParms();
            }

            return oRetVal;
        }
        
        internal object ExecuteScalarOutput(string sStoredProcedureName)
        {
            //  Output Parameter should be last parameter entered.
            //  Returns Integer type.
            object oRetVal = 0;

            try
            {
               
                _dataConnection.Open();

                //Call Stored Procedure
                _dataCommand.CommandText = sStoredProcedureName;
                _dataCommand.CommandType = CommandType.StoredProcedure;

                // Add in the parameters
                for (int i = 0; i < Parms.Count; i++)
                {
                    _dataCommand.Parameters.Add((MySqlParameter)Parms[i]);
                }

                _dataCommand.ExecuteNonQuery();

                oRetVal = _dataCommand.Parameters[_dataCommand.Parameters.Count - 1].Value;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //close the connection
                _dataConnection.Close();

                ClearParms();
            }

            return oRetVal;
        }
        
        public void ClearParms()
        {
            _dataCommand.Parameters.Clear();
            Parms.Clear();
        }

        internal object ExecuteNonQueryWithParm(string sStoredProcedureName)
        {
          
            object oRetVal = 0;

            try
            {               
                _dataConnection.Open();
                //Call Stored Procedure
                _dataCommand.CommandText = sStoredProcedureName;
                _dataCommand.CommandType = CommandType.StoredProcedure;

                // Add in the parameters
                for (int i = 0; i < Parms.Count; i++)
                {
                    _dataCommand.Parameters.Add((MySqlParameter)Parms[i]);
                }
                
                oRetVal = _dataCommand.ExecuteScalar();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //close the connection
                _dataConnection.Close();

                ClearParms();
            }

            return oRetVal;
        }
        
        public bool ExecuteDynamicSQL(string sql)
        {
            try
            {
                _dataConnection.Open();

                _dataCommand.CommandText = sql;
                _dataCommand.CommandType = CommandType.Text;

                _dataCommand.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close the connection
                _dataConnection.Close();

                ClearParms();
            }
            return true;
        }

        internal bool ExecuteNonQuery(string sStoredProcedureName)
        {
            bool bSuccess = false;

            try
            {
                _dataConnection.Open();

                //Call Stored Procedure
                _dataCommand.CommandText = sStoredProcedureName;
                _dataCommand.CommandType = CommandType.StoredProcedure;

                // Add in the parameters
                for (int i = 0; i < Parms.Count; i++)
                {
                    _dataCommand.Parameters.Add((MySqlParameter)Parms[i]);
                }

                _dataCommand.ExecuteNonQuery();

                bSuccess = true;

            }
            catch (Exception ex)
            {
                bSuccess = false;
                throw ex;
            }
            finally
            {
                //close the connection
                _dataConnection.Close();

                ClearParms();
            }

            return bSuccess;
        }

        /// <summary>
        /// Returns a Data Table from query
        /// </summary>
        /// <param name="sproc"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sproc)
        {
            DataTable dt = new DataTable();

            try
            {

                _dataConnection.Open();
                //Call Stored Procedure
                _dataCommand.CommandText = sproc;
                _dataCommand.CommandType = CommandType.StoredProcedure;

                // Add in the parameters
                for (int i = 0; i < Parms.Count; i++)
                    _dataCommand.Parameters.Add((MySqlParameter)Parms[i]);

                MySqlDataAdapter adapter = new MySqlDataAdapter(_dataCommand);
                adapter.Fill(dt);


            }
            catch (Exception ex)
            {
                dt = null;
                throw ex;

            }
            finally
            {
                //close the connection
                _dataConnection.Close();


                _dataCommand.Parameters.Clear();            // Remove saved parms
                Parms.Clear();
            }

            return dt;
        }
        #endregion

       

        #region DATAREADER

        public sealed class SmartDataReader
        {
            #region DATAREADER_VARS
            private DateTime defaultDate;
            MySqlDataReader _reader;
            #endregion

            #region DATAREADER_CONSTRUCTOR
            public SmartDataReader(MySqlDataReader reader)
            {
                this.defaultDate = DateTime.MinValue;
                _reader = reader;
            }
            #endregion

            #region DATA_ACCESSORS
            public int GetInt32(String column)
            {
                try
                {
                    return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                                            ? (int)0 : (int)_reader[column];

                }
                catch (Exception ex)
                {
                    throw ex;
                    //ExceptionLogging.SendErrorToText(ex, "GetInt32", "BitdojiLibrary.Dal.DALBase");
                }

                return 0;
            }

            public short GetInt16(String column)
            {
                try
                {
                    return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                                           ? (short)0 : (short)_reader[column];
                }
                catch //(Exception ex)
                {
#if DEBUG2              
                     _lg.WriteLog(ex.Message, Logger.MsgType.Error);
#endif
                }
                return 0;
            }

            public float GetFloat(String column)
            {
                try
                {
                    return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                                ? 0 : float.Parse(_reader[column].ToString());
                }
                catch //(Exception ex)
                {
#if DEBUG2              
                   _lg.WriteLog(ex.Message, Logger.MsgType.Error);
#endif
                }
                return 0;
            }

            public double GetDouble(String column)
            {
                try
                {
                    return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                                ? 0 : double.Parse(_reader[column].ToString());
                }
                catch //(Exception ex)
                {
#if DEBUG2              
                    _lg.WriteLog(ex.Message, Logger.MsgType.Error);
#endif
                }
                return 0;
            }

            public decimal GetDecimal(String column)
            {
                try
                {
                    return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                                ? 0 : decimal.Parse(_reader[column].ToString());
                }
                catch //(Exception ex)
                {
#if DEBUG2
                    _lg.WriteLog(ex.Message, Logger.MsgType.Error);
#endif
                }
                return 0;
            }

            public bool GetBoolean(String column)
            {
                try
                {
                    return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                                              ? false : (bool)_reader[column];
                }
                catch //(Exception ex)
                {

                    try
                    {
                        return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                                             ? false : Convert.ToBoolean(_reader[column]);
                    }
                    catch //(Exception ex)
                    {
#if DEBUG2              
                  // _lg.WriteLog(ex.Message, Logger.MsgType.Error);
#endif
                    }

                }
                return false;
            }

            public String GetString(String column)
            {
                try
                {
                    return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                                       ? null : _reader[column].ToString();
                }
                catch //(Exception ex)
                {
#if DEBUG2
                   _lg.WriteLog(ex.Message, Logger.MsgType.Error);
#endif
                }
                return null;
            }

            public DateTime GetDateTime(String column)
            {
                try
                {
                    return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                                   ? defaultDate : (DateTime)_reader[column];
                }
                catch //(Exception ex)
                {
#if DEBUG2              
                    _lg.WriteLog(ex.Message, Logger.MsgType.Error);
#endif
                }
                return DateTime.Parse("1/1/1900");
            }

            public Decimal GetMoney(String column)
            {
                try
                {
                    if (!_reader.IsDBNull(_reader.GetOrdinal(column)))
                        return Convert.ToDecimal(_reader[column]);
                   return (decimal)0;
                    
                }
                catch  (Exception ex)
                {
#if DEBUG2              
                   _lg.WriteLog(ex.Message, Logger.MsgType.Error);
#endif
                }
                return 0;
            }
            public byte[] GetBytes(String column)
            {
                try
                {
                    return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                                ? null : ((byte[])_reader[column]);
                }
                catch (Exception ex)
                {
#if DEBUG2              
                    _lg.WriteLog(ex.Message, Logger.MsgType.Error);
#endif
                }
                return null;
            }

            #endregion

            #region DATAREADER_ACCESS
            public bool Read()
            {
                return _reader.Read();
            }

            public bool HasRows()
            {
                return _reader.HasRows;
            }

            public bool NextResult()
            {
                return _reader.NextResult();
            }

            public void Close()
            {
                _reader.Close();
                _reader.Dispose();
            }
            #endregion
        }

        #endregion

        #region STATIC METHODS
      
        public static DataSet sGetDataSet(string sproc)
        {
            MySqlConnection DataConnection = null;
            MySqlCommand DataCommand = null;
            MySqlDataAdapter Adapter = null;
            MySqlDataReader Reader = null;
            DataSet results = new DataSet();

            try
            {
                DataConnection = new MySqlConnection(GetConnectionStr());     // Gets the Data Conn String 1st and then the actual db connection obj

                DataCommand = DataConnection.CreateCommand();

                DataConnection.Open();
                DataCommand.CommandText = sproc;
                DataCommand.CommandType = CommandType.StoredProcedure;

                Adapter = new MySqlDataAdapter(DataCommand);
                Adapter.Fill(results);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                try
                {
                    Reader.Close();
                    Reader.Dispose();
                }
                catch { }

                try
                {
                    DataCommand.Dispose();
                }
                catch { }

                try
                {
                    DataConnection.Close();
                    DataConnection.Dispose();
                }
                catch { }
            }

            return results;
        }
        public static DataTable sGetDataTable(string sproc)
        {
            MySqlConnection DataConnection = null;
            MySqlCommand DataCommand = null;
            MySqlDataAdapter Adapter = null;
            MySqlDataReader Reader = null;
            DataTable results = new DataTable();

            try
            {
                DataConnection = new MySqlConnection(GetConnectionStr());     // Gets the Data Conn String 1st and then the actual db connection obj

                DataCommand = DataConnection.CreateCommand();

                DataConnection.Open();
                DataCommand.CommandText = sproc;
                DataCommand.CommandType = CommandType.StoredProcedure;

                Adapter = new MySqlDataAdapter(DataCommand);
                Adapter.Fill(results);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    Reader.Close();
                    Reader.Dispose();
                }
                catch { }

                try
                {
                    DataCommand.Dispose();
                }
                catch { }

                try
                {
                    DataConnection.Close();
                    DataConnection.Dispose();
                }
                catch { }
            }

            return results;
        }
        
        public static DataTable sGetDataTableDynamic(string SQL)
        {
            MySqlConnection DataConnection = null;
            MySqlCommand DataCommand = null;
            MySqlDataAdapter Adapter = null;
            MySqlDataReader Reader = null;
            DataTable results = new DataTable();

            try
            {
                DataConnection = new MySqlConnection(GetConnectionStr());
                DataCommand = DataConnection.CreateCommand();
                DataCommand.CommandTimeout = 3600;
                DataConnection.Open();
                DataCommand.CommandText = SQL;
                DataCommand.CommandType = CommandType.Text;

                Adapter = new MySqlDataAdapter(DataCommand);
                Adapter.Fill(results);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    Reader.Close();
                    Reader.Dispose();
                }
                catch { }

                try
                {
                    DataCommand.Dispose();
                }
                catch { }

                try
                {
                    DataConnection.Close();
                    DataConnection.Dispose();
                }
                catch { }
            }

            return results;
        }
        
        public static void sExectuteNonQueryDynamic(string SQL)
        {
            MySqlConnection DataConnection = null;
            MySqlCommand dcmd = null;

            try
            {
                DataConnection = new MySqlConnection(GetConnectionStr());

                dcmd = DataConnection.CreateCommand();

                DataConnection.Open();
                dcmd.CommandText = SQL;
                dcmd.CommandType = CommandType.Text;

                dcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
                // ExceptionLogging.SendErrorToText(ex, "sExectuteNonQueryDynamic", "BitdojiLibrary.Dal.DALBase");
            }
            finally
            {

                try
                {
                    dcmd.Dispose();
                }
                catch { }

                try
                {
                    DataConnection.Close();
                    DataConnection.Dispose();
                }
                catch { }
            }

        }
        public static object sExecuteScalarDynamic(string SQL)
        {
            MySqlConnection DataConnection = null;
            MySqlCommand dcmd = null;

            try
            {
                DataConnection = new MySqlConnection(GetConnectionStr());

                dcmd = DataConnection.CreateCommand();

                DataConnection.Open();
                dcmd.CommandText = SQL;
                dcmd.CommandType = CommandType.Text;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                try
                {
                    dcmd.Dispose();
                }
                catch { }

                try
                {
                    DataConnection.Close();
                    DataConnection.Dispose();
                }
                catch { }
            }
            return dcmd.ExecuteScalar();
        }


        #endregion

        #region METHOD
        private static DataTable CreateDataTable<T>(IList<T> obj)
        {
            DataTable table = new DataTable();
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(T).GetProperties();
            foreach (var prop in propertyInfos)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var id in obj)
            {
                table.Rows.Add(id);
            }
            return table;
        }
        #endregion

    }
}


