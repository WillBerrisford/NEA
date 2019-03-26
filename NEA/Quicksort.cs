using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    //this class enables the sorting of any given list of strings by the string values of each item in the list
    class Quicksort
    {
        //this list holds the list that will be sorted
        LinkedList<string> list { get; set; }

        //this is the constructor of the class
        public Quicksort()
        { }

        //this starts the quicksort running.
        public LinkedList<string> sort(LinkedList<string> givenlist)
        {
            list = givenlist;
            quicksort(list.First, list.Last); //starts the quicksort giving to 1st value in the list as the left value and the last value in the list as the right value
            return list; 
        }

        //this creates a partition in the list, seperating all the values to come before or after the pivot value.
        LinkedListNode<string> partition(LinkedListNode<string> left, LinkedListNode<string> right)
        {
            LinkedListNode<string> leftwall = left.Previous; //this is the first value outside of the section of the list that is being sorted to the left of the section
                                                            //it is a node in the the list 
            LinkedListNode<string> rightwall = right.Next; //this is the first value outside of the section of the list that is being sorted to the right of the section
            LinkedListNode<string> pivot = right; //this is the value that all the other values are either placed before or after (according to the sort order)
            right = right.Previous; //this is the rightmost value that is being sorted

            while (true)
            {
                //String.Compare(pivot.Value, left.Value) > 0 means that the while loop will run while the pivot value is later on in the sort order than the left value
                //left.Next != null checks that the left node hasn't gone past the end of the list
                //left.Next != rightwall checks that the left node hasn't gone past the end of the current section of the list that is being sorted currently
                while (String.Compare(pivot.Value, left.Value) > 0 && left.Next != null && left.Next != rightwall) //pivot.Value > left.Value
                {
                    //while the pivot value is later (in the sort order) than the left value, left will move on to the next node in the list
                    left = left.Next;
                }

                //String.Compare(pivot.Value, right.Value) < 0 means that the while loop will continue while the pivot value is earlier in the sort order than the right value
                //right.Previous != null checks that the right node has not gone past the end of the list
                //right.Previous != leftwall checks that the right node has not gone past the end of the section of the list that is currently being sorted.
                while (String.Compare(pivot.Value, right.Value) < 0 && right.Previous != null && right.Previous != leftwall) //pivot.Value < right.Value
                {
                    //while the pivot value is earlier (in the sort order) than the right value, right will move to the previous node in the list
                    right = right.Previous;
                }

                //this checks wether the left and right nodes are in the same place in the list, or whether the right node is to the left of the lef node in the list
                for (LinkedListNode<string> node = left; node != null; node = node.Previous)
                {
                    if (node == right) //checks if the current node == the right node
                    {
                        //if the left node is in the same position or later than the right node, the following code will run
                        //this code swaps the left value and the pivot value, before returning the left value
                        string tempvalue = left.Value;
                        left.Value = pivot.Value;
                        pivot.Value = tempvalue;
                        return left;
                    }
                }
                
                //if the right node is further to the right (in its position in the list) than the left node, then the left and right values will be swapped
                //swaps the values in the left and right nodes.
                string temp = left.Value;
                left.Value = right.Value;
                right.Value = temp;

            }
        }

        //this carries out the quicksort on the list.
        void quicksort(LinkedListNode<string> left, LinkedListNode<string> right)
        {
            //this checks whether the the left value is in the same position or later on than the right value in the list
            for (LinkedListNode<string> node = left; node != null; node = node.Previous)
            {
                if (right == node)
                {
                    //if the left value is in the same postion or later than the right hand value
                    return;
                }
                else if (right == null)
                {
                    //if the right value is null, the left value must be later on, so the code will return 
                    return;
                }
            }

            //if the left value is to the left of the right value, the following code will run
            LinkedListNode<string> pivot = partition(left, right); //this creates a new partition using the given left and right values
            quicksort(pivot.Next, right); //this quicksorts the left side the of the new partition
            quicksort(left, pivot.Previous); //this quicksorts the right hand side of the new partion

            return;
        }
    }
}
