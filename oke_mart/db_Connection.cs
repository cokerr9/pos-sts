using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace oke_mart
{
    class db_Connection
    {
        public SqlConnection conn;

        public db_Connection()
        {
            string[] connectionStrings = new string[]
            {
                @"Data Source=DESKTOP-O31OODG;Initial Catalog=Oke_db;User ID=sa;Password=Admin@12345678;"
            };
            foreach (string cs in connectionStrings)
            {
                try
                {
                    conn = new SqlConnection(cs);
                    conn.Open();
                    Console.WriteLine("Connection Successful!");
                    return;
                }
                catch
                {
                    // Try next fallback connection string
                }
            }
            conn = new SqlConnection(@"Data Source=DESKTOP-O31OODG;Initial Catalog=Oke_db;Integrated Security=True;");
            Console.WriteLine("Connection Failed to open initially. Default connection assigned.");
        }
    }
}