using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NEA
{
    public class Location
    {
        private int X_coordinate { get; set; } //the x coordinate on the grid
        private int Y_coordinate { get; set; } //the y coordinate on the grid

        public Location()
        { }

        public Location(int X_coord, int Y_coord) //initialised location
        {
            X_coordinate = X_coord;
            Y_coordinate = Y_coord;
        }

        public void Set_x(int X_coord)
        {
            X_coordinate = X_coord;
        }

        public void Set_y(int Y_coord)
        {
            Y_coordinate = Y_coord;
        }

        public int Get_x()
        {
            return X_coordinate;
        }

        public int Get_y()
        {
            return Y_coordinate;
        }
    }

    class Location_Score
    {
        Location Current_Location { get; set; }
        Location Move_Location { get; set; }
        int Score { get; set; }

        public Location_Score()
        {}

        public void Location_Score_Set(Location Current, Location Move, int thescore)
        {
            Current_Location = Current;
            Move_Location = Move;
            Score = thescore;
        }

        public Location Get_Current()
        {
            return Current_Location;
        }

        public Location Get_Move()
        {
            return Move_Location;
        }

        public int Get_Score()
        {
            return Score;
        }

        public void Location_Score_Set_All(Location_Score L_S)
        {
            Current_Location = L_S.Get_Current();
            Move_Location = L_S.Get_Move();
            Score = L_S.Get_Score();
        }
    }
}
