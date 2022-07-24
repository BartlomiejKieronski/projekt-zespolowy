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
    public partial class Page3 : ContentPage
    {
        mysql_connect con = new mysql_connect();
        public IList<books> Books { get; set; }
        public class books
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Date { get; set; }
            public string ISBN { get; set; }
            public string UserId { get; set; }

        }
        int login;
        public Page3(int _logowanie)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                NavigationPage.SetHasBackButton(this, false);
            }
            login = _logowanie;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            string useriden = login.ToString();
            MySqlConnection connection = new MySqlConnection(con.connect());
            connection.Open();
            MySqlCommand command1 = new MySqlCommand("SELECT * FROM favourites WHERE userID='" + useriden + "'", connection);
            var rd = command1.ExecuteReader();
            Books = new List<books>();
            while (rd.Read())
            {
                Books.Add(new books
                {
                    Id = rd.GetInt32("Id"),
                    Title = rd.GetString("Title").ToString(),
                    Author = rd.GetString("Author").ToString(),
                    Date = rd.GetString("Date").ToString(),
                    ISBN = rd.GetString("ISBN").ToString(),
                    UserId = rd.GetInt32("userID").ToString()
                }
                );
            }
            rd.Close();
            lubie.ItemsSource = Books;

        }
        private void KsiazkiClicked(object sender,EventArgs e)
        {
            Navigation.PushAsync(new Page2(login));
        }

    }
}