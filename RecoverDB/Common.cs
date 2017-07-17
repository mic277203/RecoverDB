using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecoverDB
{
    public class TableStructure
    {
        public string Field { get; set; }
        public string Type { get; set; }
        public string Null { get; set; }
        public string Key { get; set; }
        public string Default { get; set; }
        public string Extra { get; set; }
    }

    public class DbTable
    {
        public string TableName { get; set; }
        public List<TableStructure> ListTableStuc { get; set; }
    }
}
