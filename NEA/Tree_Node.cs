using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    class Tree_Node : IEnumerable<Tree_Node>
    {
        Dictionary<int, Tree_Node> Child_Nodes { get; set; }
        public List<Tree_Node> Child_Nodes_List { get; set; }

        int ID { get; set; }
        int Score { get; set; }
        int Depth { get; set; }

        Board_Grid_AI thegrid { get; set; }

        Location Current_Location { get; set; }
        Location Move_Location { get; set; }

        public Tree_Node Parent { get; private set; }

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

        public void Add_Tree_Node(Tree_Node Given_Node, int theid)
        {
            Tree_Node New_Node = Given_Node;
            Child_Nodes.Add(theid, New_Node);
            Child_Nodes_List.Add(New_Node);
        }

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
