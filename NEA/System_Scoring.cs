using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    //this class is used to calculate the internal score used by the AI to detrmine how favourable a particular move is
    public class System_Scoring
    {
        int score { get; set; }

        public System_Scoring()
        {
            score = 0;
        }

        public void Set_Score(int i)
        {
            score = i;
        }

        //this creates a new and identical system score object
        public System_Scoring Clone()
        {
            System_Scoring New_Score = new System_Scoring();
            New_Score.Set_Score(score);
            return New_Score;
        }

        //this generates a score based on the current layout of the given board
        //adds points for team 1 owned squares
        //takes points away for team 2 owned squares
        public int Calculate_Score(Board_Grid possible_grid)
        {
            for (int i = 0; i > 25; i++) //iterates through each square of the grid
            {   
                if (possible_grid.Grid_List[i].Get_Team() == 1) //checks if the current square is controlled by team 1
                {
                    score = score + 5; //adds 5 points per location owned by team
                    if (possible_grid.Grid_List[i].Get_Occupied() == true) //if occupied by a piece it adds the strength of the piece onto the score
                    {
                        score = score + possible_grid.Grid_List[i].Get_Strength(); //adds the strength of the piece onto the score
                    }
                }              

                else if (possible_grid.Grid_List[i].Get_Team() == 2) //checks if the current square is occupied by team 2
                {
                    score = score - 5; //takes away 5 points per location owned by team
                    if (possible_grid.Grid_List[i].Get_Occupied() == true)
                    {
                        score = score - possible_grid.Grid_List[i].Get_Strength(); //takes away strength of the squares from the point
                    }
                }
            }
            return score;
        }

        //this generates a score based on the current layout of the given board
        //adds points for team 2 owned squares
        //takes points away for team 1 owned squares
        public int Calculate_Score_AI(Board_Grid_AI possible_grid)
        {
            for (int i = 0; i < 25; i++) //iterates through each square of the grid
            {
                if (possible_grid.Grid_List[i].Get_Team() == 1) //team 1 (blue) is trying to max score
                {
                    score = score + 7; //adds 5 points per location owned by team
                    if (possible_grid.Grid_List[i].Get_Occupied() == true) //if occupied by a piece it adds the strength of the piece onto the score
                    {
                        score = score + 2*(possible_grid.Grid_List[i].Get_Strength()); //adds double the strength of the piece to the score
                    }
                }

                else if (possible_grid.Grid_List[i].Get_Team() == 2) //team 2 (red) is trying to minimise score
                {
                    score = score - 7; //takes away points per location owned by team
                    if (possible_grid.Grid_List[i].Get_Occupied() == true)
                    {
                        score = score - 2*(possible_grid.Grid_List[i].Get_Strength()); //takes away double the points of the strength of the piece
                    }
                }
            }
            return score;
        }

        public int Get_System_Score()
        { return score; }
    }
}
