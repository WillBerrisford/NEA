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
        Location Current_Location { get; set; } //The current location of the unit that will be moved
        Location Move_Location { get; set; } //the location that the unit will be moved to
        List<Tree_Node> Tree_List { get; set; } //the first level of the tree (depth 0)

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
            List<Tree_Node> Tree_List = Generate_Full_Tree(thegrid, thesystem_score, thescore); //generates a new complete tree
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
                    Best_Node = Tree_List[index]; //retrieves the node in which the best score is held
                }
                Debug.WriteLine(Score);
            }
            Debug.WriteLine("Used Score:");
            Debug.WriteLine(min);

            //the current and move locations are set according to the best possible move that the player could make
            Current_Location = Best_Node.Get_Current_Location();
            Move_Location = Best_Node.Get_Move_Location(); 
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

                    int score_0 = MiniMax(Tree_Node.Child_Nodes_List[index - 1], true); //retrieves the score of the first node in the list of child nodes

                    int score_1 = MiniMax(Tree_Node.Child_Nodes_List[index], true); //retrieves the score of the second node in the list of child nodes

                    if (score_1 > score_0) //compares the 2 nodes
                    {
                        if (score_1 > max) //if score 1 is bigger
                        {
                            max = score_1; //sets maximum value to score 1
                        }
                        else if (score_1 == max) //if score 1 equals max then max remains the same
                        { }
                    }

                    else if(score_1 < score_0) //compares the 2 nodes
                    {
                        if (score_0 > max)
                        {
                            max = score_0; 
                        }
                        else if (score_0 == max)
                        { }
                    }

                    else //this runs if the 2 nodes both have the same score
                    {
                        if (score_1 > max) //checks that the score is above max
                        {
                            max = score_1;
                        }
                        else if (score_1 == max)
                        { }
                    }
                    
                }
                return max; //returns the highest possible score from the two individual nodes
            }

            else
            {
                //this player is trying to minimise the score
                int min = 9999; //sets the minimum value of the score
                for (int index = 1; index < Tree_Node.Child_Nodes_List.Count(); index++) //runs for each child node present in the current parent node
                {

                    int score_0 = MiniMax(Tree_Node.Child_Nodes_List[index -1], false); //retrieves the score of the first node in the list of child nodes

                    int score_1 = MiniMax(Tree_Node.Child_Nodes_List[index], false);//retrieves the score of the second node in the list of child nodes

                    if (score_1 < score_0) //compares the 2 nodes
                    {
                        if (score_1 < min)
                        {
                            min = score_1;
                        }
                        else if (score_1 == min)
                        { }
                    }

                    else if (score_1 > score_0) //compares the 2 nodes
                    {
                        if (score_0 < min)
                        {
                            min = score_0;
                        }
                        else if (score_0 == min)
                        { }
                    }

                    else //runs if both scores are the same
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

        //creates a new level 0 tree node (each of these will be in of this list Tree_List because they have a depth of 0)
        public Tree_Node Create_0_Node(List<Tree_Node> Tree_List, Board_Grid thegrid, System_Scoring thesystem_score, Score thescore)
        {
            Board_Grid_AI Copy_thegrid = new Board_Grid_AI(); //creates new grid in the AI version
            Copy_thegrid.Grid_List = thegrid.Clone(); //clones the grid list of the grid
            Tree_Node Node_0 = new Tree_Node(Copy_thegrid, null, null, 0, thesystem_score.Get_System_Score(), 0, null); //creates a new tree node using the system score that was generate using the given Board_Grid
            for (int index = 0; index < Tree_List.Count; index++) //runs for each child node in the tree list
            {
                Node_0.Add_Tree_Node(Tree_List[index], Tree_List[index].Get_ID()); //adds a new tree node to the child nodes of node 0
            }

            return Node_0;
        }

        //generates a new tree (down to a depth of 4) from the depth 0 trees in Tree_List
        public List<Tree_Node> Generate_Full_Tree(Board_Grid thegrid, System_Scoring thesystem_score, Score thescore)
        {
            Board_Grid_AI Copy_thegrid = new Board_Grid_AI();
            Copy_thegrid.Grid_List = thegrid.Clone(); //copies the grid list from the main grid

            List<Tree_Node> Tree_List = Create_Tree_Root(Copy_thegrid, thesystem_score, thescore); //layer depth 1 //creates new set of trees

            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int tree_index_1 = 0; tree_index_1 < Tree_List.Count; tree_index_1++) //for each possible move, a tree node is added
            {
                //adds a new layer of child node to an existing tree node at a depth of 2
                Tree_List[tree_index_1] = Create_Tree(Tree_List[tree_index_1]); 

               for (int tree_index_2 = 0; tree_index_2 < Tree_List[tree_index_1].Child_Nodes_List.Count; tree_index_2++)
                {
                    //adds a new layer of child node to an existing tree node at a depth of 3
                    Tree_List[tree_index_1].Child_Nodes_List[tree_index_2] = Create_Tree(Tree_List[tree_index_1].Child_Nodes_List[tree_index_2]); 

                    for (int tree_index_3 = 0; tree_index_3 < Tree_List[tree_index_1].Child_Nodes_List[tree_index_2].Child_Nodes_List.Count; tree_index_3++)
                    {
                        //adds a new layer of child node to an existing tree node at a depth of 4
                        Tree_List[tree_index_1].Child_Nodes_List[tree_index_2].Child_Nodes_List[tree_index_3] = Create_Tree(Tree_List[tree_index_1].Child_Nodes_List[tree_index_2].Child_Nodes_List[tree_index_3]);
                    }
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Tree_List; //returns the fully created list
        }

        //creates a list of tree nodes (one node for every possible move on the board)
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
                            Possible_Locations.RemoveAt(the_Index); //removes illegal moves
                        }
                    }

                    List<Board_Grid_AI> Possible_Boards = Generate_Boards(thegrid, thegrid.Grid_List[i].Get_Location(), Possible_Locations, thescore);
                    //generates a list of every possible legal board (made up of every legal move from the current possition)
                    int Board_Index = Possible_Boards.Count() - 1;
                    for (int the_Index = Board_Index; the_Index >= 0; the_Index--) //runs through all the possible boards
                    {
                        if (Possible_Boards[the_Index] == null) 
                        {
                            Possible_Boards.RemoveAt(the_Index); //removes invalid boards that are null
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

        //creates an individual node of a tree using the tree node above it
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
            // if even the team = 2, if odd the team = 1
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
                if (thegrid.Grid_List[i].Get_Occupied() == true && thegrid.Grid_List[i].Get_Team() == turn) //checks that the square is occupied and that it contains a peice on the correct team
                {
                    List<Location> Possible_Locations = Moves(thegrid, i); //Creates list of possible locations to move to

                    int Location_Index = Possible_Locations.Count()- 1;
                    for (int the_Index = Location_Index; the_Index > -1; the_Index--) //removes moves that are outside the board
                    {
                        if (Possible_Locations[the_Index].Get_x() > 4 || Possible_Locations[the_Index].Get_x() < 0 || Possible_Locations[the_Index].Get_y() > 4 || Possible_Locations[the_Index].Get_y() < 0) //removes locations that are not on the board
                        {
                            Possible_Locations.RemoveAt(the_Index); //removes the illegal moves
                        }
                    }

                    //generates a list of possible boards from the list of possible locations
                    List<Board_Grid_AI> Possible_Boards = Generate_Boards(thegrid, thegrid.Grid_List[i].Get_Location(), Possible_Locations, thescore);

                    int Board_Index = Possible_Boards.Count() - 1;
                    for (int the_Index = Board_Index; the_Index >= 0; the_Index--) //removes invalid boards
                    {
                        //removes invalid boards with null values
                        if (Possible_Boards[the_Index] == null)
                        {
                            Possible_Boards.RemoveAt(the_Index);
                            Possible_Locations.RemoveAt(the_Index);
                        }
                    }
                    List<int> Possible_S_Scores = Scores(Possible_Boards, thesystem_score); //generates possible system scores from the possible boards

                    //adds node to the parent tree
                    for (int index = 0; index < Possible_Boards.Count(); index++)
                    {
                        Tree.Add_Tree(Possible_Boards[index], thegrid.Grid_List[index].Get_Location(), Possible_Locations[index], id, Possible_S_Scores[index]);
                        id++;
                    }
                }
            }          
            return Tree;
        }

        //generates all the possible legal boards from the current location and the list of possible locations
        List<Board_Grid_AI> Generate_Boards(Board_Grid_AI thegrid, Location Current_Location, List<Location> Possible_Locations, Score thescore)
        {
            List<Board_Grid_AI> Possible_Boards = new List<Board_Grid_AI>();

            for (int i = 0; i < Possible_Locations.Count(); i++) //generates a new board for every possible location
            {
                Board_Grid_AI New_Grid = new Board_Grid_AI();
                New_Grid.Grid_List = thegrid.Clone(); //clones the grid list into a Board_Grid_AI object
                if (New_Grid.Move(Current_Location, Possible_Locations[i], thescore) == true) //checks if the move is legal
                {
                    Possible_Boards.Add(New_Grid); //adds legal move to the list of possible boards
                }
                else
                {
                    Possible_Boards.Add(null); //adds a null value if the move is illegal
                }
            }
            return Possible_Boards;
        }

        //generates all the possible moves for a particular piece to make
        List<Location> Moves(Board_Grid_AI thegrid, int i)
        {
            List<Location> Possible_Locations = new List<Location>(); //list of locations that it is possible to move to

            int x = thegrid.Grid_List[i].Get_Location().Get_x(); //current x coordinate of unit
            int y = thegrid.Grid_List[i].Get_Location().Get_y(); //current y coordinate of unit

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

        //generates a list of possible system scores from a list of possible boards
        List<int> Scores(List<Board_Grid_AI> Board_List, System_Scoring thesystem_score)
        {
            List<int> Possible_Scores_List = new List<int>();

            for (int i = 0; i < Board_List.Count(); i++) //runs through all the boards
            {
                Board_Grid_AI Possible_Board = new Board_Grid_AI();
                Possible_Board.Grid_List = Board_List[i].Clone(); //copies the Grid list to the new object
                System_Scoring Possible_Score = thesystem_score.Clone();//copies the system score to the new object

                Squares squares = Possible_Board.Count_squares(); //counts number of squares that both teams own
                Possible_Score.Calculate_Score_AI(Board_List[i]); //adds points based on squares controlled
                Possible_Scores_List.Add(Possible_Score.Get_System_Score()); //adds the calculated score to the list of scores
            }
            return Possible_Scores_List;
        }

    }
}
