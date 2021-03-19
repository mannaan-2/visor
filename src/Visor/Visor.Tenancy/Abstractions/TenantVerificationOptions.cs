using System.Collections.Generic;

namespace Visor.Data.MySql.Abstractions
{
    public class TenantVerificationOptions
    {
        public TenantVerificationOptions(List<string> ignoredPaths) {
            IgnoredPaths = ignoredPaths;
        }

        public List<string> IgnoredPaths { get; }
    }
}
