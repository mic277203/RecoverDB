using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace RecoverDB
{
    public class DescDBService
    {
        private string _conStr;
        private string _dbName;
        public DescDBService()
        {
            _conStr = ConfigurationManager.ConnectionStrings["DescStr"].ConnectionString;
            _dbName = ConfigurationManager.AppSettings["DbName"];
        }
        public List<TableStructure> GetDiffStruct(string tableName, List<DbTable> listDB)
        {
            List<TableStructure> listDiffStruct = new List<TableStructure>();

            DBHelper helper = new DBHelper(_conStr);
            var listdescStruts = helper.GetTableStruct(tableName);
            var listSrcStructs = listDB.FirstOrDefault(p => string.Compare(p.TableName, tableName, true) == 0).ListTableStuc;

            listSrcStructs.ForEach(p =>
            {
                if (listdescStruts.Count(m => string.Compare(m.Field, p.Field, true) == 0) == 0)
                {
                    listDiffStruct.Add(p);
                }
            });

            return listDiffStruct;
        }


        public void AddCol(string sql)
        {
            DBHelper helper = new DBHelper(_conStr);
            helper.ExcuteSql(sql);
        }
    }
}
