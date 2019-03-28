using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;

namespace NEA
{
    [System.Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(Location))] //tells the serializer that this object can be serialized
    public class Unit : INotifyPropertyChanged
    {
        public Location Unit_location { get; set; } //location of given unit
        public int Unit_Strength { get; set; } //strength of unti
        public bool Occupied { get; set; } //whether the unit is occupied by a piece
        public int Team { get; set; } //the team the unit belongs to

        public string Team_colour { get; set; } //colour of the team it belongs to
        public string Piece_colour { get; set; } //colour of the piece (depends on team)
        public string Str_Strength { get; set; } //same as Unit_Strength but converted into a string

        public event PropertyChangedEventHandler PropertyChanged;

        public Unit()
        { }

        //initialises the unit using given data
        public Unit(Location Given_location, int Given_strength, bool In_play, int Given_team, string Given_Team_colour, string Given_Piece_colour)
        {
            Unit_location = Given_location; //sets unit location
            Unit_Strength = Given_strength; //sets unit strength

            if (Given_strength == 0) //checks if unit has been destroyed
            {
                Str_Strength = null;
            }
            else
            {
                Str_Strength = Unit_Strength.ToString(); 
            }

            Occupied = In_play;
            Team = Given_team;
            Team_colour = Given_Team_colour;
            Piece_colour = Given_Piece_colour;
        }

        //updates the strength based on the strength given as well as checking if the unit has been destroyed
        public void Update_Strength(int Strength)
        {
            if (Strength == 0) //checks if unit has no piece or if piece has been killed
            {
                Str_Strength = null;
            }

            else
            {
                Str_Strength = Strength.ToString();
            }
        }

        public Location Get_Location()
        {
            return Unit_location;
        }

        public int Get_Strength()
        {
            return Unit_Strength;
        }

        public int Get_Team()
        {
            return Team;
        }

        public bool Get_Occupied()
        {
            return Occupied;
        }

        public string Get_Piece()
        {
            return Piece_colour;
        }

        public string Get_Colour()
        {
            return Team_colour;
        }

        public void Empty()
        {
            Occupied = false;
        }

        public void Check_alive() //checks whether the piece in the unit is alive
        {
            if (Unit_Strength <= 0)
            {
                Occupied = false;
                Piece_colour = "transparent";
                Update_Strength(0);
                Update_all();
            }
        }

        public void Killed() //removes the piece contained by the unit from the game
        {
            Occupied = false;
            Piece_colour = "transparent";
            Update_Strength(Unit_Strength);
            Update_all();
        }

        public void Defense_Success() //occurs if the defending piece has higher strength than the attacking piece
        {
            Unit_Strength = Unit_Strength - 1;
            Update_Strength(Unit_Strength);
            NotifyPropertyChanged("Str_Strength");
            Check_alive();
        }

        public void Defense_Draw()//occurs if attacking strength is the same as defending
        {
            Unit_Strength = Unit_Strength - 1;
            Update_Strength(Unit_Strength);
            NotifyPropertyChanged("Str_Strength");
            Check_alive();
        }

        public void Attack_Loss()//occurs if the unit has attacked a higher strength piece
        {
            Unit_Strength = Unit_Strength - 2;
            Update_Strength(Unit_Strength);
            NotifyPropertyChanged("Str_Strength");
            Check_alive();
        }

        public void Attack_Draw()//occurs if the unit has attacked a piece with the same strength as itself
        {
            Unit_Strength = Unit_Strength - 1;
            Update_Strength(Unit_Strength);
            NotifyPropertyChanged("Str_Strength");
            Check_alive();
        }

        public void Update_all() //updates UI elements
        {
            NotifyPropertyChanged("Unit_location");
            NotifyPropertyChanged("Unit_Strength");
            NotifyPropertyChanged("Occupied");
            NotifyPropertyChanged("Team");

            NotifyPropertyChanged("Team_colour");
            NotifyPropertyChanged("Piece_colour");
            NotifyPropertyChanged("Str_Strength");
        }

        public void Move_Update(Location Given_location, int Given_strength, bool In_play, int Given_team, string Given_Team_colour, string Given_Piece_colour)
        {   //Updates unit when a piece is moved into its location
            //works by copying the data from another unit into this unit
            Unit_location = Given_location;
            Unit_Strength = Given_strength;

            if (Given_strength == 0)
            {
                Str_Strength = null;
            }
            else
            {
                Str_Strength = Unit_Strength.ToString();
            }

            Occupied = In_play;
            Team = Given_team;
            Team_colour = Given_Team_colour;
            Piece_colour = Given_Piece_colour;

            Update_all(); //updates the UI of this square on the board
        }

        public void NotifyPropertyChanged(string propertyName) //updates UI for a specific property
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
