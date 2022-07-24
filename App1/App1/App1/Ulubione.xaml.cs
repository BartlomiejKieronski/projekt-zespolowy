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
    public partial class Ulubione : ContentPage
    {
        mysql_connect con = new mysql_connect();
        public IList<ulubione> Ulubiona { get; set; }
        public class ulubione
        {
            //informacje
        }
        public Ulubione()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MySqlConnection connection = new MySqlConnection(con.connect());
            connection.Open();
                    
        }
    }
}