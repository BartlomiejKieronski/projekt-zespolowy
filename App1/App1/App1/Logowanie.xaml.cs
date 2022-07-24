using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MySqlConnector;
using System.Data.SqlClient;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        //odwołanie do klasy  do połączenia z bazą
        SqlConnect con = new SqlConnect();

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
                int user = 0;
                //połączenie z bazą
                using (SqlConnection connection = con.Connection())
                {
                    connection.Open();

                    string sql = string.Format(@"SELECT [dbo].[users].[UserId]
                                FROM[dbo].[users]
                                WHERE[dbo].[users].Login = '{0}'
                                AND [dbo].[users].Haslo = HASHBYTES('SHA2_256', CONVERT(NVARCHAR(255), '{1}'))",
                                Login.Text, Haslo.Text);

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                user = reader.GetInt32(0);
                            }

                        }
                    }
                }
                if (user != 0)
                {
                    Navigation.PushAsync(new Page2(user));
                }
                else
                {
                    //alert o nieudanym zalogowaniu
                    App.Current.MainPage.DisplayAlert("Nie udało się", "", "ok");
                }
            }
            catch (SqlException ex)
            {
                //wyświetlenie błędu
                App.Current.MainPage.DisplayAlert("Błąd", ex.ToString(), "ok");
            }

            }
        private void DoRejerstracjiClicked(object sender, EventArgs e)
        {
            //przejście do rejerstracji
            Navigation.PushAsync(new Page1());
            
        }
        
    }
}
