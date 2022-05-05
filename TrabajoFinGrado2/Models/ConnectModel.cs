using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Connect.Models
{
    public class ConnectModel
    {
        public SqlConnection con { get; set; }
        public SqlCommand com { get; set; }
        public SqlDataReader dr { get; set; }
        public void connectionString_noinit()
        {
            String server = "DESKTOP-VKULAOJ\\SQLEXPRESS";
            String user = "TFGRPA";
            String password = "Con1234*";
            String data_base = "TFGRPA";
            con.ConnectionString = "data source=" + server + "; database=" + data_base + "; User Id=" + user + "; Password=" + password + "; Max Pool Size=200;";
        }
    }
}
