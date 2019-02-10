using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;

namespace NEA
{
    public class Flow_Control : INotifyPropertyChanged
    {
        public int player_turn { get; set; } //details which players turn it is
        public int click_event_no { get; set; } //whether a click event is a select or a move
        public bool is_move { get; set; } //has the move worked
        public int turn_number { get; set; } //the number if turns that have passed 
        public string player_turn_colour { get; set; } //the colour of the players who's turn is the current one
        public string player_1_win { get; set; } //turns blue if player 1 wins
        public string player_2_win { get; set; } //turns red if player 2 wins

        public Location The_Current_Location { get; set; } //current location of selected unit
        public Location The_Move_Location { get; set; } //location the player is tring to move the unit to

        public Squares squares { get; set; } //instance of class Squares used for counting number of squares occupied by each team

        public Score thescore { get; set; } //instance of class Score, tracks the score of each player

        public System_Scoring system_score { get; set; }

        public event PropertyChangedEventHandler PropertyChanged; //Allows properties to change so the UI is updated

        public Flow_Control()
        { }

        public Flow_Control(bool run) //initialized class
        {
            player_turn_colour = "Blue";
            click_event_no = 0;
            player_turn = 1;
            is_move = false;
            thescore = new Score();
            system_score = new System_Scoring();
            turn_number = 0;
            player_1_win = "White";
            player_2_win = "White";
        }

        public Score Get_Score()
        {
            return thescore;
        }

        public AI AI_Move(Board_Grid thegrid, System_Scoring thesystem_score, Score thescore)
        {
            AI New_AI = new AI();
            New_AI.Run_AI(thegrid, thesystem_score, thescore);
            return New_AI;
        }

        void new_click(Location the_location, Board_Grid thegrid, Flow_Control Control) //controls click logic
        {

            if (click_event_no == 0) //if the click is the first time the user has clicked
            {
                int index = thegrid.Get_Index(the_location); //gets index of the location
                The_Current_Location = new Location(the_location.Get_x(), the_location.Get_y()); //generates current location
                click_event_no = 1;

                if (player_turn == thegrid.Grid_List[index].Get_Team()) //checks if the unit selected on is in the team thats turn i
                { is_move = true; }
                else
                { is_move = false; }
            }
            else
            {
                if (is_move == true)
                {
                    The_Move_Location = new Location(the_location.Get_x(), the_location.Get_y()); //sets the location to move to
                    if (thegrid.Move(The_Current_Location, The_Move_Location, thescore) == true) //checks whether the move is legal + carries out move
                    {
                        for (int index = 0; index < thegrid.Grid_List.Count(); index++)
                        {
                            thegrid.Grid_List[index].Update_all();
                        }

                        turn_number = turn_number + 1;

                        if (turn_number % 2 == 0) //checks if turn is even
                        {
                            squares = new Squares();
                            squares = thegrid.Count_squares(); //counts number of squares that both teams own
                            thescore.square_points(squares); //adds points based on squares controlled
                        }

                        EndGame(thescore, thegrid); //checks if one team has won
                        click_event_no = 0; //resets click event number
                        Debug.WriteLine("Click Event 1");

                        if (player_turn == 1) //switches current turn to other player
                        {
                            bool AI_On = true;

                            if (AI_On == true)
                            {
                                player_turn = 2;
                                player_turn_colour = "Red";
                                NotifyPropertyChanged("player_turn");
                                NotifyPropertyChanged("player_turn_colour");

                                system_score.Calculate_Score(thegrid); //calculates the system score based on the grid
                                AI Move_Locations = AI_Move(thegrid, system_score, thescore); // run AI
                                Location Current_Location = Move_Locations.Get_Current();
                                Location Move_Location = Move_Locations.Get_Move();

                                if(Current_Location == null || Move_Location == null)
                                {
                                    EndGame(thescore, thegrid);
                                }
                                else
                                {
                                    thegrid.Move(Current_Location, Move_Location, thescore); //uses the generated moves to move the pieces
                                }
                               
                                turn_number = turn_number + 1; //increases turn number
                                click_event_no = 0; //resets click event number

                                squares = new Squares();
                                squares = thegrid.Count_squares(); //counts number of squares that both teams own
                                thescore.square_points(squares); //adds points based on squares controlled
                                EndGame(thescore, thegrid); //checks if one team has won

                                player_turn = 1;
                                player_turn_colour = "Blue";
                                NotifyPropertyChanged("player_turn");
                                NotifyPropertyChanged("player_turn_colour");
                            }

                            else
                            {
                                player_turn = 2;
                                player_turn_colour = "Red";
                                NotifyPropertyChanged("player_turn");
                                NotifyPropertyChanged("player_turn_colour");
                            }
                        }
                        else //switches current turn to other player
                        {
                            player_turn = 1;
                            player_turn_colour = "Blue";
                            NotifyPropertyChanged("player_turn");
                            NotifyPropertyChanged("player_turn_colour");
                        }
                    }
                    else { click_event_no = 0; } //resets click event if moves fail

                }
                else
                {
                    click_event_no = 0; //resets click event if moves fail
                }
            }
        }

        public void EndGame(Score thescore, Board_Grid thegrid)
        {
            if (thescore.team_1_points >= 250) //checks point goals have not been hit
            {
                player_1_win = "Blue";
                NotifyPropertyChanged("player_1_win");
            }
            else if (thescore.team_2_points >= 250) //checks point goals have not been hit
            {
                player_2_win = "Red";
                NotifyPropertyChanged("player_2_win");
            }

            else
            {
                player_1_win = "Blue";
                player_2_win = "Red";
                NotifyPropertyChanged("player_1_win");
                NotifyPropertyChanged("player_2_win");

                for (int i = 0; i < 25; i++) //checks whether either team has run out of units 
                {
                    if (thegrid.Grid_List[i].Get_Occupied() == true && thegrid.Grid_List[i].Get_Team() == 1)
                    {
                        player_2_win = "White";
                        NotifyPropertyChanged("player_2_win");
                    }

                    if (thegrid.Grid_List[i].Get_Occupied() == true && thegrid.Grid_List[i].Get_Team() == 2)
                    {
                        player_1_win = "White";
                        NotifyPropertyChanged("player_1_win");
                    }
                }
            }
        }

        public void NotifyPropertyChanged(string propertyName) //updates ui
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Login_Click(string name, string password)
        {
            
        }

        public void Click_Event_0_0(Board_Grid thegrid, Flow_Control Control) //activates when button 0_0 is clicked
        {                       
            Location loc = new Location(0, 0);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_0_1(Board_Grid thegrid, Flow_Control Control)
        {                      
            Location loc = new Location(0, 1);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_0_2(Board_Grid thegrid, Flow_Control Control)
        {          
            Location loc = new Location(0, 2);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_0_3(Board_Grid thegrid, Flow_Control Control)
        {             
            Location loc = new Location(0, 3);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_0_4(Board_Grid thegrid, Flow_Control Control)
        {
            Location loc = new Location(0, 4);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_1_0(Board_Grid thegrid, Flow_Control Control)
        {         
            Location loc = new Location(1, 0);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_1_1(Board_Grid thegrid, Flow_Control Control)
        {          
            Location loc = new Location(1, 1);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_1_2(Board_Grid thegrid, Flow_Control Control)
        {          
            Location loc = new Location(1, 2);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_1_3(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(1, 3);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_1_4(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(1, 4);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_2_0(Board_Grid thegrid, Flow_Control Control)
        {          
            Location loc = new Location(2, 0);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_2_1(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(2, 1);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_2_2(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(2, 2);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_2_3(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(2, 3);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_2_4(Board_Grid thegrid, Flow_Control Control)
        {      
            Location loc = new Location(2, 4);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_3_0(Board_Grid thegrid, Flow_Control Control)
        {          
            Location loc = new Location(3, 0);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_3_1(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(3, 1);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_3_2(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(3, 2);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_3_3(Board_Grid thegrid, Flow_Control Control)
        {            
            Location loc = new Location(3, 3);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_3_4(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(3, 4);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_4_0(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(4, 0);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_4_1(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(4, 1);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_4_2(Board_Grid thegrid, Flow_Control Control)
        {            
            Location loc = new Location(4, 2);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_4_3(Board_Grid thegrid, Flow_Control Control)
        {           
            Location loc = new Location(4, 3);
            new_click(loc, thegrid, Control);
        }

        public void Click_Event_4_4(Board_Grid thegrid, Flow_Control Control)
        {          
            Location loc = new Location(4, 4);
            new_click(loc, thegrid, Control);
        }
    }
}
