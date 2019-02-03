using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace NEA
{
    class Account : Database_Connect
    {
        private string AccountName { get; set; }
        private bool SignedIn { get; set; }
        private string Password { get; set; }

        public Account()
        { }
        
        public void SignIn(string name, string password)
        {
            Salt_Hash Hash_Salt = Hash(password);
            string command_text = @"SELECT PassHash FROM Users_2 " +
                "WHERE UserNames = @name";

            SqlConnection connection = Connect();
            try
            {
                SqlCommand command = new SqlCommand(command_text, connection);
                command.Parameters.AddWithValue("@name", name);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (Hash_Salt.Get_Salt() == reader["PassHash"].ToString())
                        {
                            SignedIn = true;
                            Debug.WriteLine("User Has been signed in");
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception Error)
            {
                Debug.WriteLine(Error);
            }
        }

        public Salt_Hash Hash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var PBKDF2_Salted_Hash = new Rfc2898DeriveBytes(password, salt, 15000);

            byte[] hashed_bytes = new byte[36];
            Array.Copy(salt, 0, hashed_bytes, 0, 16);
            Array.Copy(PBKDF2_Salted_Hash.GetBytes(20), 0, hashed_bytes, 16, 20);
            string Pass_Hash = Convert.ToBase64String(hashed_bytes);
            Debug.WriteLine(Pass_Hash);

            Salt_Hash Salt_and_Hash = new Salt_Hash(Convert.ToBase64String(salt), Pass_Hash);
            return Salt_and_Hash;
        }

        public void add_user(string name, string password)
        {
            Salt_Hash Hash_Salt = Hash(password);
            string command_text = @"INSERT INTO Users_2 (UserNames, PassHash, Salt) " +
                "Values ('" + name + "', '" + Hash_Salt.Get_Hash() + "', '" + Hash_Salt.Get_Salt() + "')";

            SqlConnection connection = Connect();
            try
            {
                SqlCommand command = new SqlCommand(command_text, connection);
                connection.Open();
                SqlDataReader read = command.ExecuteReader();
                connection.Close();

            }

            catch (Exception error)
            {
                Debug.WriteLine(error.ToString());
            }
        }
    }
}
