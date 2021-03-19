using System.ComponentModel.DataAnnotations;
using Visor.Tenancy.Abstractions;

namespace Visor.Data.MySql.Identity.Entities
{
    public class Tenant : ITenant
    {
        public int TenantId { get; set; }
        [Required]
        [MaxLength(256)]
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string Host { get; set; }
        [Required]
        public bool Active { get; set; }
        public string Description { get; set; }
    }
}
