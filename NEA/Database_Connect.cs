using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;

namespace NEA
{
    class Database_Connect
    {
        public Database_Connect()
        { }

        public SqlConnection Connect()
        { 
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "DESKTOP-26UCP1R";
                builder.UserID = "test";
                builder.Password = "test";
                builder.InitialCatalog = "NEA";

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                return connection;
                {
                    /*connection.Open();
                    Debug.WriteLine("Connected");
                    connection.Close();
                    return connection;*/

                }
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
