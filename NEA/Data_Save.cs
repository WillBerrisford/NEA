using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    class Data_Save
    {
        public List<Unit> Grid_List { get; set; }
        public int player_turn { get; set; } //details which players turn it is
        int click_event_no { get; set; } //whether a click event is a select or a move
        bool is_move { get; set; } //has the move worked
        int turn_number { get; set; } //the number if turns that have passed 
        public string player_turn_colour { get; set; } //the colour of the players who's turn is the current one
        public string player_1_win { get; set; } //turns blue if player 1 wins
        public string player_2_win { get; set; } //turns red if player 2 wins

        Location The_Current_Location { get; set; } //current location of selected unit
        Location The_Move_Location { get; set; } //location the player is tring to move the unit to

        Squares squares { get; set; } //instance of class Squares used for counting number of squares occupied by each team

        public Score thescore { get; set; } //instance of class Score, tracks the score of each player

        System_Scoring system_score { get; set; }

    }
}
