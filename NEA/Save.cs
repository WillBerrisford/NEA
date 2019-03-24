using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Data.SqlClient;

namespace NEA
{
    //This class controls the saving and loading of date from the database
    class Save : Database_Connect
    {
        public DataView Data { get; set; }  //this is the encapsulating class that contains the account, the board and the flow control
                                            //having access to this allows access to the account details of the user
        public List<Unit> Unit_List { get; set; }   //this records the positions of all the pieces on the board

        public Save(DataView data)
        {
            Data = data;
        }

        public Save(List<Unit> units)
        {
            Unit_List = units;
        }

        //this functions checks if a username already exists in the database
        //if it does then false is returned, if no username is found then true is returned
        public bool Check_Game_Names(string GameName, string UserName)
        {
            //sql query selecting all game names where the game ID is the same as the user ID of the current user
            string command_text = @"SELECT GameName FROM GameData WHERE GameUserID=(SELECT ID FROM Users_2 WHERE UserNames=@UserName)";

            //starts a new connection
            MySqlConnection connection = Connect();
            try
            {
                MySqlCommand command = new MySqlCommand(command_text, connection); //creates the sql command
                command.Parameters.AddWithValue("@UserName", UserName); //parameterized the sql command
                connection.Open(); //opens the new connection

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) //while each line of the table that was returned is being read
                                            //the following code will be executed
                    {
                        Debug.WriteLine(reader["GameName"].ToString());
                        if (GameName == reader["GameName"].ToString()) 
                        {
                            //if the entry in the GameName column is the same as the given game name
                            return false;
                        }
                    }
                    return true;
                }
            }

            //catches and records any connection errors
            catch (Exception error)
            {
                Debug.WriteLine(error.ToString());
                return true;
            }
        }

        //this function saves the current state of the board and the scores alongside the name of this save game,
            // the user who saved the game, their id and the id of the actual game itself
        public void Save_Game(bool SignedIn, string UserName, string ID, string GameName)
        {
            //the following checks that a user is actually signed in and that their account exists in the database
            if (SignedIn == true && Check_Game_Names(GameName, UserName) == true)
            {
                //initialises a new xml serializer (which will convert the objects to xml)
                XmlSerializer Serialize_xml = new XmlSerializer(Data.GetType());
                var xml = "";

                using (var string_write = new StringWriter()) //creates a string writer for writing correctly to a string
                { 
                    using (XmlWriter writer = XmlWriter.Create(string_write)) //creates an xml writer for writing the correct xml
                    {
                        Serialize_xml.Serialize(writer, Data); //writes the entireity of the date object containing the account, 
                                                                //flow control and the board to an xml string
                        xml = string_write.ToString(); //converts the xml into string format
                        Debug.WriteLine("The XML string" + xml); 
                    }
                }

                int i_ID = Convert.ToInt32(ID); //converts the given ID into an integer
                MySqlConnection connection = Connect(); //initialises a new connection

                //creates a new sql query which saves the current game, the username of the current user, the current user's id and the
                    //user defined name of the game
                string command_text = @"INSERT INTO GameData (GameInstance, GameUserName, GameUserID, GameName) " +
                    "Values (@xml, @UserName, @i_ID, @GameName)";

                try
                {
                    MySqlCommand command = new MySqlCommand(command_text, connection); //creates a new sql command

                    command.Parameters.AddWithValue("@xml", xml);           //
                    command.Parameters.AddWithValue("@UserName", UserName); //
                    command.Parameters.AddWithValue("@i_ID", i_ID);         //Parameterises the sql command
                    command.Parameters.AddWithValue("@GameName", GameName); //

                    connection.Open(); //opens the connection
                    MySqlDataReader read = command.ExecuteReader(); //excecutes the command
                    connection.Close(); //closes the connection
                }

                //catches and records any errors that occure
                catch (Exception error)
                {
                    Debug.WriteLine(error.ToString());
                }
            }

            else
            {
                Debug.WriteLine("Save Did not work");
            }
        }

        //ths function retrieves the xml strings containing the data for a specific game from the server
        public string Get_XML_String(string GameName, string GameUserID)
        {
            string xml_string;

            //sql query selecting the instance of a game where the game name and user id's are the same as the game name's and id's provided
            string command_text = @"SELECT GameInstance FROM GameData WHERE GameUserID=@GameUserID AND GameName=@GameName";

            MySqlConnection connection = Connect(); //initialises new connection
            try
            {
                MySqlCommand command = new MySqlCommand(command_text, connection); //creates new sql command
                command.Parameters.AddWithValue("@GameUserID", Convert.ToInt32(GameUserID)); //parameterises sql
                command.Parameters.AddWithValue("@GameName", GameName);                      //
                connection.Open(); //opens the connection

                using (MySqlDataReader dataReader = command.ExecuteReader()) //allows the program to read the retrieved table record by record
                {
                    while (dataReader.Read()) //reads through the retrieved table record by record
                    {
                        Debug.WriteLine(dataReader["GameInstance"].ToString()); 
                        return xml_string = dataReader["GameInstance"].ToString(); //returns the first found xml string
                    }
                }
                //if no instances are returned the null is returned
                return null;
            }

            //catches and displays and errors that have occured
            catch (Exception error)
            {
                Debug.WriteLine(error.ToString());
                return null;
            }
        }

        //turns the xml string back into c# objects
        public DataView Deserialize(string xml_string)
        {
            XmlSerializer reader = new XmlSerializer(typeof(DataView)); //creates new reader able to read the DataView object
            StringReader xml = new StringReader(xml_string); //allows the program to read the xml
            DataView data = (DataView)reader.Deserialize(xml); //turns the xml back into a dataview object
            xml.Close(); //stops reading the xml
            return data; //returns the newly loaded object
        }

        //this loads a game from the server given the name of the particular game and the id of the current user
        public DataView Load_Game(string GameName, string GameUserID)
        {

            string xml_string = Get_XML_String(GameName, GameUserID); //retrieves the xml string of the game with that name 
                                                                        //which belongs to the correct user
            if (xml_string == null) //checks to see if valid xml has been returned
            {
                Debug.WriteLine("xml_string == null, name may be wrong");
            }
            else
            {
                //creates a new data object from the xml string
                DataView Data = Deserialize(xml_string);
                return Data;
            }
            return null;
        }    

    }
}
