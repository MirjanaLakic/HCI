﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Win32;


namespace citanjeKnjiga
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Library Lib { get; set; }
        public MainWindow()
        {
            this.Lib = new Library();
            InitializeComponent();
            this.Lib.Load();
            ObservableCollection<Book> listRecentRead = new ObservableCollection<Book>();
            ObservableCollection<Book> listRecentImported = new ObservableCollection<Book>();
            ObservableCollection<Book> listFavorites = new ObservableCollection<Book>();
            ObservableCollection<Book> listBooks = new ObservableCollection<Book>();
            foreach(Book b in this.Lib.books){
                listBooks.Add(b);
                if(b.RecentlyAdded)
                    listRecentImported.Add(b);
                if (b.Recently)
                    listRecentRead.Add(b);
                if(b.Favorite)
                    listFavorites.Add(b);
            }
            this.DG1.ItemsSource = listRecentRead;
            this.DG2.ItemsSource = listRecentImported;
            this.DG3.ItemsSource = listFavorites;
            this.DG4.ItemsSource = listBooks;

            
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                openBook(openFileDialog.FileName);
            }
        }

        private void btnImportFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                this.Lib.importBook(openFileDialog.FileName);
            }
        }

        private void btnImpAndOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                openBook(openFileDialog.FileName);
                this.Lib.importBook(openFileDialog.FileName);
            }
        }

        public void openBook(string path)
        {
            this.rightPannel.Children.Clear();
            StreamReader sr = new StreamReader(path);
            string book = sr.ReadToEnd();
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run(book));
            FlowDocument doc = new FlowDocument();
            doc.Blocks.Add(p);
            FlowDocumentReader read = new FlowDocumentReader();
            read.Document = doc;
            this.rightPannel.Children.Add(read);

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
