using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace NEA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataView Data = new DataView(); //starts new DataView class
        Account_Data Backup = new Account_Data();

        public MainWindow()
        {
            this.DataContext = Data; //initialises the window using Data as the datacontext
            InitializeComponent();
            Data.theaccount.StringListGameName = new LinkedList<string>();
            Data.theaccount.StringListGameName.AddLast("bbb");
            Data.theaccount.StringListGameName.AddLast("bba");
            Data.theaccount.StringListGameName.AddLast("bab");
            Data.theaccount.StringListGameName.AddLast("abb");
            Data.theaccount.StringListGameName.AddLast("baa");
            Data.theaccount.StringListGameName.AddLast("aab");
            Data.theaccount.StringListGameName.AddLast("aaa");
            Data.theaccount.StringListGameName.AddLast("xcb");
            Data.theaccount.StringListGameName.AddLast("iau");
            Data.theaccount.StringListGameName.AddLast("f");
            Data.theaccount.StringListGameName.AddLast("u");
            Data.theaccount.StringListGameName.AddLast("p");
            Data.theaccount.sort_list();
            Debug.Write("");
        }

        //warning the user if the username they entered does not exist
        void UserNameCheckFail(object sender, EventArgs e)
        {
            MessageBox.Show("Username does not exist", "Check the username you entered is correct");
        }

        //Displays the list of saved games to the user
        void Display_Game_Name_Grid()
        {
            GameNameList.ItemsSource = null; //clears item source of old data
            GameNameList.ItemsSource = Data.theaccount.Return_game_list(); //sets the item source to the list of saved games
        }

        //loads the game with the name specified by the player
        void Load_Game_Button(object sender, RoutedEventArgs e)
        {
            Save Game_Save = new Save(Data);//creates a new instance of the save class
            DataView Data_temp = Game_Save.Load_Game(GameNameTextBox.Text, Data.theaccount.Get_ID());//loads a saved game from the server
            if (Data_temp != null) //checks that the loaded game is valid and not null
            {
                Data = Data_temp; //sets the Data object to be equal to the data object retrieved from the server
                this.DataContext = Data; //sets the data context to the new data object
                Data.theaccount.SignIn(Backup.Get_Name(), Backup.Get_Password()); //signs back into the users account
                Debug.WriteLine("Load Succeded");
            }
            else
            {
                MessageBox.Show("Game data was unable to load", "Either is not connected to an account or the name is incorrect");
            }
        }

        //this saves the current dataview object to the server
        void Save_Game_Button(object sender, RoutedEventArgs e)
        {
            Save Game_Save = new Save(Data); //copies the current save into a new save
            //Saves the game to the server
            Game_Save.Save_Game(Data.theaccount.Is_Signed_In(), Data.theaccount.Get_AccountName(), Data.theaccount.Get_ID(), GameNameTextBox.Text);
            Data.theaccount.SignIn(Backup.Get_Name(), Backup.Get_Password()); //signs back into the user account
            Display_Game_Name_Grid(); //Displays the list of saved games to the user
        }

        //logs the user into an existing accoutn
        void Login_Button(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(UserNameTextBox.Text);
            Data.theaccount.SignIn(UserNameTextBox.Text, PassWordTextBox.Text); //signs into the account
            //creates a backup of the account details
            Backup = new Account_Data(Data.theaccount.Get_AccountName(), Data.theaccount.Get_ID(), Data.theaccount.Is_Signed_In(), Data.theaccount.Get_Password());
            Display_Game_Name_Grid(); //Displays the list of saved games to the user
        }

        //adds a new user account
        void Sign_Up_Button(object sender, RoutedEventArgs e)
        {
            Data.theaccount.add_user(UserNameTextBox.Text, PassWordTextBox.Text);
        }

        void Click_0_0(object sender, RoutedEventArgs e) //activated when button 0_0 is clicked
        {
            Data.Control.Click_Event_0_0(Data.thegrid, Data.Control); //activates corresponding events in flow control         
        }

        void Click_0_1(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_0_1(Data.thegrid, Data.Control);
        }

        void Click_0_2(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_0_2(Data.thegrid, Data.Control);
        }

        void Click_0_3(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_0_3(Data.thegrid, Data.Control);
        }

        void Click_0_4(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_0_4(Data.thegrid, Data.Control);
        }

        void Click_1_0(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_1_0(Data.thegrid, Data.Control);
        }

        void Click_1_1(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_1_1(Data.thegrid, Data.Control);
        }

        void Click_1_2(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_1_2(Data.thegrid, Data.Control);
        }

        void Click_1_3(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_1_3(Data.thegrid, Data.Control);
        }

        void Click_1_4(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_1_4(Data.thegrid, Data.Control);
        }

        void Click_2_0(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_2_0(Data.thegrid, Data.Control);
        }

        void Click_2_1(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_2_1(Data.thegrid, Data.Control);
        }

        void Click_2_2(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_2_2(Data.thegrid, Data.Control);
        }

        void Click_2_3(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_2_3(Data.thegrid, Data.Control);
        }

        void Click_2_4(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_2_4(Data.thegrid, Data.Control);
        }

        void Click_3_0(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_3_0(Data.thegrid, Data.Control);
        }

        void Click_3_1(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_3_1(Data.thegrid, Data.Control);
        }

        void Click_3_2(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_3_2(Data.thegrid, Data.Control);
        }

        void Click_3_3(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_3_3(Data.thegrid, Data.Control);
        }

        void Click_3_4(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_3_4(Data.thegrid, Data.Control);
        }

        void Click_4_0(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_4_0(Data.thegrid, Data.Control);
        }

        void Click_4_1(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_4_1(Data.thegrid, Data.Control);
        }

        void Click_4_2(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_4_2(Data.thegrid, Data.Control);
        }

        void Click_4_3(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_4_3(Data.thegrid, Data.Control);
        }

        void Click_4_4(object sender, RoutedEventArgs e)
        {
            Data.Control.Click_Event_4_4(Data.thegrid, Data.Control);
        }
       
    }
}
