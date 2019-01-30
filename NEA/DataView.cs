using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    class DataView //a wrapper class containing the grid and flow control 
    {
        public Board_Grid thegrid { get; set; }
        public Flow_Control Control { get; set; }

        public DataView()
        {
            thegrid = new Board_Grid();
            Control = new Flow_Control();
        }

        public Board_Grid Get_Grid()
        {
            return thegrid;
        }
    }
}
