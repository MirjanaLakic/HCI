using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace citanjeKnjiga
{
    class Library
    {
        public List<Book> books;
        public List<Book> recentlyRead;

        public Library()
        {
            this.Load();
        }
        public void Load()
        {
            this.books = new List<Book>();
            this.recentlyRead = new List<Book>();
            StreamReader sr = new StreamReader("../../books.txt");
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
            sr.Close();
        }

        public void importBook(string path)
        {
            string[] parts = path.Split('\\');
            int index = parts.Length;
            string name = parts[index - 1];
            string targetPath = "..\\..\\books";
            string sourcePath = "";
            foreach (string part in parts)
            {
                if (!part.Equals(name))
                {
                    sourcePath += part;
                    sourcePath += "\\";
                }
            }
            string sourceFile = Path.Combine(sourcePath, name);
            string targetFile = Path.Combine(targetPath, name);
            File.Copy(sourceFile, targetFile, true);

            Book book = new Book(name, false, false, true);
            this.books.Add(book);
        }

        public void saveBooks()
        {
            List<string> lines = new List<string>();
            
            foreach (Book book in this.books)
            {
                int fav = 0, re = 0, ra = 0;
                if (book.Favorite)
                    fav = 1;
                if (book.Recently)
                    re = 1;
                if (book.RecentlyAdded)
                    ra = 1;
                string line = book.Name + ',' + fav + ',' + re + ',' + ra;
                lines.Add(line);
            }
            StreamWriter file = new StreamWriter("../../books.txt");
            foreach (string line in lines)
            {
                file.WriteLine(line);
            }
            file.Close();
        }
    }
}
