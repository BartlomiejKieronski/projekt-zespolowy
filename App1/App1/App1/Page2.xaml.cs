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
        mysql_connect con = new mysql_connect();
        public IList<books> Books { get; set; }
        public class books
        {
            public int Id {get;set;}
            public string Title { get; set; }
            public string Author { get; set; }
            public string Date { get; set; }
            public string ISBN { get; set; }

        }
        string login;
        public Page2(string _logowanie)
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
            MySqlConnection connection = new MySqlConnection(con.connect());
            connection.Open();
            MySqlCommand command1 = new MySqlCommand("SELECT * FROM books", connection);

            var rd = command1.ExecuteReader();
            Books = new List<books>();
            while (rd.Read())
            {
                Books.Add( new books
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
            Ksiazka.ItemsSource = Books;
        }
        private void DoUlubionychClicked(object sender,EventArgs e)
        {
            var button = (Button)sender;
            var ClassId = button.ClassId;
            if (ClassId == null)
            {
                button.IsVisible = false;
            }
        }
    }
}