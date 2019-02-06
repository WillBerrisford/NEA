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

        public void Save_Game(bool SignedIn, string UserName)
        {
            if (SignedIn == true)
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
                SqlConnection connection = Connect();
                string command_text = @"INSERT INTO GameData (GameInstance, GameUserID) " +
                    "Values ('" + xml + "', '" + UserName + "')";

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
}
