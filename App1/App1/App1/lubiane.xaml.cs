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
        //odwolanie do klasy z połączeniem z bazą
        mysql_connect con = new mysql_connect();
        //lista z klasy
        public IList<books> Books { get; set; }
        //klasa z polami do przechowywaniadanych pobieranych z bazy
        public class books
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Date { get; set; }
            public string ISBN { get; set; }
            public string UserId { get; set; }

        }
        //zmienna z id użytkownika
        int login;
        public Page3(int _logowanie)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                NavigationPage.SetHasBackButton(this, false);
            }
            //przypisywanie id użytkownika do zmiennej
            login = _logowanie;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //konvertowanie id użytkownika do stringu
            string useriden = login.ToString();
            //połączenie z bazą
            MySqlConnection connection = new MySqlConnection(con.connect());
            connection.Open();
            //pobieranie danych z bazy
            MySqlCommand command1 = new MySqlCommand("SELECT * FROM favourites WHERE userID='" + useriden + "'", connection);
            //wykonywanie zapytania
            var rd = command1.ExecuteReader();
            //tworzenie nowej listy 
            Books = new List<books>();
            //dodawanie rekordów z bazy do listy
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
            //wyświetlanie z listy na ekran
            lubie.ItemsSource = Books;

        }
        private void KsiazkiClicked(object sender,EventArgs e)
        {
            //przejście do książek w nawigacji
            Navigation.PushAsync(new Page2(login));
        }

    }
}