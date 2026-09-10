using oke_mart.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oke_mart.Controller
{
    internal class ControllerSetting : ModelSetting
    {
        // Insert Setting
        public void InsertSetting()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "InsertSetting";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = CompanyName;
            cmd.Parameters.Add("@CompanyLogo", SqlDbType.VarBinary).Value = CompanyLogo;
            cmd.ExecuteNonQuery();
        }

        // Updata Setting 
        // Update Setting 
        public void UpdateSetting()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UpdateSetting";
            cmd.CommandType = CommandType.StoredProcedure;

            // Fixed typo: @SeetingId -> @SettingId
            cmd.Parameters.Add("@SettingId", SqlDbType.Int).Value = SettingId;
            cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = CompanyName;
            cmd.Parameters.Add("@CompanyLogo", SqlDbType.VarBinary).Value = CompanyLogo;

            cmd.ExecuteNonQuery();
        }
    }
}
