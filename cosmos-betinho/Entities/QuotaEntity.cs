using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmos_betinho.Entities
{
    public class QuotaEntity
    {
        public Guid Id { get; set; }
        public Guid ActionId { get; set; }
        public List<int> QuotasAquired { get; set; }
        public int TotalQuotasAquired { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? ThirdEmail { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public DateTime BilledAt { get; set; }
        public DateTime? AtachedAt { get; set; }
        public decimal TotalBilled { get; set; }
        public string? AtachLink { get; set; }

    }
}
