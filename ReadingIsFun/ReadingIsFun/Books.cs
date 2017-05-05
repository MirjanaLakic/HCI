using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ReadingIsFun
{
    class Books
    {
        private static int numOfRecent = 10;
        public Dictionary<String, Book> BookList { get; }
        public List<String> Recent { get; }
        public Books()
        {
            BookList = new Dictionary<string, Book>();
            Recent = new List<string>();
            Read();
        }
        public void Save()
        {
            if (!Directory.Exists(".\\ProgramData"))
                Directory.CreateDirectory(".\\ProgramData");
            this.saveBooks();
            this.saveRecent();
        }
        public void Read()
        {
            if(!Directory.Exists(".\\ProgramData"))
                Directory.CreateDirectory(".\\ProgramData");
            this.readBooks();
            this.readRecent();
        }
        public Book getBook(string path)
        {
            setRecent(path);
            Book book = null;
            try { 
                book = BookList[path];
            }catch(Exception)
            {
                book = newBook(path);
            }
            return book;
        }
        private Book newBook(string path)
        {
            BookList[path] = new Book();
            return BookList[path];
        }

        private void setRecent(string path)
        {
            Recent.Remove(path);
            if (Recent.Count < numOfRecent)
            {
                Recent.Insert(0,path);
            }
            else
            {
                Recent.Insert(0, path);
                Recent.RemoveAt(numOfRecent);
            }
        }

        private void saveBooks()
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(".\\ProgramData\\books.bin", FileMode.Create));
            foreach(var pair in BookList)
            {
                bw.Write(pair.Key);
                pair.Value.Save(bw);
            }
            bw.Close();
        }
        private void saveRecent()
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(".\\ProgramData\\rbooks.bin", FileMode.Create));
            while(Recent.Count!=0)
            {
                string path = Recent.First();
                Recent.Remove(path);
                bw.Write(path);
            }
            bw.Close();
        }
        
        private void readBooks() {
            
            BinaryReader br = new BinaryReader(new FileStream(".\\ProgramData\\books.bin", FileMode.OpenOrCreate));
            while (br.PeekChar() != -1)
            {
                Book temp = new Book();
                string path = "";
                path = br.ReadString();
                temp.Read(br);
                this.BookList[path] = temp;
            }
            br.Close();
        }
        private void readRecent()
        {
            BinaryReader br = new BinaryReader(new FileStream(".\\ProgramData\\rbooks.bin", FileMode.OpenOrCreate));
            while (br.PeekChar() != -1)
            {
                string temp = "";
                temp = br.ReadString();
                Recent.Add(temp);
            }
            br.Close();
            
        }

    }
}
