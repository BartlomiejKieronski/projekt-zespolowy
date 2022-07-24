using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;
using System.Data;


namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
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

        }
        //zmienna przechowująca id użytkonika
        int login;

        public Page2(int _logowanie)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                NavigationPage.SetHasBackButton(this, false);
            }
            //przypisanie id uzytkownika do zmiennej
            login = _logowanie;
            


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //połączenie z bazą
             MySqlConnection connection = new MySqlConnection(con.connect());
             connection.Open();
            //zapytanie
             MySqlCommand command1 = new MySqlCommand("SELECT * FROM books", connection);
            //wykonanie zapytania
             var rd = command1.ExecuteReader();
            //przypisanie listy do klasy
             Books = new List<books>();
            //odczytywanie wierszy z bazu i przypisywanie ich do listy w klasie
             while (rd.Read())
             {
                 Books.Add(new books
                 {
                     Id = rd.GetInt32("Id"),
                     Title = rd.GetString("Title").ToString(),
                     Author = rd.GetString("Author").ToString(),
                     Date = rd.GetString("Date").ToString(),
                     ISBN = rd.GetInt32("ISBN").ToString()
                 }
                 );
             }
             rd.Close();
            //przypisanie listy do zbindowanych zmiennych
             Ksiazka.ItemsSource = Books;
         }
         private void DoUlubionychClicked(object sender, EventArgs e)
         {
            //odczytywanie zmiennej przypisanej do przycisku
             var button = (Button)sender;
             string ClassId = button.ClassId;
            //zmiena zmiennej na typ int
             int bookId = Convert.ToInt32(ClassId);
            //połączenie z bazą
             MySqlConnection connection = new MySqlConnection(con.connect());
             connection.Open();
            //zapytania dobazy 
             MySqlCommand query1 = new MySqlCommand("SELECT Title FROM books WHERE Id = '" + bookId + "'", connection);
             MySqlCommand query2 = new MySqlCommand("SELECT Author FROM books WHERE Id = '" + bookId + "'", connection);
             MySqlCommand query3 = new MySqlCommand("SELECT Date FROM books WHERE Id = '" + bookId + "'", connection);
             MySqlCommand query4 = new MySqlCommand("SELECT ISBN FROM books WHERE Id = '" + bookId + "'", connection);
            //konwertowanie wyników
             string Title = query1.ExecuteScalar().ToString();
             string Author = query2.ExecuteScalar().ToString();
             string Date = query3.ExecuteScalar().ToString();
             string ISBN = query4.ExecuteScalar().ToString();
            //tworzenie nowego rekordu w bazie
             string sql_insert = "INSERT INTO favourites(Title, Author, Date, ISBN, UserID) VALUES('" + Title + "','" + Author + "','" + Date + "','"+ ISBN+"','"+ login +"')";
             MySqlCommand command = new MySqlCommand(sql_insert, connection);
            //przejście do strony z ulubionymi po dodaniu ulubionej książki
             if(command.ExecuteNonQuery() == 1)
             {
                Navigation.PushAsync(new Page3(login));
            }
             connection.Close();
         
        }
        private void UlubioneClicked(object sender, EventArgs e)
        {
            //przejście do Ulubionych w navigacji
            Navigation.PushAsync(new Page3(login));
        }
    }
}