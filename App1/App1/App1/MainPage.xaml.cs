using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MySqlConnector;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        mysql_connect con = new mysql_connect();

        public MainPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                NavigationPage.SetHasBackButton(this, false);
            }

        }
        
        private void Zaloguj_Clicked(object sender, EventArgs e)
        {

            try
            {
                MySqlConnection connection = new MySqlConnection(con.connect());
                connection.Open();
                MySqlCommand command1 = new MySqlCommand("SELECT Login FROM uzytkownicy  WHERE Login = '" + Login.Text + "'", connection);
                MySqlCommand command2 = new MySqlCommand("SELECT Haslo FROM uzytkownicy  WHERE Haslo = '" + Haslo.Text + "'", connection);
                string Logowanie = command1.ExecuteScalar().ToString();
                string Hasla = command2.ExecuteScalar().ToString();

                MySqlCommand userID = new MySqlCommand("SELECT Id FROM uzytkownicy WHERE Login = '" + Logowanie + "' AND Haslo='" + Hasla + "'", connection);
                int user = Convert.ToInt32(userID.ExecuteScalar());
                if (!(Logowanie == null) && !(Hasla == null))
                {
                    Navigation.PushAsync(new Page2(user));

                }
                else
                {
                App.Current.MainPage.DisplayAlert("Nie udało się", "", "ok");
                }
                connection.Close();
            }
            catch
            {
                App.Current.MainPage.DisplayAlert("Nie udało się błąd", "", "ok");
            }

            }
        private void DoRejerstracjiClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page1());
            
        }
        
    }
}
