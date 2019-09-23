using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Entities
{
    public class DbDefinition
    {
        public int DbId { get; set; }

        public string Server { get; set; }

        public string DbName { get; set; }

        public DateTime IndexDateUtc { get; set; }
    }
}
