using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Nozama
{
    public class Connection
    {
        public MySqlConnection connection;
        public void Connect()
        {
            connection = new MySqlConnection("Datasource=127.0.0.1;username=root;password=;database=nozama");
        }
    }
}
