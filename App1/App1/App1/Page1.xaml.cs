using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        mysql_connect con = new mysql_connect();
        public Page1()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                NavigationPage.SetHasBackButton(this, false);

            }
            
        }
        private void Zarejestruj_Clicked(object sender, EventArgs e)
        {
            /*if(Haslo.Text == PowtorzHaslo.Text)
            {
                try { 
                MySqlConnection connection = new MySqlConnection(con.connect());
                connection.Open();
                    string login = Login.Text.ToString();
                    string haslo = Haslo.Text.ToString();
                MySqlCommand command1 = new MySqlCommand("INSERT INTO uzytkownicy (Login, Haslo) VALUES('"+ login + "','"+ haslo +"')");
                    
                }
                catch
                {

                }
            }*/
            if (Haslo.Text == PowtorzHaslo.Text)
            {
                try
                {
                    MySqlConnection connection = new MySqlConnection(con.connect());
                    connection.Open();
                    string login = Login.Text.ToString();
                    string haslo = Haslo.Text.ToString();
                   string sql_insert = "INSERT INTO uzytkownicy(Login,Haslo) VALUES('" + login + "','" + haslo + "')";
                   MySqlCommand command = new MySqlCommand(sql_insert, connection);
                        try
                        {
                            if (command.ExecuteNonQuery() == 1)
                            {
                                Navigation.PushAsync(new MainPage());
                            }
                            else
                            {
                            App.Current.MainPage.DisplayAlert("Nie udało się utworzyć konta!","","ok");
                            }
                        }
                        catch (Exception ex)
                        {
                        
                        }
                    connection.Close();
                }
                catch
                {
                    App.Current.MainPage.DisplayAlert("Błąd podczas łączenia z bazą danych","","Ok");
                }
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Hasła muszą być identyczne!","","ok");
            }
        }
        private void DoLoginuClicked(object sender,EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}