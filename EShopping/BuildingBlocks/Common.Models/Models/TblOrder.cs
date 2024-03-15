using System;
using System.Collections.Generic;

namespace Common.Models.Models
{
    public partial class TblOrder
    {
        public int Id { get; set; }
        public string? OrderNumber { get; set; }
        public int? UserId { get; set; }
        public decimal? Price { get; set; }
        public int? PaymentMode { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
