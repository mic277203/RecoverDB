using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace RecoverDB
{
    public class SrcDBService
    {
        private string _conStr;
        private string _dbName;
        public SrcDBService()
        {
            _conStr = ConfigurationManager.ConnectionStrings["SrcStr"].ConnectionString;
            _dbName = ConfigurationManager.AppSettings["DbName"];
        }
        public void CreateDBJson()
        {
            string jsonRootPath = AppDomain.CurrentDomain.BaseDirectory + "CurrentTable";
            string jsonPath = jsonRootPath + "\\" + _dbName + ".json";

            if (File.Exists(jsonPath))
            {
                return;
            }

            DBHelper helper = new DBHelper(_conStr);
            var listTables = helper.GetTableNames(_dbName);
            List<DbTable> listDt = new List<DbTable>();

            foreach (string tb in listTables)
            {
                DbTable dt = new DbTable();

                var listStruct = helper.GetTableStruct(tb);

                dt.ListTableStuc = listStruct;
                dt.TableName = tb;

                listDt.Add(dt);
            }

            if (!Directory.Exists(jsonRootPath))
            {
                Directory.CreateDirectory(jsonRootPath);
            }

            string jsonResult = JsonConvert.SerializeObject(listDt);
            File.WriteAllText(jsonPath, jsonResult, Encoding.UTF8);
        }

        public List<DbTable> GetDBTables()
        {
            string jsonPath = AppDomain.CurrentDomain.BaseDirectory + "CurrentTable\\" + _dbName + ".json";

            if (!File.Exists(jsonPath))
            {
                throw new ArgumentNullException("源数据库表结构Json不存在");
            }

            string jsonResult = File.ReadAllText(jsonPath, Encoding.UTF8);

            var listDbTables = JsonConvert.DeserializeObject<List<DbTable>>(jsonResult);

            return listDbTables;
        }
    }
}
