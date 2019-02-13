using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace NEA
{
    public class Account : Database_Connect, INotifyPropertyChanged
    {
        public string AccountName { get; set; } 
        public string AccountID { get; set; }
        public bool SignedIn { get; set; }
        public string Password { get; set; }
        public ObservableCollection<GameListDisplay> GameList { get; set; }
        public List<string> StringListGameName { get; set; } 

        public Account()
        { }
        
        //signs a user into an existing account
        public void SignIn(string name, string password)
        {
            string command_text = @"SELECT ID,PassHash,Salt,UserNames FROM Users_2 " +
                "WHERE UserNames = @name";

            MySqlConnection connection = Connect();
            try
            {
                MySqlCommand command = new MySqlCommand(command_text, connection);
                command.Parameters.AddWithValue("@name", name);
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Generates hash value based on password
                        string The_Hash = Hash(password, Salt(reader["Salt"].ToString(), true)).Get_Hash();
                        if (The_Hash == reader["PassHash"].ToString())
                        {
                            //assigns retrived values to class attributes
                            SignedIn = true;
                            AccountName = reader["UserNames"].ToString();
                            AccountID = reader["ID"].ToString();
                            Password = password; 
                            Debug.WriteLine("Sign in successful");

                            NotifyPropertyChanged("AccountName");
                            NotifyPropertyChanged("GameList");

                        }
                    }
                }
                connection.Close();
                Get_Game_List(name);
            }
            catch (Exception Error)
            {
                Debug.WriteLine("Failed to login");
                Debug.WriteLine(Error);
                connection.Close();
            }
        }

        //generates salt used to salt the hash
        public byte[] Salt(string The_Salt, bool using_salt)
        {
            
            if (using_salt == true)
            {
                //if a salt has been given
                byte[] salt = Convert.FromBase64String(The_Salt);
                return salt;
            }
            else
            {
                //generates a new salt
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                return salt;
            }
        }

        //Generates a salted hash
        public Salt_Hash Hash(string password, byte[] salt)
        {

            var Salted_Hash = new Rfc2898DeriveBytes(password, salt, 15000);

            byte[] hashed_bytes = new byte[36];
            Array.Copy(salt, 0, hashed_bytes, 0, 16);
            Array.Copy(Salted_Hash.GetBytes(20), 0, hashed_bytes, 16, 20); //copies byte[], string and int values to array
            string Pass_Hash = Convert.ToBase64String(hashed_bytes); //converts bytes to string
            Debug.WriteLine("\nSalt Used:" + Convert.ToBase64String(salt));

            Salt_Hash Salt_and_Hash = new Salt_Hash(Convert.ToBase64String(salt), Pass_Hash); //creates new salt_hash class with the salt and the hash
            return Salt_and_Hash;
        }

        public bool check_user(string name)//checks whether a username is already in the table
        {
            string command_text = @"SELECT UserNames FROM Users_2 WHERE UserNames=@name";
            MySqlConnection connection = Connect();
            try
            {
                MySqlCommand command = new MySqlCommand(command_text, connection);

                command.Parameters.AddWithValue("@name", name);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.WriteLine(reader["UserNames"].ToString());
                        if (name == reader["UserNames"].ToString())
                        {
                            //user already exists
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

        //Adds a new user to the database
        public void add_user(string name, string password)
        {
            //checks if the name already exists
            if (check_user(name) == true)
            {
                //generates new salt
                Salt_Hash Hash_Salt = Hash(password, Salt(null, false));
                string command_text = @"INSERT INTO Users_2 (UserNames, PassHash, Salt) " +
                    "Values (@name, @Hash, @Salt)";
                //inserts new values into database
                MySqlConnection connection = Connect();
                try
                {
                    MySqlCommand command = new MySqlCommand(command_text, connection);

                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@Hash", Hash_Salt.Get_Hash());
                    command.Parameters.AddWithValue("@Salt", Hash_Salt.Get_Salt());

                    connection.Open();
                    MySqlDataReader read = command.ExecuteReader();
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

        //retrieves list of 
        public void Get_Game_List(string GameUserID)
        {
            ObservableCollection<GameListDisplay> Name_List = new ObservableCollection<GameListDisplay>();
            List<string> Name_List_String = new List<string>();
            string command_text = @"SELECT GameName FROM GameData WHERE GameUserName = @ID";

            MySqlConnection connection = Connect();
            try
            {
                MySqlCommand command = new MySqlCommand(command_text, connection);
                command.Parameters.AddWithValue("@ID", GameUserID);
                connection.Open();

                using (MySqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Name_List.Add(new GameListDisplay(dataReader["GameName"].ToString()));
                        Name_List_String.Add(dataReader["GameName"].ToString());
                        NotifyPropertyChanged("GameList");
                    }
                    GameList = Name_List;
                    StringListGameName = Name_List_String;
                }
            }

            catch (Exception error)
            {
                Debug.WriteLine(error.ToString());
            }
        }

        public ObservableCollection<GameListDisplay> Return_game_list()
        {
            return GameList;
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
