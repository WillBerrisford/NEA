using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;

namespace NEA
{
    public class Board_Grid_AI : INotifyPropertyChanged
    {
        public List<Unit> Grid_List { get; set; }  //The Grid_List where all the units are stored

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Unit> get_Grid()
        {
            return Grid_List;
        }

        //initialises the grid
        public Board_Grid_AI()
        {
            Grid_List = new List<Unit>();
        }

        public List<Unit> Clone()
        {
            List<Unit> thegrid = new List<Unit>();

            for (int i = 0; i < 25; i++)
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
                int team = Grid_List[position].Get_Team();
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
                        { }
                    }
                }
            }

            return valid_move;
        }

        //finds all possible legal locations for a move
        public List<Location> Get_Possible_Locations(int Current_Index)
        {
            List<Location> Possible_Locations = new List<Location>();

            int x = Grid_List[Current_Index].Get_Location().Get_x();
            int y = Grid_List[Current_Index].Get_Location().Get_y();

            y = y + 1;
            Possible_Locations.Add(new Location(x, y));

            x = x + 1;
            Possible_Locations.Add(new Location(x, y));

            y = y - 1;
            Possible_Locations.Add(new Location(x, y));

            y = y - 1;
            Possible_Locations.Add(new Location(x, y));

            x = x - 1;
            Possible_Locations.Add(new Location(x, y));

            x = x - 1;
            Possible_Locations.Add(new Location(x, y));

            y = y + 1;
            Possible_Locations.Add(new Location(x, y));

            y = y + 1;
            Possible_Locations.Add(new Location(x, y));

            return Possible_Locations;
        }

        //checks whether moving to a certain location would require an attack to happen
        public bool Check_Attack(int Move_Index, int Current_Index)
        {
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

            if (Check_Move(Current_Index, Move_Index) == true)
            {
                if (Check_Attack(Move_Index, Current_Index) == true)
                {
                    Attack(Current_Index, Move_Index, thescore);
                    success = true;
                }

                else
                {
                    Move_Units(Current_Index, Move_Index);
                    success = true;
                }
            }

            else
            { }
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
                Grid_List[Move_Index].Killed();
                thescore.add_point(Grid_List[Move_Index].Get_Team());
                Move_Units(Current_Index, Move_Index);
            }

            //Defending unit wins
            else if (Grid_List[Current_Index].Get_Strength() < Grid_List[Move_Index].Get_Strength())
            {
                Grid_List[Move_Index].Defense_Success();
                Grid_List[Current_Index].Attack_Loss();
            }

            //Draw
            else if (Grid_List[Current_Index].Get_Strength() == Grid_List[Move_Index].Get_Strength())
            {
                Grid_List[Move_Index].Defense_Draw();
                Grid_List[Current_Index].Attack_Draw();
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
