﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace NEA
{
    public class Database_Connect
    {
        public Database_Connect()
        { }

        public MySqlConnection Connect()
        { 
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                /*builder.Server = "192.168.1.73,3306";
                builder.UserID = "client";
                builder.Password = "Nu&PIO5yZD43DkZ#fx#0";
                builder.Database = "NEA";*/
                string connection_string = "Server = 192.168.1.73; Port = 3306; Database = NEA; Uid = client; Pwd = Nu&PIO5yZD43DkZ#fx#0;";

                MySqlConnection connection = new MySqlConnection(connection_string);
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
