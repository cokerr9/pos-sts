using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oke_mart.Model
{
    internal class ModelSupplier : db_Connection
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public string SupplierName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int UserId { get; set; }

    }
}
