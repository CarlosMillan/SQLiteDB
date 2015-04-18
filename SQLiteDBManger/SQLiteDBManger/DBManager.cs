using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDBManger
{
    public class DBManager
    {
        //private SQLiteCommand sql_cmd;
        //private DataSet DS = new DataSet();
        //private DataTable DT = new DataTable();
        //private SQLiteDataAdapter DB;        
        private SQLiteConnection _connection = new SQLiteConnection();

        public DBManager(string databasepath)
        {
            _connection.ConnectionString = String.Concat(@"Data Source=", databasepath);
        }

        #region Operations
        /// <summary>
        /// Insert a row.
        /// </summary>
        /// <param name="tablename">Table´s name</param>
        /// <param name="values">Values to insert</param>
        public void Insert(string tablename, params object[] values)
        {
            try
            {
                StringBuilder InsertStatment = new StringBuilder();
                InsertStatment.AppendFormat(@"INSERT INTO {0} VALUES({1})", tablename, String.Join(",", values));
                SQLiteCommand Command = new SQLiteCommand(InsertStatment.ToString(), _connection);
                _connection.Open();
                Command.ExecuteNonQuery();                                
            }
            catch (Exception E)
            {
                throw E;
            }
            finally
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Delete a row or set of them.
        /// </summary>
        /// <param name="tablename">Table´s name</param>
        /// <param name="whereclause">Where clause whitout WHERE keyword</param>        
        public void Delete(string tablename, string whereclause = null) 
        {
            try
            {
                StringBuilder DeleteStatment = new StringBuilder();
                DeleteStatment.AppendFormat(@"DELETE FROM {0}", tablename);

                if (!String.IsNullOrEmpty(whereclause))
                    DeleteStatment.AppendFormat(" WHERE {0}", whereclause);

                SQLiteCommand Command = new SQLiteCommand(DeleteStatment.ToString(), _connection);
                _connection.Open();
                Command.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                throw E;
            }
            finally
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Update a row or set of them.
        /// </summary>
        /// <param name="tablname">Table's name</param>
        /// <param name="values">Key-value pair</param>
        /// <param name="whereclause">Where clause whitout WHERE keyword</param>
        public void Update(string tablename, string whereclause, object[] values) 
        {
            try
            {
                if (General.IsValidKeyValuePair(values))
                {
                    StringBuilder UpdateStatment = new StringBuilder();
                    UpdateStatment.AppendFormat(@"UPDATE {0} SET ", tablename);

                    for (int index = 0; index < values.Length; index++)                    
                        UpdateStatment.AppendFormat(@"{0} = '{1}',", values[index++], values[index]);

                    UpdateStatment = UpdateStatment.Remove(UpdateStatment.Length - 1, 1);

                    if (!String.IsNullOrEmpty(whereclause))                    
                        UpdateStatment.AppendFormat(" WHERE {0}", whereclause);

                    SQLiteCommand Command = new SQLiteCommand(UpdateStatment.ToString(), _connection);
                    _connection.Open();
                    Command.ExecuteNonQuery();
                }
                else
                    throw new Exception("Values field must have key-value elements.");
            }
            catch(Exception E)
            {
                throw E;
            }
            finally
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Update a row or set of them.
        /// </summary>
        /// <param name="tablname">Table's name</param>
        /// <param name="values">Key-value pair</param>        
        public void Update(string tablename, object[] values)
        {
            Update(tablename, null, values); 
        }

        /// <summary>
        /// Gets a table.
        /// </summary>
        /// <param name="tablename">Table's name</param>
        /// <param name="whereclause">Where clause whitout WHERE keyword</param>
        /// <param name="fields">Fields to query</param>
        /// <returns>Data result</returns>
        public DataTable GetTable(string tablename, string whereclause, params object[] fields) 
        {
            DataTable Result = new DataTable();

            try
            {
                string ToGet = String.Join(",", fields);
                StringBuilder SelectStatment = new StringBuilder();
                SelectStatment.AppendFormat(@"SELECT {0} FROM {1} {2}"
                                             , String.IsNullOrEmpty(ToGet) ? "*" : ToGet
                                             , tablename
                                             , String.IsNullOrEmpty(whereclause) ? string.Empty : whereclause);
                SQLiteCommand Command = new SQLiteCommand(SelectStatment.ToString().Trim(), _connection);                
                SQLiteDataAdapter SQLiteAdapter = new SQLiteDataAdapter(Command);
                _connection.Open();
                SQLiteAdapter.Fill(Result);                
            }
            catch(Exception E)
            {
                throw E;
            }
            finally
            {
                _connection.Close();
            }

            return Result;
        }

        /// <summary>
        /// Gets a table.
        /// </summary>
        /// <param name="tablename">Table's name</param>        
        /// <param name="fields">Fields to query</param>
        /// <returns>Data result</returns>
        public DataTable GetTable(string tablename, params object[] fields)
        {
            return GetTable(tablename, null, fields);
        }

        /// <summary>
        /// Get a value from table
        /// </summary>
        /// <param name="tablename">Table´s name</param>
        /// <param name="field">Field to query</param>
        /// <param name="whereclause">Where clause whitout WHERE keyword</param>
        /// <returns>Object result</returns>
        public object GetValue(string tablename, string field, string whereclause = null)
        {
            object Result = null;

            try
            {
                StringBuilder SelectStatment = new StringBuilder();
                SelectStatment.AppendFormat(@"SELECT {0} FROM {1} {2}", field, tablename, String.IsNullOrEmpty(whereclause) ? string.Empty : whereclause);
                SQLiteCommand Command = new SQLiteCommand(SelectStatment.ToString(), _connection);                
                _connection.Open();
                Result = Command.ExecuteScalar();                
            }
            catch (Exception E)
            {
                throw E;
            }
            finally
            {
                _connection.Close();
            }

            return Result;
        }

        public DataTable GetTableFromQuery(string statment)
        {
            DataTable Result = new DataTable();

            try
            {
                SQLiteCommand Command = new SQLiteCommand(statment, _connection);
                SQLiteDataAdapter SQLiteAdapter = new SQLiteDataAdapter(Command);
                _connection.Open();
                SQLiteAdapter.Fill(Result);
            }
            catch (Exception E)
            {
                throw E;
            }
            finally
            {
                _connection.Close();
            }

            return Result;
        }

        public void ExecuteQuery(string statment)
        {
            try
            {
                SQLiteCommand Command = new SQLiteCommand(statment, _connection);
                _connection.Open();
                Command.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                throw E;
            }
            finally
            {
                _connection.Close();
            }
        }
        #endregion
    }
}
