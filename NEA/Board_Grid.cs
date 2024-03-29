﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;

namespace NEA
{
    [System.Serializable] 
    [System.Xml.Serialization.XmlInclude(typeof(List<Unit>))] //allows the object to be serialized into an xml string
    public class Board_Grid : INotifyPropertyChanged
    {     
        public List<Unit> Grid_List { get; set; }  //The Grid_List where all the units are stored

        public event PropertyChangedEventHandler PropertyChanged; //updates the UI

        public List<Unit> get_Grid()
        {
            return Grid_List;
        }

        public Board_Grid()
        { }

        //initialises the grid
        public Board_Grid(bool run)
        {
            Grid_List = new List<Unit>();
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++) //runs through all the possible grid squares on the board
                {
                    //sets default values for non-team squares
                    int Given_Strength = 0;
                    bool In_Play = false;
                    int Given_Team = 3;
                    string Given_Team_Colour = "white";
                    string Given_Piece_Colour = "white";

                    //sets default values for team 2 squares
                    if (y == 4)
                    {
                        Given_Strength = set_strength(x);
                        In_Play = true;
                        Given_Team = 2;
                        Given_Team_Colour = "orange";
                        Given_Piece_Colour = "Red";
                    }

                    //sets default values for team 1 squares
                    else if (y == 0)
                    {
                        Given_Strength = set_strength(x);
                        In_Play = true;
                        Given_Team = 1;
                        Given_Team_Colour = "Lightblue";
                        Given_Piece_Colour = "blue";
                    }

                    Location Given_Location = new Location(x, y); //creates a location so it can be assigned to a unit
                    Grid_List.Add(new Unit(Given_Location, Given_Strength, In_Play, Given_Team, Given_Team_Colour, Given_Piece_Colour)); //adds the piece to the board
                }
            }
        }

        public List<Unit> Clone() //clones the grid to a new object
        {
            List<Unit> thegrid = new List<Unit>();
            for (int i = 0; i < 25; i++) //this creates an identical copy of every unit in the current grid and puts it in the new grid
            {
                Unit theunit = new Unit(Grid_List[i].Get_Location(), Grid_List[i].Get_Strength(), Grid_List[i].Get_Occupied(), Grid_List[i].Get_Team(), Grid_List[i].Get_Colour(), Grid_List[i].Get_Piece());
                thegrid.Add(theunit);
            }
            return thegrid;
        }

        //sets strength based on starting x coordinate
        private int set_strength(int x)
        {
            int Given_Strength;

            if (x == 4 || x == 0) 
            {
                Given_Strength = 2;
            }
            else if (x == 1 || x == 3)
            {
                Given_Strength = 3;
            }
            else
            {
                Given_Strength = 5;
            }

            return Given_Strength;
        }

        //counts number of squares owned by each team
        public Squares Count_squares()
        {
            Squares Team_squares = new Squares();

            for (int position = 0; position < 25; position++)
            {
                int team = Grid_List[position].Get_Team(); //gets the team of a given square
                if (team == 1)
                {
                    Team_squares.Add_Team_One();
                }

                else if (team == 2)
                {
                    Team_squares.Add_Team_Two();
                }

                else
                {
                    Team_squares.Add_Neutral();
                }

            }

            return Team_squares;
        }

        //Checks whether the move is valid
        public bool Check_Move(int Current_Index, int Move_Index)
        {
            bool valid_move = false;

            //Checks if the position that is selected contains a unit.
            if (Grid_List[Current_Index].Get_Occupied() == false)
            {
                return valid_move;
            }

            else
            {
                if (Move_Index < 25 && Move_Index > 0)//checks the unit is trying to move somewhere within the grid
                {
                    List<Location> Possible_Locations = Get_Possible_Locations(Current_Index); //gets a list of locations that the current piece is permitted to move to
                    for (int i = 0; i < 8; i++)
                    {
                        //checks whether location being moved to is in the list of allowed locations 
                        if (Possible_Locations[i].Get_x() == Grid_List[Move_Index].Get_Location().Get_x() && Possible_Locations[i].Get_y() == Grid_List[Move_Index].Get_Location().Get_y())
                        {
                            //if move position is the same team, checks that it is not occupied
                            if (Grid_List[Move_Index].Get_Team() == Grid_List[Current_Index].Get_Team() && Grid_List[Move_Index].Get_Occupied() == true)
                            { }
                            else
                            {                              
                                return true;
                            }
                        }
                        else
                        {}
                    }
                }
            }

            return valid_move;
        }

        //finds all possible legal locations for a move
        public List<Location> Get_Possible_Locations(int Current_Index)
        {
            List<Location> Possible_Locations = new List<Location>(); //list of locations that it is possible to move to

            int x = Grid_List[Current_Index].Get_Location().Get_x(); //current x coordinate of unit
            int y = Grid_List[Current_Index].Get_Location().Get_y(); //current y coordinate of unit

            y = y + 1; //the square above the unit
            Possible_Locations.Add(new Location(x, y));

            x = x + 1; //the square diagonally above and to the right of the unit
            Possible_Locations.Add(new Location(x, y));

            y = y - 1;//the square to the right of the unit
            Possible_Locations.Add(new Location(x, y));

            y = y - 1;//the square diagonally below and to the right of the unit
            Possible_Locations.Add(new Location(x, y));

            x = x - 1;//the square below the unit
            Possible_Locations.Add(new Location(x, y));

            x = x - 1;//the square diagonally below and to the left of the unit
            Possible_Locations.Add(new Location(x, y));

            y = y + 1;//the square to the left of the unit
            Possible_Locations.Add(new Location(x, y));

            y = y + 1;//the square diagonally above and to the left of the unit
            Possible_Locations.Add(new Location(x, y));

            return Possible_Locations;
        }

        //checks whether moving to a certain location would require an attack to happen
        public bool Check_Attack(int Move_Index, int Current_Index)
        {
            //checks if a square is occupied and if so it checks whether the unit is on the opposite team
            if (Grid_List[Move_Index].Get_Occupied() == true && Grid_List[Move_Index].Get_Team() != Grid_List[Current_Index].Get_Team())
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        //Moves one piece into another location.  Attacking another piece if it needs to.
        public bool Move(Location Current, Location Move_To, Score thescore)
        {
            int Current_Index = Get_Index(Current);
            int Move_Index = Get_Index(Move_To);
            bool success = false;

            if (Check_Move(Current_Index, Move_Index) == true) //checks that the move is legal
            {
                if (Check_Attack(Move_Index, Current_Index) == true) //checks whether an attack is required
                {
                    Attack(Current_Index, Move_Index, thescore); //carries out an attack interaction between the two units
                    success = true;
                }

                else
                {
                    Move_Units(Current_Index, Move_Index); //moves the unit into the new position
                    success = true;
                }
            }

            else
            {  }
            return success;
        }

        //This does the actual moving of the values of the variables from the original position to the new position on the grid.
        //Changes the values of the original grid, keeping the same team as the piece that moved away, but removing the piece itself.
        public void Move_Units(int Current_Index, int Move_Index)
        {
            Grid_List[Move_Index].Move_Update(Grid_List[Move_Index].Get_Location(), Grid_List[Current_Index].Get_Strength(), Grid_List[Current_Index].Get_Occupied(), Grid_List[Current_Index].Get_Team(), Grid_List[Current_Index].Get_Colour(), Grid_List[Current_Index].Get_Piece());
            Grid_List[Current_Index].Move_Update(Grid_List[Current_Index].Get_Location(), 0, false, Grid_List[Current_Index].Get_Team(), Grid_List[Current_Index].Get_Colour(), "transparent");
        }

        //One unit attacks another
        public void Attack(int Current_Index, int Move_Index, Score thescore)
        {
            //Attacking unit wins
            if (Grid_List[Current_Index].Get_Strength() > Grid_List[Move_Index].Get_Strength())
            {
                Grid_List[Move_Index].Killed(); //destroys defending unit
                thescore.add_point(Grid_List[Current_Index].Get_Team()); //add points to the team of the attacking unit
                Move_Units(Current_Index, Move_Index); //moves the attacking unit to where the defending unit was
            }

            //Defending unit wins
            else if (Grid_List[Current_Index].Get_Strength() < Grid_List[Move_Index].Get_Strength())
            {
                Grid_List[Move_Index].Defense_Success(); //makes the change in the strength value after a successful defense
                Grid_List[Current_Index].Attack_Loss(); //makes the change in the strength value after a failed attack 
            }

            //Draw
            else if (Grid_List[Current_Index].Get_Strength() == Grid_List[Move_Index].Get_Strength())
            {
                Grid_List[Move_Index].Defense_Draw(); //
                Grid_List[Current_Index].Attack_Draw(); //changes strength values accordingly
            }
        }

        //Find the index where the values of the given location = the values of a location in grid list
        public int Get_Index(Location The_Location)
        {
            return Grid_List.FindIndex(x => x.Get_Location().Get_x() == The_Location.Get_x() && x.Get_Location().Get_y() == The_Location.Get_y());
        }

        //Notify the UI that a property has changed
        public void NotifyPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
