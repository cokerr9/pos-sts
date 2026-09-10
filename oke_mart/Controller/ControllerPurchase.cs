using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oke_mart.Controller
{
    internal class ControllerPurchase : Model.ModelPurchase
    {
        public DataTable dtPro = new DataTable();
        public DataSet dsPro = new DataSet();
        public void ViewProduct()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            string sql = "SELECT * FROM tblProduct where status = 'Yes';";
            SqlDataAdapter adapterPro = new SqlDataAdapter(sql, conn);
            dsPro.Clear();
            adapterPro.Fill(dsPro, "Product");
            dtPro = dsPro.Tables["Product"];
        }

        // View Data 
        public DataTable dt = new DataTable();
        public DataSet ds = new DataSet();
        public SqlDataAdapter adapter = new SqlDataAdapter();
        public void ViewPurchase()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            string sql = "SELECT * FROM V_Purchase";
            adapter = new SqlDataAdapter(sql, conn);
            ds.Clear();
            adapter.Fill(ds);
            dt = ds.Tables[0];
        }

        public void InsertPurchase()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (SqlCommand cmd = new SqlCommand("InsertPurchase", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = ProductId;
                cmd.Parameters.Add("@UnitPrice", SqlDbType.Int).Value = UnitPrice;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Quantity;
                cmd.Parameters.Add("@TotalPrice", SqlDbType.Int).Value = TotalPrice;
                cmd.Parameters.Add("@PurchaseDate", SqlDbType.Date).Value = PurchaseDate;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = (UserId > 0) ? (object)UserId : (object)DBNull.Value;
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdatePurchase()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (SqlCommand cmd = new SqlCommand("UpdatePurchase", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PurchaseId", SqlDbType.Int).Value = PurchaseId;
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = ProductId;
                cmd.Parameters.Add("@UnitPrice", SqlDbType.Int).Value = UnitPrice;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Quantity;
                cmd.Parameters.Add("@TotalPrice", SqlDbType.Int).Value = TotalPrice;
                cmd.Parameters.Add("@PurchaseDate", SqlDbType.Date).Value = PurchaseDate;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = (UserId > 0) ? (object)UserId : (object)DBNull.Value;
                cmd.ExecuteNonQuery();
            }
        }

        public void DeletePurchase()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (SqlCommand cmd = new SqlCommand("DeletePurchase", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PurchaseId", SqlDbType.Int).Value = PurchaseId;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
