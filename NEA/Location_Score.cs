using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    //This class packages the current location of a piece and the location that this piece will be moved to 
    // along with a score corresponding to this move
    public class Location_Score
    {
        public Location Current_Location { get; set; } //the current location of the piece which will be moved
        public Location Move_Location { get; set; } //the location on the board that this piece will be moved to
        public int Score { get; set; } //the score correspnding to this move

        //initialises object with empty values
        public Location_Score()
        { }

        //initializes the object based on given values
        public void Location_Score_Set(Location Current, Location Move, int thescore)
        {
            Current_Location = Current;
            Move_Location = Move;
            Score = thescore;
        }

        //retrieves the current location 
        public Location Get_Current()
        {
            return Current_Location;
        }
        
        //retrieves the location to be moved to 
        public Location Get_Move()
        {
            return Move_Location;
        }
        
        //retrieves the score
        public int Get_Score()
        {
            return Score;
        }

        //sets all the attributes to be the same as the attributes of another Location_Score object
        public void Location_Score_Set_All(Location_Score L_S)
        {
            Current_Location = L_S.Get_Current();
            Move_Location = L_S.Get_Move();
            Score = L_S.Get_Score();
        }
    }
}
