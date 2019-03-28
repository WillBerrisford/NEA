using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    //this class represents a node containing two locations and the corresponding score
    public class Tree_Node : IEnumerable<Tree_Node>
    {
        Dictionary<int, Tree_Node> Child_Nodes { get; set; } //this dictionary asscociates a child tree node with the score for that tree node
        public List<Tree_Node> Child_Nodes_List { get; set; }//a dictionary of tree nodes contained by this tree node
                                                             //these nodes correspond to all the possible legal moves that could be made 
                                                             //from the starting position of the board in the current tree node

        int ID { get; set; } //this is the unique ID of the Tree Node
        int Score { get; set; } //this is the score of the Tree Node
        int Depth { get; set; } //this is the depth of the tree node 

        Board_Grid_AI thegrid { get; set; } //the grid associated with this tree node

        Location Current_Location { get; set; } //the current location of the piece which will be moved 
        Location Move_Location { get; set; } //the future location that the piece will be moved to

        public Tree_Node Parent { get; private set; } //the tree node that this node is the child of

        public Tree_Node()
        { }

        //intialises the tree node with given values
        public Tree_Node(Board_Grid_AI Board, Location Current, Location Move, int theid, int thescore, int thedepth, Tree_Node Parent_Node)
        {
            thegrid = Board;
            Current_Location = Current;
            Move_Location = Move;
            ID = theid;
            Score = thescore;
            Depth = thedepth;
            if (Depth != 0)
            {
                Parent = Parent_Node;
            }
            Child_Nodes = new Dictionary<int, Tree_Node>();
            Child_Nodes_List = new List<Tree_Node>();
        }

        //adds a new child node to the tree 
        public void Add_Tree_Node(Tree_Node Given_Node, int theid)
        {
            Tree_Node New_Node = Given_Node;
            Child_Nodes.Add(theid, New_Node);
            Child_Nodes_List.Add(New_Node);
        }

        //generates a new child node using provided information
        public void Add_Tree(Board_Grid_AI Board, Location Current, Location Move, int theid, int thescore)
        {
            Tree_Node New_Node = new Tree_Node(Board, Current, Move, theid, thescore, Depth + 1, this);
            Child_Nodes.Add(theid, New_Node);
            Child_Nodes_List.Add(New_Node);
        }

        public int Get_Depth()
        {
            return Depth;
        }

        public Board_Grid_AI Get_Grid()
        {
            return thegrid;
        }

        public int Get_Score()
        {
            return Score;
        }

        public int Get_ID()
        {
            return ID;
        }

        public Location Get_Current_Location()
        {
            return Current_Location;
        }

        public Location Get_Move_Location()
        {
            return Move_Location;
        }

        public List<Tree_Node> Get_Child_List()
        {
            return Child_Nodes_List;
        }

        Dictionary<int, Tree_Node> Get_Child_Dictionary()
        {
            return Child_Nodes;
        }

        public IEnumerator<Tree_Node> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
