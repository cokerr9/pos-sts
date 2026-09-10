using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace oke_mart.Controller
{
    internal class ControllerUsers : Model.ModelUsers
    {
        // Insert Data User

        public void InsertUser()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "InsertUser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;
            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
            cmd.Parameters.Add("@UserType", SqlDbType.VarChar).Value = UserType;
            cmd.ExecuteNonQuery();
        }

        // Updat Data User
        public void UpdateUsers()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UpdateUsers";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
            cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;
            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
            cmd.Parameters.Add("@UserType", SqlDbType.VarChar).Value = UserType;
            cmd.ExecuteNonQuery();
        }

        // Delete Data User
        public void DeleteUsers()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DeleteUser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
            cmd.ExecuteNonQuery();
        }
        // View Data User 
        public DataTable dt = new DataTable();
        public DataSet ds = new DataSet();
        public SqlDataAdapter adapter = new SqlDataAdapter();

        public void ShowData()
        {
            string sql = "SELECT * FROM tblUsers;";
            SqlCommand cmd = new SqlCommand(sql,conn);
            adapter.SelectCommand = cmd;
            ds.Clear();
            adapter.Fill(ds);
            dt = ds.Tables[0];
        }
    }
}
