using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;
using System.Data.SqlClient;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        //odwołanie do klasy z stringien do połączenie z bazą
        SqlConnect con = new SqlConnect();
        
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
                string password = Haslo.Text.ToString();
                string login = Login.Text.ToString();
                try
                {
                    using (SqlConnection connection = con.Connection())
                    {
                        string query = string.Format(@"IF NOT EXISTS (SELECT Login FROM [dbo].users WHERE Login='{0}')
                                            BEGIN
                                                INSERT INTO [dbo].users (Login, Haslo, Admin) VALUES ('{1}', HASHBYTES('SHA2_256', CONVERT(NVARCHAR(255),'{2}')), 0);
                                            END;", login, login, password);

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            int result = command.ExecuteNonQuery();

                            // Check Error
                            if (result < 0)
                                App.Current.MainPage.DisplayAlert("Konto zostało pomyślnie utworzone!", "", "ok");
                            else
                                App.Current.MainPage.DisplayAlert("Nie udało się utworzyć konta!", "", "ok");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //wyświetlenie alertu o błędzie
                    App.Current.MainPage.DisplayAlert("Błąd podczas łączenia z bazą danych", ex.ToString(), "Ok");
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