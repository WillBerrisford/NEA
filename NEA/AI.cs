using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NEA
{
    public class AI
    {
        Location Current_Location { get; set; }
        Location Move_Location { get; set; }
        List<Tree_Node> Tree_List { get; set; }

        public AI()
        { }

        public Location Get_Current()
        {
            return Current_Location;
        }

        public Location Get_Move()
        {
            return Move_Location;
        }

        public void Run_AI(Board_Grid thegrid, System_Scoring thesystem_score, Score thescore)
        {
            List<Tree_Node> Tree_List = Generate_Full_Tree(thegrid, thesystem_score, thescore);
            Tree_Node Best_Node = new Tree_Node(null, null, null, 0, 0, 0, null); //initialises a tree node that can be written over by the best node in the algorithm
            Random rnd = new Random();

            //the lower the score the better
            int min = 9999; //sets the max score of the worst case scenario
            for (int index = 0; index < Tree_List.Count(); index++) //runs for every child node in the tree
            {
                int Score = MiniMax(Tree_List[index], false); //retrieves the best score of the child node of the tree, using the minimax algorithm
                if (Score < min) //checks if the score is lower than the previous minimum score
                {
                    min = Score; //sets the min as the score
                    Best_Node = Tree_List[index]; //retrieves the node in which the best score is held
                }
                if (Score == min) //runs if the score is the same as the minimum score
                {
                    min = Score;
                    Best_Node = Tree_List[index]; 
                }
                Debug.WriteLine(Score);
            }
            Debug.WriteLine("Used Score:");
            Debug.WriteLine(min);

            Current_Location = Best_Node.Get_Current_Location(); //sets the current location of the piece that needs to be moved
            Move_Location = Best_Node.Get_Move_Location(); //sets the location which the piece needs to move to
        }

        public int MiniMax(Tree_Node Tree_Node, bool MaxPLayer) 
        {
            if (Tree_Node.Get_Depth() == 4) //4 is the maximum depth of the tree
            {
                return Tree_Node.Get_Score(); //the returns the final score of the tree
            }

            Tree_Node Tree = Tree_Node; //creates new tree node
            if (MaxPLayer == false) //checks if the player is the maximising player or not
            {
                //this player is trying to maximise their score
                int max = -9999; //sets the initial maximum score 
                for (int index = 1; index < Tree_Node.Child_Nodes_List.Count(); index++)  //runs for every child node in the given tree
                {

                    int score_0 = MiniMax(Tree_Node.Child_Nodes_List[index - 1], true); //retrieves the score of the one node

                    int score_1 = MiniMax(Tree_Node.Child_Nodes_List[index], true); //retrieves the score of the second node

                    if (score_1 > score_0) //checks if score 1 is bigger 
                    {
                        if (score_1 > max) //if score 1 is bigger
                        {
                            max = score_1; //sets maximum value to score 1
                        }
                        else if (score_1 == max) //if score 1 equals max then max remains the same
                        { }
                    }

                    else if(score_1 < score_0) //if score 1 is smaller
                    {
                        if (score_0 > max)
                        {
                            max = score_0;
                        }
                        else if (score_0 == max)
                        { }
                    }

                    else
                    {
                        if (score_1 > max)
                        {
                            max = score_1;
                        }
                        else if (score_1 == max)
                        { }
                    }
                    
                }
                return max;
            }

            else
            {
                //this player is trying to minimise the score
                int min = 9999; //sets the minimum value of the score
                for (int index = 1; index < Tree_Node.Child_Nodes_List.Count(); index++) //runs for each child node present in the current parent node
                {

                    int score_0 = MiniMax(Tree_Node.Child_Nodes_List[index -1], false); 

                    int score_1 = MiniMax(Tree_Node.Child_Nodes_List[index], false);

                    if (score_1 < score_0)
                    {
                        if (score_1 < min)
                        {
                            min = score_1;
                        }
                        else if (score_1 == min)
                        { }
                    }

                    else if (score_1 > score_0)
                    {
                        if (score_0 < min)
                        {
                            min = score_0;
                        }
                        else if (score_0 == min)
                        { }
                    }

                    else
                    {
                        if (score_1 < min)
                        {
                            min = score_1;
                        }
                        else if (score_1 == min)
                        { }
                    }

                }
                return min;
            }
        }

        public Tree_Node Create_0_Node(List<Tree_Node> Tree_List, Board_Grid thegrid, System_Scoring thesystem_score, Score thescore)
        {
            Board_Grid_AI Copy_thegrid = new Board_Grid_AI(); //creates new grid in the AI version
            Copy_thegrid.Grid_List = thegrid.Clone(); //clones the grid list of the grid
            Tree_Node Node_0 = new Tree_Node(Copy_thegrid, null, null, 0, thesystem_score.Get_System_Score(), 0, null); //creates a new tree node
            for (int index = 0; index < Tree_List.Count; index++) //runs for each child node in the tree list
            {
                Node_0.Add_Tree_Node(Tree_List[index], Tree_List[index].Get_ID()); //adds a new tree node to the child nodes of node 0
            }

            return Node_0;
        }

        public List<Tree_Node> Generate_Full_Tree(Board_Grid thegrid, System_Scoring thesystem_score, Score thescore)
        {
            Board_Grid_AI Copy_thegrid = new Board_Grid_AI(); 
            Copy_thegrid.Grid_List = thegrid.Clone(); //copies the grid list from the main grid

            List<Tree_Node> Tree_List = Create_Tree_Root(Copy_thegrid, thesystem_score, thescore); //layer depth 1 //creates new set of trees

            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int tree_index_1 = 0; tree_index_1 < Tree_List.Count; tree_index_1++) //for each possible move, a tree node is added
            {
                Tree_List[tree_index_1] = Create_Tree(Tree_List[tree_index_1]); //layer depth 2

               for (int tree_index_2 = 0; tree_index_2 < Tree_List[tree_index_1].Child_Nodes_List.Count; tree_index_2++)
                {
                    Tree_List[tree_index_1].Child_Nodes_List[tree_index_2] = Create_Tree(Tree_List[tree_index_1].Child_Nodes_List[tree_index_2]); //layer depth = 3

                    for (int tree_index_3 = 0; tree_index_3 < Tree_List[tree_index_1].Child_Nodes_List[tree_index_2].Child_Nodes_List.Count; tree_index_3++)
                    {
                        Tree_List[tree_index_1].Child_Nodes_List[tree_index_2].Child_Nodes_List[tree_index_3] = Create_Tree(Tree_List[tree_index_1].Child_Nodes_List[tree_index_2].Child_Nodes_List[tree_index_3]); //layer depth = 4
                    }
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Tree_List;
        }

        List<Tree_Node> Create_Tree_Root(Board_Grid_AI thegrid, System_Scoring thesystem_score, Score thescore)
        {
            List<Tree_Node> New_Tree_List = new List<Tree_Node>();

            int id = 1;
            for (int i = 0; i < 25; i++) //runs through each square on the board 
            {
                if (thegrid.Grid_List[i].Get_Occupied() == true && thegrid.Grid_List[i].Get_Team() == 2) //checks if the grid position is occupied and if so, that the piece is on red team
                {
                    List<Location> Possible_Locations = Moves(thegrid, i); //sets possible locations as all the possible moves for the piece to make

                    int Location_Index = Possible_Locations.Count() - 1; 
                    for (int the_Index = Location_Index; the_Index > -1; the_Index--) //removes moves that are outside the board
                    {
                        if (Possible_Locations[the_Index].Get_x() > 4 || Possible_Locations[the_Index].Get_x() < 0 || Possible_Locations[the_Index].Get_y() > 4 || Possible_Locations[the_Index].Get_y() < 0) //removes locations that are not on the board
                        {
                            Possible_Locations.RemoveAt(the_Index);
                        }
                    }

                    List<Board_Grid_AI> Possible_Boards = Generate_Boards(thegrid, thegrid.Grid_List[i].Get_Location(), Possible_Locations, thescore);
                    //generates what the grids would look like if the moves were carrued out
                    int Board_Index = Possible_Boards.Count() - 1;
                    for (int the_Index = Board_Index; the_Index >= 0; the_Index--) //removes invalid boards
                    {
                        if (Possible_Boards[the_Index] == null) //removes invalid boards that are null
                        {
                            Possible_Boards.RemoveAt(the_Index);
                            Possible_Locations.RemoveAt(the_Index); //removes the locations from possible locations, meaning that that grid position cannot be moved to
                        }
                    }

                    List<int> Possible_Scores = Scores(Possible_Boards, thesystem_score); //generates a list of scores from each of the possible boards

                    for (int index = 0; index < Possible_Boards.Count(); index++) //for each possible board combination the for loop is run
                    {
                        Tree_Node New_Tree = new Tree_Node(Possible_Boards[index], thegrid.Grid_List[i].Get_Location(), Possible_Locations[index], id, Possible_Scores[index], 1, null); //creates a new tree node for each valid move
                        New_Tree_List.Add(New_Tree);
                        id++;
                    }
                }
            }
            return New_Tree_List;
        }

        Tree_Node Create_Tree(Tree_Node Tree)
        {
            Board_Grid_AI thegrid = new Board_Grid_AI();
            thegrid.Grid_List = Tree.Get_Grid().Clone(); //clones the grid list of the parent node's grid
            System_Scoring thesystem_score = new System_Scoring();
            thesystem_score.Set_Score(Tree.Get_Score()); //retrieves the score of the parent node
            Score thescore = new Score(); //not used just needs value to calculate new boards

            int depth = Tree.Get_Depth(); //gets the depth of the parent node
            int turn;

            if (depth % 2 == 0) //determines which team currently has its turn
            {
                turn = 2;
            }
            else
            {
                turn = 1;
            }

            int id = 0;
            for (int i = 0; i < 25; i++) //runs once for each location on the grid
            {
                if (thegrid.Grid_List[i].Get_Occupied() == true && thegrid.Grid_List[i].Get_Team() == turn)
                {
                    List<Location> Possible_Locations = Moves(thegrid, i); //Creates list of possible locations to move to

                    int Location_Index = Possible_Locations.Count()- 1;
                    for (int the_Index = Location_Index; the_Index > -1; the_Index--) //removes moves that are outside the board
                    {
                        if (Possible_Locations[the_Index].Get_x() > 4 || Possible_Locations[the_Index].Get_x() < 0 || Possible_Locations[the_Index].Get_y() > 4 || Possible_Locations[the_Index].Get_y() < 0) //removes locations that are not on the board
                        {
                            Possible_Locations.RemoveAt(the_Index);
                        }
                    }

                    List<Board_Grid_AI> Possible_Boards = Generate_Boards(thegrid, thegrid.Grid_List[i].Get_Location(), Possible_Locations, thescore);

                    int Board_Index = Possible_Boards.Count() - 1;
                    for (int the_Index = Board_Index; the_Index >= 0; the_Index--) //removes invalid boards
                    {
                        if (Possible_Boards[the_Index] == null)
                        {
                            Possible_Boards.RemoveAt(the_Index);
                            Possible_Locations.RemoveAt(the_Index);
                        }
                    }
                    List<int> Possible_S_Scores = Scores(Possible_Boards, thesystem_score);

                    for (int index = 0; index < Possible_Boards.Count(); index++)
                    {
                        Tree.Add_Tree(Possible_Boards[index], thegrid.Grid_List[index].Get_Location(), Possible_Locations[index], id, Possible_S_Scores[index]);
                        id++;
                    }
                }
            }          
            return Tree;
        }

        List<Board_Grid_AI> Generate_Boards(Board_Grid_AI thegrid, Location Current_Location, List<Location> Possible_Locations, Score thescore)
        {
            List<Board_Grid_AI> Possible_Boards = new List<Board_Grid_AI>();

            for (int i = 0; i < Possible_Locations.Count(); i++)
            {
                Board_Grid_AI New_Grid = new Board_Grid_AI();
                New_Grid.Grid_List = thegrid.Clone();
                if (New_Grid.Move(Current_Location, Possible_Locations[i], thescore) == true)
                {
                    Possible_Boards.Add(New_Grid);
                }
                else
                {
                    Possible_Boards.Add(null);
                }
            }
            return Possible_Boards;
        }

        List<Location> Moves(Board_Grid_AI thegrid, int i)
        {
            List<Location> Possible_Locations = new List<Location>(); //list of locations that it is possible to move to

            int x = thegrid.Grid_List[i].Get_Location().Get_x();
            int y = thegrid.Grid_List[i].Get_Location().Get_y();

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

        List<int> Scores(List<Board_Grid_AI> Board_List, System_Scoring thesystem_score)
        {
            List<int> Possible_Scores_List = new List<int>();

            for (int i = 0; i < Board_List.Count(); i++)
            {
                Board_Grid_AI Possible_Board = new Board_Grid_AI();
                Possible_Board.Grid_List = Board_List[i].Clone();
                System_Scoring Possible_Score = thesystem_score.Clone();

                Squares squares = Possible_Board.Count_squares(); //counts number of squares that both teams own
                Possible_Score.Calculate_Score_AI(Board_List[i]); //adds points based on squares controlled
                Possible_Scores_List.Add(Possible_Score.Get_System_Score()); //adds the calculated score to the list of scores
            }
            return Possible_Scores_List;
        }

    }
}
