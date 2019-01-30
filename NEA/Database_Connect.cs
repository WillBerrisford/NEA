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

        public void add_user(string name, string hash)
        {
            SqlConnection connection = Connect();
            string command_text = @"INSERT INTO Users_2 (UserNames, PassHash) " +
                "Values ('" + name + "', '" + hash + "')";
            try
            {
                SqlCommand command = new SqlCommand(command_text, connection);
                connection.Open();
                SqlDataReader read = command.ExecuteReader();
                connection.Close();
            }

            catch(Exception error)
            {
                Debug.WriteLine(error.ToString());
            }

        }
    }
}
