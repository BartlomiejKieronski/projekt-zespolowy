using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;
using System.Data;
using System.Data.SqlClient;
using Xamarin.Essentials;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        //odwolanie do klasy z połączeniem z bazą
        SqlConnect con = new SqlConnect();
        //lista z klasy
        public IList<books> Books { get; set; }
        DataTable dt = new DataTable();
        //klasa z polami do przechowywaniadanych pobieranych z bazy
        public class books
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string ISBN { get; set; }
            public string Tags { get; set; }

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
            try
            {
                using (SqlConnection connection = con.Connection())
                {
                    connection.Open();

                    string sql = string.Format(@"SELECT [dbo].books.BookId
                                                        ,[dbo].authors.Author
                                                        ,[dbo].books.Title
                                                        ,[dbo].books.ISBN
                                                        ,STRING_AGG([dbo].tags.Tag, ', ') AS Tags
                                                        FROM [dbo].books
                                                        JOIN [dbo].tagbook
                                                          ON [dbo].tagbook.BookId = [dbo].books.BookId
                                                        JOIN [dbo].authorbook
                                                          ON [dbo].books.BookId = [dbo].authorbook.BookId
                                                        JOIN [dbo].authors
                                                          ON [dbo].authorbook.AuthorId = dbo.authors.AuthorId
                                                        JOIN [dbo].tags
                                                          ON [dbo].tags.TagId = [dbo].tagbook.TagId
                                                        GROUP BY [dbo].books.Title,[dbo].authors.Author,[dbo].books.ISBN, [dbo].books.BookId;");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
                
            }
            catch (SqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Błąd podczas łączenia z bazą danych" + ex.ToString(), "", "ok");
            }
            Books = new List<books>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                books book = new books
                {
                    Id = Convert.ToInt32(dt.Rows[i]["BookId"]),
                    Title = dt.Rows[i]["Title"].ToString(),
                    Author = dt.Rows[i]["Author"].ToString(),
                    ISBN = dt.Rows[i]["ISBN"].ToString(),
                    Tags = dt.Rows[i]["Tags"].ToString()
                };
                Books.Add(book);
            }
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
            try
            {
                using (SqlConnection connection = con.Connection())
                {
                    string query = string.Format(@"IF NOT EXISTS (SELECT BookId, UserId FROM [dbo].favourites WHERE BookId={0} AND UserId = {1})
                                            BEGIN
                                                INSERT INTO [dbo].favourites (BookId, UserId) VALUES ({2},{3});
                                            END;", bookId, login, bookId, login);

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        // Check Error
                        if (result < 0)
                            App.Current.MainPage.DisplayAlert("Książka już w ulubionych!", "", "ok");
                        else
                            App.Current.MainPage.DisplayAlert("Dodano do ulubionych.", "", "ok");
                    }
                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Błąd", ex.ToString(), "ok");
            }

        }
        private void CzytajClicked(object sender, EventArgs e)
        {
            //odczytywanie zmiennej przypisanej do przycisku
            var button = (Button)sender;
            string ClassId = button.ClassId;
            //zmiena zmiennej na typ int
            int bookId = Convert.ToInt32(ClassId);
            //URL książki
            string URL = "";
            //połączenie z bazą
            try
            {
                using (SqlConnection connection = con.Connection())
                {
                    connection.Open();

                    string sql = string.Format("SELECT [dbo].books.URL FROM [dbo].books WHERE [dbo].books.BookId = {0}", bookId);

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                URL = reader.GetString(0);
                            }
                        }
                    }
                }
                Launcher.OpenAsync(new Uri(URL));
            }
            catch (SqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Błąd", ex.ToString(), "ok");
            }
        }
        private void UlubioneClicked(object sender, EventArgs e)
        {
            //przejście do Ulubionych w navigacji
            Navigation.PushAsync(new Page3(login));
        }
    }
}