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
        //odwołanie do klasy z stringien do połączenie z bazą
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
            //sprawdzenie czy pola są takie same
            if (Haslo.Text == PowtorzHaslo.Text)
            {
                try
                {
                    //połączenie z bazą
                    MySqlConnection connection = new MySqlConnection(con.connect());
                    connection.Open();
                    string login = Login.Text.ToString();
                    string haslo = Haslo.Text.ToString();
                    //zapytanie do wprowadzenia danych do bazy
                   string sql_insert = "INSERT INTO uzytkownicy(Login,Haslo) VALUES('" + login + "','" + haslo + "')";
                   //wykonanie zapytania
                    MySqlCommand command = new MySqlCommand(sql_insert, connection);
                        try
                        {
                            if (command.ExecuteNonQuery() == 1)
                            {
                            //jeśli się udało przejście do logowania
                                Navigation.PushAsync(new MainPage());
                            }
                            else
                            {
                            //jeśli sie nie udało wyświetlenie alertu
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
                    //wyświetlenie alertu o błędzie
                    App.Current.MainPage.DisplayAlert("Błąd podczas łączenia z bazą danych","","Ok");
                }
            }
            else
            {
                //wyświetlenie alertu o różnych hasłąch
                App.Current.MainPage.DisplayAlert("Hasła muszą być identyczne!","","ok");
            }
        }
        private void DoLoginuClicked(object sender,EventArgs e)
        {
            //przejście do strony logowania
            Navigation.PushAsync(new MainPage());
        }
    }
}