using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace AdviceLocal.Api.DAL {
    public class Repository {
        //private void BulkCopy(SqlConnection sqlConnection,string tableName, DataTable dataTable)
        public bool BulkCopy<T> (SqlConnection sqlConnection, string tableName, IEnumerable<T> data) {
            sqlConnection.Open ();

            using (SqlTransaction tran = sqlConnection.BeginTransaction (IsolationLevel.ReadCommitted)) {
                using (var bulkCopy = new SqlBulkCopy (sqlConnection, SqlBulkCopyOptions.Default, tran)) {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 10000;
                    bulkCopy.BulkCopyTimeout = 60; //seconds                
                    try {
                        bulkCopy.WriteToServer (AsDataTable<T> (data));
                    } catch (Exception ex) {
                        string errMsg = ex.Message;
                        Console.WriteLine ("DB Error " + errMsg);
                        tran.Rollback ();
                        sqlConnection.Close ();
                        return false;
                    }
                }
                tran.Commit ();
                sqlConnection.Close ();
                return true;
            }
        }

        public DataTable AsDataTable<T> (IEnumerable<T> data) {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties (typeof (T));
            var table = new DataTable ();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add (prop.Name, Nullable.GetUnderlyingType (prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data) {
                DataRow row = table.NewRow ();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue (item) ?? DBNull.Value;
                table.Rows.Add (row);
            }
            return table;
        }

        public bool TruncateTable (SqlConnection sqlConnection, string tableName) {

            sqlConnection.Open ();
            string sqlTrunc = "TRUNCATE TABLE " + tableName;
            using (SqlCommand cmd = new SqlCommand (sqlTrunc, sqlConnection)) {
                try {
                    cmd.ExecuteNonQuery ();
                } catch (Exception) {
                    sqlConnection.Close ();
                    return false;
                }
            }
            sqlConnection.Close ();
            return true;
        }

        public bool RenameTable (SqlConnection sqlConnection) {
            {
                sqlConnection.Open ();
                string sqlTrunc = "dbo.spRenameTable";
                using (SqlCommand cmd = new SqlCommand (sqlTrunc, sqlConnection) {
                    CommandType = CommandType.StoredProcedure
                }) {
                    try {
                        cmd.ExecuteNonQuery ();
                    } catch (Exception) {
                        sqlConnection.Close ();
                        return false;
                    }
                }
                sqlConnection.Close ();
                return true;
            }
        }
    }
}