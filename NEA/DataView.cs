using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    [System.Serializable] //this allows the serialiser to serialize all public parts of this object
    [System.Xml.Serialization.XmlInclude(typeof(Board_Grid))] //This specifically tells the serializer that the Board_Grid class can and should be serialized
    [System.Xml.Serialization.XmlInclude(typeof(Flow_Control))] //This specifically tells the serializer that the Flow_Control class can and should be serialized
    [System.Xml.Serialization.XmlInclude(typeof(Account))] //This specifically tells the serializer that the Account class can and should be serialized
    public class DataView //a wrapper class containing the grid and flow control 
    {
        public Board_Grid thegrid { get; set; }
        public Flow_Control Control { get; set; }
        public Account theaccount { get; set; }

        public DataView()
        {
            thegrid = new Board_Grid(true);
            Control = new Flow_Control(true);
            theaccount = new Account();           
        }

        public Board_Grid Get_Grid()
        {
            return thegrid;
        }
    }
}
