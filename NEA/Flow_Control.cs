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
        public bool playing_game { get; set; }

        public Location The_Current_Location { get; set; } //current location of selected unit
        public Location The_Move_Location { get; set; } //location the player is tring to move the unit to

        public Squares squares { get; set; } //instance of class Squares used for counting number of squares occupied by each team

        public Score thescore { get; set; } //instance of class Score, tracks the score of each player

        public System_Scoring system_score { get; set; }

        public event PropertyChangedEventHandler PropertyChanged; //Allows properties to change so the UI is updated

        public Flow_Control()
        { }

        public Flow_Control(bool run) //initialized class with default values
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
            playing_game = true;
        }

        public Score Get_Score()
        {
            return thescore;
        }

        //creates a new instance of the AI and the AI then makes a move on the board
        public AI AI_Move(Board_Grid thegrid, System_Scoring thesystem_score, Score thescore)
        {
            AI New_AI = new AI();
            New_AI.Run_AI(thegrid, thesystem_score, thescore);
            return New_AI;
        }

        void new_click(Location the_location, Board_Grid thegrid, Flow_Control Control) //controls click logic
        {
            if (playing_game)
            {
                //Checking whether this is the first time the user has clicked (selecting a piece that will be moved)
                //or whether it is the second time (selecting a place to move to)
                if (click_event_no == 0)
                {
                    //this selects a piece that will be moved:
                    int index = thegrid.Get_Index(the_location); //gets index of the location of the piece
                    The_Current_Location = new Location(the_location.Get_x(), the_location.Get_y()); //generates current location
                    click_event_no = 1; //notifies the program that the a piece has been selected to be moved

                    if (player_turn == thegrid.Grid_List[index].Get_Team()) //checks if the unit selected is on the same team as the current player
                    { is_move = true; } //allows for the moving of the piece if it is on the same team as the player
                    else
                    { is_move = false; } //does not allow a move to take place because a player cannot move a piece belonging to another team
                }
                else
                {
                    //this selects the position on the board that a previously selected piece will be moved to 
                    if (is_move == true) //checks if the piece was selected legally (on the correct team)
                    {
                        The_Move_Location = new Location(the_location.Get_x(), the_location.Get_y()); //sets the location that the piece will be moved to
                        if (thegrid.Move(The_Current_Location, The_Move_Location, thescore) == true) //checks whether the move is legal
                                                                                                     //Will also make the move if the move is legal
                        {
                            //updates each of the grid squares for any changes that have occurred in the move
                            for (int index = 0; index < thegrid.Grid_List.Count(); index++)
                            {
                                thegrid.Grid_List[index].Update_all();
                            }

                            turn_number = turn_number + 1; //tells the program that 1 more turn has been completed

                            if (turn_number % 2 == 0) //checks if it is the end of player 2's go
                                                      //if so it means that the scores according to the ownership of squares must be added
                            {
                                squares = new Squares();
                                squares = thegrid.Count_squares(); //counts number of squares that both teams own
                                thescore.square_points(squares); //adds points based on squares controlled
                            }

                            EndGame(thescore, thegrid); //checks if one team has won
                            click_event_no = 0; //tells the program that the 2nd click to determine where to move the piece to has been completed
                            Debug.WriteLine("Click Event 1");

                            if (player_turn == 1) //switches current turn to other player
                            {
                                bool AI_On = true;

                                if (AI_On == true) //checks if the ai is enabled
                                {
                                    player_turn = 2;                            //
                                    player_turn_colour = "Red";                 // 
                                    NotifyPropertyChanged("player_turn");       //
                                    NotifyPropertyChanged("player_turn_colour");// changes to he UI to show that is is currently player 2's/the AI's go

                                    system_score.Calculate_Score(thegrid); //calculates internal score used by the AI to judge how strong different moves are
                                                                           //this generates the current one using the current board
                                    AI Move_Locations = AI_Move(thegrid, system_score, thescore); // runs AI which determines which piece to move
                                                                                                  //And where to move the piece to
                                    Location Current_Location = Move_Locations.Get_Current(); //retrieves the location of the piece that is going to be moved
                                    Location Move_Location = Move_Locations.Get_Move(); //retrieces the location of the place that the piece will be moved to

                                    if (Current_Location == null || Move_Location == null) //checks that the AI is able to make a move. 
                                    {
                                        //if not the game checks if the AI has any pieces left
                                        EndGame(thescore, thegrid);
                                    }
                                    else
                                    {
                                        //if the move is valid, then the move is made
                                        thegrid.Move(Current_Location, Move_Location, thescore); //uses the generated moves to move the pieces
                                    }

                                    turn_number = turn_number + 1; //informs the program that another turn is over
                                    click_event_no = 0; //resets click event number

                                    squares = new Squares();
                                    squares = thegrid.Count_squares(); //counts number of squares that both teams own
                                    thescore.square_points(squares); //adds points based on squares controlled
                                    EndGame(thescore, thegrid); //checks if one team has won

                                    player_turn = 1;                             //
                                    player_turn_colour = "Blue";                 //
                                    NotifyPropertyChanged("player_turn");        //Updates the UI to inform the player that it is currently the
                                    NotifyPropertyChanged("player_turn_colour"); // turn of player 1
                                }

                                else
                                {
                                    player_turn = 2;                            //
                                    player_turn_colour = "Red";                 //
                                    NotifyPropertyChanged("player_turn");       //Updates the UI to inform the player that it is currently the
                                    NotifyPropertyChanged("player_turn_colour");// turn of player 2
                                }
                            }
                            else //switches current turn to other player
                            {
                                player_turn = 1;                            //
                                player_turn_colour = "Blue";                //
                                NotifyPropertyChanged("player_turn");       //Updates the UI to inform the player that it is currently the
                                NotifyPropertyChanged("player_turn_colour");// turn of player 1
                            }
                        }
                        else { click_event_no = 0; } //tells the program that a move was invalid so it needs to return to selecting a piece to move to
                                                     // rather than continuing to try to find a positioin on the board to move to

                    }
                    else
                    {
                        click_event_no = 0;  //tells the program that a piece selected to move was invalid so it needs to return to selecting a
                                             //  piece to move, in order to select a valid piece.
                    }
                }
            }
            else { return; }
        }

        //this checks if the game has ended, either through a points victory, or one team has lost all it's pieces
        public void EndGame(Score thescore, Board_Grid thegrid)
        {
            //checks  that neither team has exceeded the points goal
            if (thescore.team_1_points >= 250) 
            {
                //if player 1 has won
                player_1_win = "Blue";                  //
                NotifyPropertyChanged("player_1_win");  //Updates the UI, notifying the player that player 1 has won
                playing_game = false; //ends the game
            }
            else if (thescore.team_2_points >= 250) 
            {
                //if player 2 has won
                player_2_win = "Red";                   //
                NotifyPropertyChanged("player_2_win");  //Updates the UI, notifying the player that player 2 has won
                playing_game = false; //ends the game
            }

            else //if neither team has enough points to win
            {
                //This section sets both teams to having won, this is changed to not having won if the other team is found to still 
                // have pieces on the board
                player_1_win = "Blue";                  //
                player_2_win = "Red";                   //
                NotifyPropertyChanged("player_1_win");  //Temporarily shows that both players have won (too quick for the player to see)
                NotifyPropertyChanged("player_2_win");  //
                playing_game = false; 

                for (int i = 0; i < 25; i++) //checks whether either team has run out of units 
                {
                    if (thegrid.Grid_List[i].Get_Occupied() == true && thegrid.Grid_List[i].Get_Team() == 1)    //checks whether any units belonging to 
                                                                                                                //team 1 are still alive
                    {
                        //If player 1 still has units then player 2 cannot win
                        //Changes the UI to reflect this
                        player_2_win = "White";
                        NotifyPropertyChanged("player_2_win");
                        playing_game = true; 
                    }

                    if (thegrid.Grid_List[i].Get_Occupied() == true && thegrid.Grid_List[i].Get_Team() == 2)    //checks whether any units belonging to 
                                                                                                                //team 1 are still alive
                    {
                        //If player 2 still has units then player 1 cannot win
                        //Changes the UI to reflect this
                        player_1_win = "White";
                        NotifyPropertyChanged("player_1_win");
                        playing_game = true; 
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

        //These activate when a certain button is clicked
        //If the button clicked as the coordinates of 0, 0 for example then Click_Event_0_0 will run and so on
        //Each function simply creates a new location according to the location of the button that caused the function to run
        //Each function then starts a new click process, passing in it's location as a parameter
        public void Click_Event_0_0(Board_Grid thegrid, Flow_Control Control) 
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
