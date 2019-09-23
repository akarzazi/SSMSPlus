using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Entities
{
    public class DbObject
    {
        public int DbId { get; set; }

        public long ObjectId { get; set; }

        public string Type { get; set; }

        public string SchemaName { get; set; }

        public string Name { get; set; }

        public string Definition { get; set; }

        public DateTime ModificationDate { get; set; }

        public long? ParentObjectId { get; set; }

        public DbObject Parent { get; set; }
    }
}
