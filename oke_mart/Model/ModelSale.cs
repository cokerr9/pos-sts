using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oke_mart.Model
{
    internal class ModelSale : db_Connection
    {
        public int SaleId { get; set; }
        public int ReceiptId { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Payment { get; set; }
        public decimal Change { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SaleDate { get; set; }
        public int UserId { get; set; }
    }
}
