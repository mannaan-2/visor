using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visor.Abstractions.Entities.Config.OpenApi
{
    public class Settings
    {
        public string AuthorizationUrl { get; set; }
        public string TokenUrl { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TermsOfServiceUrl { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactUrl { get; set; }

        public string License { get; set; }
        public string LicenseUrl { get; set; }
        public  Dictionary<string, string> Scopes{ get; set; }
        public string ExposedEndpoint { get; set; }
        public string EndpointDescription { get; set; }
        public string UIPrefix { get; set; }
        public string OAuthClientid { get; set; }
        public string OAuthAppName { get; set; }
        public bool UsePkce { get; set; }
    }
}
