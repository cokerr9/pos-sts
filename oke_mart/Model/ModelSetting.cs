using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oke_mart.Model
{
    internal class ModelSetting : db_Connection
    {
        public int SettingId { get; set; }
        public string CompanyName { get; set; }
        public byte[] CompanyLogo { get; set; }
    }
}
