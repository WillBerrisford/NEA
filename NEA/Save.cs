using System;
using System.Collections.Generic;
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
        private DataView Data { get; set; }

        public Save(DataView data)
        {
            Data = data;
        }

        public void Save_Game()
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(DataView));
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, Data);
                    xml = sww.ToString(); // Your XML
                    Debug.WriteLine("The XML string" + xml);
                }
            }
        }

    }
}
