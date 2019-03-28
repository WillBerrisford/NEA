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
        public int X_coordinate { get; set; } //the x coordinate on the grid
        public int Y_coordinate { get; set; } //the y coordinate on the grid

        public Location()
        { }

        //initialised location according to the X and Y coordinates that it is given
        public Location(int X_coord, int Y_coord) 
        {
            X_coordinate = X_coord;
            Y_coordinate = Y_coord;
        }

        //sets the X coordinate individually 
        public void Set_x(int X_coord) 
        {
            X_coordinate = X_coord;
        }

        //sets the Y coordinate invdividually
        public void Set_y(int Y_coord)
        {
            Y_coordinate = Y_coord;
        }

        //retrieces the X coordinate
        public int Get_x()
        {
            return X_coordinate;
        }
        
        //retrieves the Y coordinate
        public int Get_y()
        {
            return Y_coordinate;
        }
    }
}
