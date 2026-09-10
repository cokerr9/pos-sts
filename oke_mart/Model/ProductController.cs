using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oke_mart.Model
{
    internal class ProductController : Model.ModelProduct
    {
        // View Category 
        public DataTable dtCa { get; set; } = new DataTable();
        public DataSet dsCa = new DataSet();
        public void ViewCategory()
        {
            string sql = "SELECT * FROM V_CategoryForProduct";
            SqlDataAdapter adapterC = new SqlDataAdapter(sql,conn);
            adapterC.Fill(dsCa, "Category");
            dtCa = dsCa.Tables["Category"];
        }
        // View Supplier
        public DataTable dtSu { get; set; } = new DataTable();
        public DataSet dsSu = new DataSet();
        public void ViewSupplier()
        {
            string sql = "SELECT * FROM V_SupplierForProduct";
            SqlDataAdapter adapterS = new SqlDataAdapter(sql, conn);
            adapterS.Fill(dsSu, "Supplier");
            dtSu = dsSu.Tables["Supplier"];
        }
    }
}
