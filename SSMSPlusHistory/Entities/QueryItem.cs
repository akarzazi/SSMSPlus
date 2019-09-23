using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SSMSPlusHistory.Entities
{
    public class QueryItem
    {
        public int Id { get; set; }

        public DateTime ExecutionDateUtc { get; set; }

        public string Query { get; set; }

        public string Server { get; set; }

        public string Username { get; set; }

        public string Database { get; set; }
    }
}
