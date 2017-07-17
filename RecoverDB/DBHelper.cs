using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace RecoverDB
{
    public class DBHelper
    {
        private string _conStr;
        public DBHelper(string conStr)
        {
            _conStr = conStr;
        }
        public List<TableStructure> GetTableStruct(string tableName)
        {
            List<TableStructure> listTs = new List<TableStructure>();
            string sql = "desc " + tableName;

            using (MySqlConnection con = new MySqlConnection(_conStr))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = sql;
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TableStructure ts = new TableStructure()
                    {
                        Field = reader["Field"].ToSafeString(),
                        Type = reader["Type"].ToSafeString(),
                        Null = reader["Null"].ToSafeString(),
                        Key = reader["Key"].ToSafeString(),
                        Default = reader["Default"].ToSafeString(),
                        Extra = reader["Extra"].ToSafeString()
                    };

                    listTs.Add(ts);
                }

                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }

            return listTs;
        }


        public List<String> GetTableNames(string dbName)
        {
            string sql = "select TABLE_NAME from information_schema.tables where TABLE_SCHEMA = '" + dbName + "'";

            List<string> listTables = new List<string>();

            using (MySqlConnection con = new MySqlConnection(_conStr))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = sql;
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listTables.Add(reader["TABLE_NAME"].ToSafeString());
                }

                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            };

            return listTables;
        }


        public void ExcuteSql(string sql)
        {
            using (MySqlConnection con = new MySqlConnection(_conStr))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            };
        }

    }


}
