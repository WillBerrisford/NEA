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
       // Database_Connect Database = new Database_Connect();

        public MainWindow()
        {
            this.DataContext = Data;
            InitializeComponent();          
        }

        void Load_Game_Button(object sender, RoutedEventArgs e)
        {
            Save Game_Save = new Save(Data);
            DataView Data_temp = Game_Save.Load_Game(GameNameTextBox.Text, Data.theaccount.Get_ID());
            if (Data_temp != null)
            {
                Data = Data_temp;
                this.DataContext = Data;
                Debug.WriteLine("Load Succeded");
            }
            else
            {
                Debug.WriteLine("Data Unable to Load");
            }
        }

        void Save_Game_Button(object sender, RoutedEventArgs e)
        {
            Save Game_Save = new Save(Data);
            Game_Save.Save_Game(Data.theaccount.Is_Signed_In(), Data.theaccount.Get_AccountName(), Data.theaccount.Get_ID(), GameNameTextBox.Text);
        }

        void Login_Button(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(UserNameTextBox.Text);
            Data.theaccount.SignIn(UserNameTextBox.Text, PassWordTextBox.Text);
        }

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
