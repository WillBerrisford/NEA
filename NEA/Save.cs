using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NEA
{
    class Save : Database_Connect
    {
        public DataView Data { get; set; }

        public Save(DataView data)
        {
            Data = data;
        }

        public bool Check_Game_Names(string GameName, string UserName)
        {
            string command_text = @"SELECT GameName FROM GameData WHERE GameUserID=(SELECT ID FROM Users_2 WHERE UserNames=@UserName)";
            SqlConnection connection = Connect();
            try
            {
                SqlCommand command = new SqlCommand(command_text, connection);
                command.Parameters.AddWithValue("@UserName", UserName);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.WriteLine(reader["GameName"].ToString());
                        if (GameName == reader["GameName"].ToString())
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
                return true;
            }
        }

        public void Save_Game(bool SignedIn, string UserName, string ID, string GameName)
        {
            if (SignedIn == true && Check_Game_Names(GameName, UserName) == true)
            {
                XmlSerializer Serialize_xml = new XmlSerializer(Data.GetType());
                var xml = "";

                using (var string_write = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(string_write))
                    {
                        Serialize_xml.Serialize(writer, Data);
                        xml = string_write.ToString(); // Your XML
                        Debug.WriteLine("The XML string" + xml);
                    }
                }

                int i_ID = Convert.ToInt32(ID);
                SqlConnection connection = Connect();
                string command_text = @"INSERT INTO GameData (GameInstance, GameUserName, GameUserID, GameName) " +
                    "Values (@xml, @UserName, @i_ID, @GameName)";

                try
                {
                    SqlCommand command = new SqlCommand(command_text, connection);

                    command.Parameters.AddWithValue("@xml", xml);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@i_ID", i_ID);
                    command.Parameters.AddWithValue("@GameName", GameName);

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
                Debug.WriteLine("Save Did not work");
            }
        }

        public string Get_XML_String(string GameName, string GameUserID)
        {
            string xml_string;
            string command_text = @"SELECT GameInstance FROM GameData WHERE GameUserID=@GameUserID AND GameName=@GameName";

            SqlConnection connection = Connect();
            try
            {
                SqlCommand command = new SqlCommand(command_text, connection);
                command.Parameters.AddWithValue("@GameUserID", Convert.ToInt32(GameUserID));
                command.Parameters.AddWithValue("@GameName", GameName);
                connection.Open();

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        return xml_string = dataReader["GameInstance"].ToString();
                    }
                }
                return null;
            }

            catch (Exception error)
            {
                Debug.WriteLine(error.ToString());
                return null;
            }
        }

        public DataView Deserialize(string xml_string)
        {
            XmlSerializer reader = new XmlSerializer(typeof(DataView));
            StringReader xml = new StringReader(xml_string);
            DataView data = (DataView)reader.Deserialize(xml);
            xml.Close();
            return data;
        }

        public DataView Load_Game(string GameName, string GameUserID)
        {

            string xml_string = Get_XML_String(GameName, GameUserID);
            if (xml_string == null)
            {
                Debug.WriteLine("xml_string == null, name may be wrong");
            }
            else
            {
                DataView Data = Deserialize(xml_string);
                return Data;
            }
            return null;
        }    

    }
}
