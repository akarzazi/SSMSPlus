using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Entities
{
    public class DbColumn
    {
        public int DbId { get; set; }

        public long TableId { get; set; }

        public string Name { get; set; }

        public string Datatype { get; set; }

        public int? Precision { get; set; }

        public int? Scale { get; set; }

        public string Definition { get; set; }

        public DbObject Parent { get; set; }
    }
}