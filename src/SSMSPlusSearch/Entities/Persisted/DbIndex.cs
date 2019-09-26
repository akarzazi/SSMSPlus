using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Entities
{
    public class DbIndex
    {
        public int DbId { get; set; }
        public int OwnerId { get; set; }
        public int IndexNumber { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string FilterDefinition { get; set; }
        public bool IsUnique { get; set; }

        public DbObject Parent { get; set; }
        public DbIndexColumn[] Columns { get; set; }
    }
}
