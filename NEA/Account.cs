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
using System.Xml.Serialization;

namespace NEA
{
    public class Account : Database_Connect, INotifyPropertyChanged
    {
        public string AccountName { get; set; } 
        public string AccountID { get; set; }
        public bool SignedIn { get; set; }
        public string Password { get; set; }
        public ObservableCollection<GameListDisplay> GameList { get; set; }
        [XmlIgnoreAttribute]
        public LinkedList<string> StringListGameName { get; set; }

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
                MySqlCommand command = new MySqlCommand(command_text, connection); //creates a new command instance to send to the database
                command.Parameters.AddWithValue("@name", name); //paramterises the query
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader()) //excecutes command
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

                            NotifyPropertyChanged("AccountName"); //Updates the ui to reflect changes
                            NotifyPropertyChanged("GameList");

                        }
                    }
                }
                connection.Close();
                if (SignedIn == true)
                {
                    Get_Game_List(name); //retrieves lists of saved games
                }
            }
            catch (Exception Error) //runs if there is an error with the operation
            {
                Debug.WriteLine("Failed to login");
                Debug.WriteLine(Error); //displays the error in the debug window
                connection.Close();
            }
        }

        //generates salt used to salt the hash
        public byte[] Salt(string The_Salt, bool using_salt)
        {
            
            if (using_salt == true)
            {
                //if a salt has been given
                byte[] salt = Convert.FromBase64String(The_Salt); //converts given salt to byte[]
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
            // uses Rfc2898 (which implements the PBKDF2 hashing algorithm)
            //hashes the given password, with the salt, using 150,000 iterations (for increased security)
            var Salted_Hash = new Rfc2898DeriveBytes(password, salt, 150000);

            byte[] hashed_bytes = new byte[36]; //creates new byte array
            Array.Copy(salt, 0, hashed_bytes, 0, 16); //copies the salt into a the hashed bytes array
            Array.Copy(Salted_Hash.GetBytes(20), 0, hashed_bytes, 16, 20); //copies the salted hash byte array into the hashes bytes array
            string Pass_Hash = Convert.ToBase64String(hashed_bytes); //converts bytes to string

            Salt_Hash Salt_and_Hash = new Salt_Hash(Convert.ToBase64String(salt), Pass_Hash); //creates new salt_hash class with the salt and the finsished hash
            return Salt_and_Hash;
        }

        public bool check_user(string name)//checks whether a username is already in the table
        {
            string command_text = @"SELECT UserNames FROM Users_2 WHERE UserNames=@name";
            MySqlConnection connection = Connect(); 
            try
            {
                MySqlCommand command = new MySqlCommand(command_text, connection); //initiates a new command to the server

                command.Parameters.AddWithValue("@name", name); //parameterises the query

                connection.Open(); //opens the connection

                using (MySqlDataReader reader = command.ExecuteReader()) //reads the response to the query
                {
                    while (reader.Read())
                    {
                        if (name == reader["UserNames"].ToString()) //checks if the username from the database is the same one provided by the user
                        {
                            //user already exists
                            return false;
                        }
                        else
                        { }
                    }
                    //user does not exist
                    return true;
                }
            }

            catch (Exception error) //runs if there is an error with the statment
            {
                Debug.WriteLine(error.ToString()); //displays the error in the degub window
                return false;
            }
        }

        //Adds a new user to the database 
        public void add_user(string name, string password)
        {
            //checks if the name already exists
            if (check_user(name) == true)
            {
                //generates new salt_hash
                Salt_Hash Hash_Salt = Hash(password, Salt(null, false));
                string command_text = @"INSERT INTO Users_2 (UserNames, PassHash, Salt) " +
                    "Values (@name, @Hash, @Salt)";
                //inserts new Username, new salted and hashed password and a new salt into the database
                MySqlConnection connection = Connect();
                try
                {
                    MySqlCommand command = new MySqlCommand(command_text, connection); //creates the new sql command

                    command.Parameters.AddWithValue("@name", name);                //
                    command.Parameters.AddWithValue("@Hash", Hash_Salt.Get_Hash());// Parameterises query
                    command.Parameters.AddWithValue("@Salt", Hash_Salt.Get_Salt());//

                    connection.Open();
                    MySqlDataReader read = command.ExecuteReader(); //executes command
                    connection.Close();
                }

                catch (Exception error)
                {
                    Debug.WriteLine(error.ToString()); //shows error in debug window
                }
            }
            else
            {
                Debug.WriteLine("\nUsername taken");
            }
        }

        //retrieves list of existing games in the database
        public void Get_Game_List(string GameUserID)
        {
            ObservableCollection<GameListDisplay> Name_List = new ObservableCollection<GameListDisplay>();
            LinkedList<string> Name_List_String = new LinkedList<string>();
            string command_text = @"SELECT GameName FROM GameData WHERE GameUserName = @ID"; //Statement retrieves all the names of games saved by the user that is currently logged in

            MySqlConnection connection = Connect();
            try
            {
                MySqlCommand command = new MySqlCommand(command_text, connection);
                command.Parameters.AddWithValue("@ID", GameUserID); //parameterises sql query
                connection.Open();

                using (MySqlDataReader dataReader = command.ExecuteReader()) //excecutes command
                {
                    while (dataReader.Read())
                    {
                        Name_List.Add(new GameListDisplay(dataReader["GameName"].ToString())); //adds the retrieved game names to a the ObsevableCollection
                        Name_List_String.AddLast(dataReader["GameName"].ToString()); //adds the retrieved game names to the list of game names
                        NotifyPropertyChanged("GameList"); //notifies the UI that GameList has been changed
                    }
                    //GameList = Name_List; //sets GameList equal to the contents of the observable collection
                    StringListGameName = Name_List_String; //sets the class attribute StringListGameName to the contents of the Name_List_String
                    Quicksort sorting = new Quicksort();

                    if (StringListGameName == null)
                    {
                        GameList = new ObservableCollection<GameListDisplay>();
                        return;
                    }

                    StringListGameName = sorting.sort(StringListGameName); //sorts the strings using the quicksort algorithm

                    ObservableCollection<GameListDisplay> temp = new ObservableCollection<GameListDisplay>();
                    foreach (string x in StringListGameName)
                    {
                        temp.Add(new GameListDisplay(x));
                    }
                    GameList = temp;
                }
            }

            catch (Exception error)
            {
                Debug.WriteLine(error.ToString()); //writes errors to the debug window
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
