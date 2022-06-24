using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Request
{
    public class CustomerTransactionDTO
    {
        public Guid Id { get; set; }
        public string TransactionReference { get; set; }
        public decimal Amount { get; set; }
        public string OrderStatusStatus { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Description { get; set; }
    }
}
