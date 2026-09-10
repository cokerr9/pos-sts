using System;
using System.Data;
using System.Data.SqlClient;

namespace oke_mart.Controller
{
    internal class ControllerCategory : Model.ModelCategory
    {
        public void InsertCategory()
        {
            using (SqlCommand cmd = new SqlCommand("InsertCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = CategoryName;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(Description) ? (object)DBNull.Value : Description;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
                cmd.Parameters.Add("@Create_At", SqlDbType.DateTime).Value = Create_At;

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.ExecuteNonQuery();

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // 2. View Data 
        public DataTable dt = new DataTable();
        public DataSet ds = new DataSet();
        public SqlDataAdapter adapter = new SqlDataAdapter();

        public void ViewData()
        {
            string sql = "SELECT * FROM tblCategory;";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                adapter.SelectCommand = cmd;

                ds.Clear();
                adapter.Fill(ds);
                dt = ds.Tables[0];
            }
        }

        // 1. Update Category Method
        public void UpdateCategory()
        {
            using (SqlCommand cmd = new SqlCommand("UpdateCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
                cmd.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = CategoryName;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(Description) ? (object)DBNull.Value : Description;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.ExecuteNonQuery();

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // 2. Delete Category Method
        public void DeleteCategory()
        {
            using (SqlCommand cmd = new SqlCommand("DeleteCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.ExecuteNonQuery();

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}