using System;
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
        public MainWindow()
        {
            InitializeComponent();
            Library lib = new Library();
            lib.Load();
            ObservableCollection<Book> listRecentRead = new ObservableCollection<Book>();
            ObservableCollection<Book> listRecentImported = new ObservableCollection<Book>();
            ObservableCollection<Book> listFavorites = new ObservableCollection<Book>();
            ObservableCollection<Book> listBooks = new ObservableCollection<Book>();
            foreach(Book b in lib.books){
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

        public void openBook(string path)
        {
            StreamReader sr = new StreamReader(path);
            string book = sr.ReadToEnd();
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run(book));
            FlowDocument doc = new FlowDocument();
            doc.Blocks.Add(p);
            FlowDocumentReader read = new FlowDocumentReader();
            read.Document = doc;
            this.Content = read;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
