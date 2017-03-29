using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace citanjeKnjiga
{
    class Library
    {
        List<Book> books;
        List<Book> recentlyRead;

        public Library()
        {
            this.Load();
        }
        public void Load()
        {
            this.books = new List<Book>();
            this.recentlyRead = new List<Book>();
            StreamReader sr = new StreamReader("books.txt");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                bool fav = false;
                bool re = false;
                bool ra = false;
                string[] parts = line.Split(',');
                string name = parts[0];
                if (parts[1].Equals("1"))
                {
                    fav = true;
                }
                if (parts[2].Equals("1"))
                {
                    re = true;
                }
                if (parts[3].Equals("1"))
                {
                    ra = true;
                }
                Book book = new Book(name,fav,re,ra);
                this.books.Add(book);
                if (book.Recently)
                {
                    this.recentlyRead.Add(book);
                }
            }
        }
    }
}
