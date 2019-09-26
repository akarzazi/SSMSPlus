using SSMSPlusCore.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusCore.Settings
{
    public class DistributionSettings : IValidatable<DistributionSettings>
    {
        public string ContributeUrl { get; set; }
        public string ContributeText { get; set; }

        public DistributionSettings Validate()
        {
            if (string.IsNullOrWhiteSpace(ContributeUrl))
                throw new ArgumentException(nameof(ContributeUrl));

            if (string.IsNullOrWhiteSpace(ContributeText))
                throw new ArgumentException(nameof(ContributeText));

            return this;
        }
    }
}
