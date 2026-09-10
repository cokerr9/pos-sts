using System;
using System.Data;
using System.Data.SqlClient;

namespace oke_mart.Controller
{
    internal class ControllerSupplier : Model.ModelSupplier
    {
        public DataTable dt { get; set; } = new DataTable();

        public void InsertSupplier()
        {
            using (SqlCommand cmd = new SqlCommand("InsertSupplier", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = CompanyName;
                cmd.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = SupplierName;
                cmd.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = PhoneNumber;
                cmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = (UserId > 0) ? (object)UserId : (object)DBNull.Value;

                ExecuteCommand(cmd);
            }
        }
        // Update Supplier Method
        public void UpdateSupplier()
        {
            using (SqlCommand cmd = new SqlCommand("UpdateSupplier", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SupplierId", SupplierId);
                cmd.Parameters.AddWithValue("@CompanyName", CompanyName);
                cmd.Parameters.AddWithValue("@SupplierName", SupplierName);
                cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                cmd.Parameters.AddWithValue("@Address", Address);
                cmd.Parameters.AddWithValue("@UpdateBy", (UserId > 0) ? (object)UserId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Update_At", DateTime.Now);

                ExecuteCommand(cmd);
            }
        }

        // Delete Supplier Method
        public void DeleteSupplier()
        {
            using (SqlCommand cmd = new SqlCommand("DeleteSupplier", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierId", SupplierId);
                ExecuteCommand(cmd);
            }
        }

        public void ViewData()
        {
            string sql = "SELECT * FROM V_Supplier";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                dt.Clear();
                adapter.Fill(dt);
            }
        }

        private void ExecuteCommand(SqlCommand cmd)
        {
            bool openedByUs = false;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    openedByUs = true;
                }
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (openedByUs && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}