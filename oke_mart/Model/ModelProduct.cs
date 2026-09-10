using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oke_mart.Model
{
    internal class ModelProduct : db_Connection
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public double UnitPrice { get; set; }
        public double SalePrice { get; set; }
        public int QtyInStock { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Status { get; set; }
        public byte[] Photo { get; set; }
        public int UserCreate { get; set; }
        public DateTime Create_At { get; set; }
        public int UserUpdate { get; set; }
        public DateTime Update_At { get; set; }
    }
}
