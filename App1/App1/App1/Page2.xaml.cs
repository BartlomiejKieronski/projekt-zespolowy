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