using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;

namespace NEA
{
    public class Database_Connect
    {
        public Database_Connect()
        { }

        public SqlConnection Connect()
        { 
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "192.168.1.73,3306";
                builder.UserID = "client";
                builder.Password = "Nu&PIO5yZD43DkZ#fx#0";
                builder.InitialCatalog = "NEA";

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                return connection;
                {}
            }

            catch (Exception error)
            {
                Debug.WriteLine(error.ToString());
            }

            Debug.WriteLine("");
            return null;
        }

    }
}
