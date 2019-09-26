using SSMSPlusCore.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Settings
{
    public class DemoDbSettings : IValidatable<DemoDbSettings>
    {
        public string ConnectionString { get; set; }
        public string DbName { get; set; }

        public DemoDbSettings Validate()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new ArgumentException(nameof(ConnectionString));

            if (string.IsNullOrWhiteSpace(DbName))
                throw new ArgumentException(nameof(DbName));

            return this;
        }
    }
}
