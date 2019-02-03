using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    class System_Scoring
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

        public System_Scoring Clone()
        {
            System_Scoring New_Score = new System_Scoring();
            New_Score.Set_Score(score);
            return New_Score;
        }

        public int Calculate_Score(Board_Grid possible_grid)
        {
            for (int i = 0; i > 25; i++) //iterates through each square of the grid
            {
                if (possible_grid.Grid_List[i].Get_Team() == 1) 
                {
                    score = score + 5; //adds 5 points per location owned by team
                    if (possible_grid.Grid_List[i].Get_Occupied() == true) //if occupied by a piece it adds the strength of the piece onto the score
                    {
                        score = score + possible_grid.Grid_List[i].Get_Strength();
                    }
                }              

                else if (possible_grid.Grid_List[i].Get_Team() == 2)
                {
                    score = score - 5; //adds 5 points per location owned by team
                    if (possible_grid.Grid_List[i].Get_Occupied() == true)
                    {
                        score = score - possible_grid.Grid_List[i].Get_Strength();
                    }
                }
            }
            return score;
        }

        public int Calculate_Score_AI(Board_Grid_AI possible_grid)
        {
            for (int i = 0; i < 25; i++) //iterates through each square of the grid
            {
                if (possible_grid.Grid_List[i].Get_Team() == 1) //team 1 (blue) is trying to max score
                {
                    score = score + 7; //adds 5 points per location owned by team
                    if (possible_grid.Grid_List[i].Get_Occupied() == true) //if occupied by a piece it adds the strength of the piece onto the score
                    {
                        score = score + 2*(possible_grid.Grid_List[i].Get_Strength());
                    }
                }

                else if (possible_grid.Grid_List[i].Get_Team() == 2) //team 2 (red) is trying to minimise score
                {
                    score = score - 7; //adds 5 points per location owned by team
                    if (possible_grid.Grid_List[i].Get_Occupied() == true)
                    {
                        score = score - 2*(possible_grid.Grid_List[i].Get_Strength());
                    }
                }
            }
            return score;
        }

        public int Get_System_Score()
        { return score; }
    }
}
