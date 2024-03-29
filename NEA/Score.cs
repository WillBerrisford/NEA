﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace NEA
{
    public class Score : INotifyPropertyChanged
    {
        public int team_1_points { get; set; } //number of points that team 1 has
        public int team_2_points { get; set; } //number of points that team 2 has

        public event PropertyChangedEventHandler PropertyChanged; //handles UI updates

        public Score()
        {
            team_1_points = 0; //both teams start the game with 0 points
            team_2_points = 0;
        }

        public void add_point(int team) //adds one point to specified team due to a piece on the opposing team being destroyed
        {
            //determines which team should get the points
            switch (team)
            {
                case 1: //if the points need to be added to team 1 
                    team_1_points = team_1_points + 5;
                    NotifyPropertyChanged("team_1_points");
                    break;
                case 2: //if the points need to be added to team 2
                    team_2_points = team_2_points + 5;
                    NotifyPropertyChanged("team_2_points");
                    break;
            }
        }

        public void square_points(Squares squares) //add points to each team depending on number of squares controlled by each team
        {
            team_1_points = team_1_points + squares.Get_Team_One(); //
            team_2_points = team_2_points + squares.Get_Team_Two(); //adds one point to each team for each square controlled by each team
            NotifyPropertyChanged("team_1_points");
            NotifyPropertyChanged("team_2_points");
        }

        public void NotifyPropertyChanged(string propertyName) //updates UI
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
