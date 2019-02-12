using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

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
                /*SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.Server = "";
                builder.UserID = "client";
                builder.Password = "Nu&PIO5yZD43DkZ#fx#0";
                builder.Database = "NEA";
                //string connection_string = "Server = 192.168.1.73; Port = 3306; Database = NEA; Uid = client; Pwd = Nu&PIO5yZD43DkZ#fx#0;";

                SqlConnection connection = new SqlConnection(connection_string);
                return connection;
                {}*/
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "DESKTOP-26UCP1R";
                builder.UserID = "test";
                builder.Password = "test";
                builder.InitialCatalog = "NEA";

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                return connection;

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
