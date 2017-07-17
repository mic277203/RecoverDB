using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace RecoverDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "数据库同步工具";

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            StringBuilder strBuilder = new StringBuilder();

            string[] tables = ConfigurationManager.AppSettings["TableNames"].Split(',');

            SrcDBService service = new SrcDBService();
            service.CreateDBJson();
            var listsrcTables = service.GetDBTables();
            DescDBService descService = new DescDBService();

            foreach (var t in tables)
            {
                var listDiff = descService.GetDiffStruct(t, listsrcTables);

                listDiff.ForEach(p =>
                {
                    strBuilder.Append(CreateAddColSql(t, p));
                });
            }

            string isAuto = ConfigurationManager.AppSettings["IsAuto"];

            if (string.Compare("true", isAuto, true) == 0)
            {
                string sqlResult = strBuilder.ToString();

                if (!string.IsNullOrEmpty(sqlResult))
                {
                    descService.AddCol(sqlResult);
                }
            }
            else
            {
                string sqlPath = AppDomain.CurrentDomain.BaseDirectory + "Excute.sql";
                File.WriteAllText(sqlPath, strBuilder.ToString());
            }

            Console.WriteLine("同步完成，按任意键退出...");
            Console.ReadKey();
        }


        public static string CreateAddColSql(string tableName, TableStructure struc)
        {
            string sql = string.Format("alter table {0} add {1} {2} ", tableName, struc.Field, struc.Type);

            if (!string.IsNullOrEmpty(struc.Default))
            {
                sql += "default " + struc.Default;
            }

            sql += ";\n";

            return sql;
        }
    }


    public static class ObjectHelper
    {
        public static string ToSafeString(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
    }
}
