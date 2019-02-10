using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    public class GameListDisplay
    {
        public string GameName { get; set; }

        public GameListDisplay()
        { }

        public GameListDisplay(string name)
        {
            GameName = name;
        }
    }
}
