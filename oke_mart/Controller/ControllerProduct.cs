using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oke_mart.Controller
{
    internal class ControllerProduct : Model.ModelProduct
    {
        // View Category 
        public DataTable DtCa = new DataTable();
        public DataSet DsCa = new DataSet();

        public void ViewCategory()
        {
            string sql = "SELECT * FROM V_CategoryForProduct;";
            SqlDataAdapter adapterCa = new SqlDataAdapter(sql, conn);

            DsCa.Clear();
            adapterCa.Fill(DsCa, "Category");
            DtCa = DsCa.Tables["Category"];
        }

        // View Supplier 
        public DataTable DtSu = new DataTable();
        public DataSet DsSu = new DataSet();

        public void ViewSupplier()
        {
            string sql = "SELECT * FROM tblSupplier;";

            SqlDataAdapter adapterSu = new SqlDataAdapter(sql, conn);

            DsSu.Clear();
            adapterSu.Fill(DsSu, "Supplier");
            DtSu = DsSu.Tables["Supplier"];
        }

        /// <summary>
        /// For Insert , Update , Delete data from table Product
        /// </summary>

        // Insert data
        public void InsertProduct()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "InsertProduct";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
            cmd.Parameters.Add("@Barcode", SqlDbType.NVarChar).Value = Barcode;
            cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = ProductName;
            cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = SupplierId;
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Float).Value = UnitPrice;
            cmd.Parameters.Add("@SalePrice", SqlDbType.Float).Value = SalePrice;
            cmd.Parameters.Add("@QtyinStock", SqlDbType.Int).Value = QtyInStock;
            cmd.Parameters.Add("@ExpireDate", SqlDbType.Date).Value = ExpireDate;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
            cmd.Parameters.Add("@Photo", SqlDbType.Image).Value = Photo;
            cmd.Parameters.Add("@UserCreate", SqlDbType.Int).Value = (UserCreate > 0) ? (object)UserCreate : (object)DBNull.Value;
            cmd.Parameters.Add("@Create_At", SqlDbType.DateTime).Value = Create_At;

            if (conn.State == ConnectionState.Closed) conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        // Update data
        public void UpdateProduct()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UpdateProduct";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = ProductId; 
            cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
            cmd.Parameters.Add("@Barcode", SqlDbType.NVarChar).Value = Barcode;
            cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = ProductName;
            cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = SupplierId;
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Float).Value = UnitPrice;
            cmd.Parameters.Add("@SalePrice", SqlDbType.Float).Value = SalePrice;
            cmd.Parameters.Add("@QtyinStock", SqlDbType.Int).Value = QtyInStock;
            cmd.Parameters.Add("@ExpireDate", SqlDbType.Date).Value = ExpireDate;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
            cmd.Parameters.Add("@Photo", SqlDbType.Image).Value = Photo;
            cmd.Parameters.Add("@UserUpdate", SqlDbType.Int).Value = (UserUpdate > 0) ? (object)UserUpdate : (object)DBNull.Value; 
            cmd.Parameters.Add("@Update_At", SqlDbType.DateTime).Value = Update_At;  

            if (conn.State == ConnectionState.Closed) conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        // Delete data from table Product
        public void DeleteProduct()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DeleteProduct";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = ProductId;
            if (conn.State == ConnectionState.Closed) conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        // View data 
        public DataTable dt = new DataTable();
        public DataSet ds = new DataSet();
        public SqlDataAdapter adapter = new SqlDataAdapter();

        // Create Method for Call data to use
        public void ViewData()
        {
            string sql = "SELECT * FROM V_ProductDetails;"; 
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                dt.Clear();
                adapter.Fill(dt);
            }
        }
    }
}