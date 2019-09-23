using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Entities
{
    public class DbIndexColumn
    {
        public int DbId { get; set; }
        public int OwnerId { get; set; }
        public int IndexNumber { get; set; }
        public int IndexColumnId { get; set; }
        public int OwnerColumnId { get; set; }
        public string ColumnName { get; set; }
        public bool Included { get; set; }
        public bool IsDesc { get; set; }
    }
}