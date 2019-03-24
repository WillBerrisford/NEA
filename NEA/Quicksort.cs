using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    class Quicksort
    {
        LinkedList<string> list { get; set; }

        public Quicksort()
        { }

        public LinkedList<string> sort(LinkedList<string> givenlist)
        {
            list = givenlist;
            quicksort(list.First, list.Last);
            return list;
        }

        LinkedListNode<string> partition(LinkedListNode<string> left, LinkedListNode<string> right)
        {
            LinkedListNode<string> leftwall = left.Previous;
            LinkedListNode<string> rightwall = right.Next;
            LinkedListNode<string> pivot = right;
            right = right.Previous;

            while (true)
            {
                while (String.Compare(pivot.Value, left.Value) > 0 && left.Next != null && left.Next != rightwall) //pivot.Value > left.Value
                {
                    left = left.Next;
                }

                while (String.Compare(pivot.Value, right.Value) < 0 && right.Previous != null && right.Previous != leftwall) //pivot.Value < right.Value
                {
                    right = right.Previous;
                }

                for (LinkedListNode<string> node = left; node != null; node = node.Previous)
                {
                    if (node == right)
                    {
                        string tempvalue = left.Value;
                        left.Value = pivot.Value;
                        pivot.Value = tempvalue;
                        return left;
                    }
                }
                
                string temp = left.Value;
                left.Value = right.Value;
                right.Value = temp;

            }
        }

        void quicksort(LinkedListNode<string> left, LinkedListNode<string> right)
        {
            for (LinkedListNode<string> node = left; node != null; node = node.Previous)
            {
                if (right == node)
                {
                    return;
                }
                else if (right == null)
                {
                    return;
                }
            }

            LinkedListNode<string> pivot = partition(left, right);
            quicksort(pivot.Next, right);
            quicksort(left, pivot.Previous);

            return;
        }
    }
}
