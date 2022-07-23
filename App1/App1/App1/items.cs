using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    public class books
    {
        int Id;
        string Title;
        string Author;
        string Date;
        string ISBN;

        public books(int Id, string Title,string Author, string Date, string ISBN)
        {
            this.Id = Id;
            this.Title = Title;
            this.Author = Author;
            this.Date = Date;
            this.ISBN = ISBN;
        }
    }
}
