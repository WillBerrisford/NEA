using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.ComponentModel;

namespace NEA
{
    public class Account : Database_Connect, INotifyPropertyChanged
    {
        public string AccountName { get; set; } 
        public string AccountID { get; set; }
        public bool SignedIn { get; set; }
        public string Password { get; set; }

        public Account()
        { }

        public void Backup_Account(string name, string id, bool signed_in, string password)
        {

        }
        
        public void SignIn(string name, string password)
        {
            string command_text = @"SELECT ID,PassHash,Salt,UserNames FROM Users_2 " +
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
                        string The_Hash = Hash(password, Salt(reader["Salt"].ToString(), true)).Get_Hash();
                        Debug.WriteLine("The Stored Salt:" + reader["Salt"].ToString());
                        Debug.WriteLine("Hash1 " + The_Hash + " \nHash2 " + reader["PassHash"].ToString());
                        if (The_Hash == reader["PassHash"].ToString())
                        {
                            SignedIn = true;
                            AccountName = reader["UserNames"].ToString();
                            AccountID = reader["ID"].ToString();
                            Password = password; 
                            Debug.WriteLine("Sign in successful");

                            NotifyPropertyChanged("AccountName");

                        }
                    }
                }
                connection.Close();
            }
            catch (Exception Error)
            {
                Debug.WriteLine(Error);
                connection.Close();
            }
        }

        public byte[] Salt(string The_Salt, bool using_salt)
        {
            if (using_salt == true)
            {
                byte[] salt = Convert.FromBase64String(The_Salt);
                return salt;
            }
            else
            {
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                return salt;
            }
        }

        public Salt_Hash Hash(string password, byte[] salt)
        {

            var PBKDF2_Salted_Hash = new Rfc2898DeriveBytes(password, salt, 15000);

            byte[] hashed_bytes = new byte[36];
            Array.Copy(salt, 0, hashed_bytes, 0, 16);
            Array.Copy(PBKDF2_Salted_Hash.GetBytes(20), 0, hashed_bytes, 16, 20);
            string Pass_Hash = Convert.ToBase64String(hashed_bytes);
            Debug.WriteLine("\nSalt Used:" + Convert.ToBase64String(salt));

            Salt_Hash Salt_and_Hash = new Salt_Hash(Convert.ToBase64String(salt), Pass_Hash);
            return Salt_and_Hash;
        }

        public bool check_user(string name)//checks whether a username is already in the table
        {
            string command_text = @"SELECT UserNames FROM Users_2 WHERE UserNames=@name";
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
                        Debug.WriteLine(reader["UserNames"].ToString());
                        if (name == reader["UserNames"].ToString())
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    return true;
                }
            }

            catch (Exception error)
            {
                Debug.WriteLine(error.ToString());
                return false;
            }
        }

        public void add_user(string name, string password)
        {
            if (check_user(name) == true)
            {
                Salt_Hash Hash_Salt = Hash(password, Salt(null, false));
                string command_text = @"INSERT INTO Users_2 (UserNames, PassHash, Salt) " +
                    "Values (@name, @Hash, @Salt)";

                SqlConnection connection = Connect();
                try
                {
                    SqlCommand command = new SqlCommand(command_text, connection);

                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@Hash", Hash_Salt.Get_Hash());
                    command.Parameters.AddWithValue("@Salt", Hash_Salt.Get_Salt());

                    connection.Open();
                    SqlDataReader read = command.ExecuteReader();
                    connection.Close();
                }

                catch (Exception error)
                {
                    Debug.WriteLine(error.ToString());
                }
            }
            else
            {
                Debug.WriteLine("\nUsername taken");
            }
        }

        public string Get_Password()
        {
            return Password;
        }

        public string Get_ID()
        {
            return AccountID;
        }

       public string Get_AccountName()
        {
            return AccountName;
        }

        public bool Is_Signed_In()
        {
            return SignedIn;
        }

        public event PropertyChangedEventHandler PropertyChanged; //Allows properties to change so the UI is updated

        public void NotifyPropertyChanged(string propertyName) //updates ui
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
