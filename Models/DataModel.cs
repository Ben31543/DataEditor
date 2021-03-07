using System;
using System.Collections.Generic;

namespace Models
{
    public class DataModel
    {
        public string Name { get; set; }

        public Type ColumnType { get; set; }

        public bool IsUnique { get; set; }

        public List<object> Values { get; set; }
    }
}
