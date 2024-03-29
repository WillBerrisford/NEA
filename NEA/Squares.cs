﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NEA
{
    public class Squares
    {
        public int Team_one { get; set; } //number of squares controlled by team 1
        public int Team_two { get; set; }//number of squares controlled by team 2
        public int Neutral { get; set; }//number of squares that are neutral

        public Squares() //initialises class with all values at 0
        {
            Team_one = 0;
            Team_two = 0;
            Neutral = 0;
        }

        public void Add_Team_One() //adds one to team 1
        {
            Team_one = Team_one + 1;
        }

        public void Add_Team_Two() //adds one to team 2
        {
            Team_two = Team_two + 1;
        }

        public void Add_Neutral() //adds one to neutral
        {
            Neutral = Neutral + 1;
        }

        public int Get_Team_One() //retrieves the number of squares controlled by team 1
        {
            return Team_one;
        }

        public int Get_Team_Two() //retrieves the number of squares controlled by team 2
        {
            return Team_two;
        }

        public int Get_Neutral() //retrieves the number of squares not controlled by either player
        {
            return Neutral;
        }
    }
}
