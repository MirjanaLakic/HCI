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
using System.Windows.Threading;
using System.IO;

namespace ReadingIsFun
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private Books Library { get; set; }
        private string CurrentBook { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += this.key_events;
            Library = new Books();
            CurrentBook = null;
            this.DataContext = this;
            EditRecentMenu();
        }
        private void doc_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }

        private void doc_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files[0].Split('.').Last() == "txt")
                    openBook(files[0]);
            }
        }
        public void openBook(string path)
        {
            if (CurrentBook != null)
            {
                Library.BookList[CurrentBook].LastPage = docReader.MasterPageNumber;
                Library.BookList[CurrentBook].FontSize = docReader.Zoom;
            }
            ((FlowDocument)docReader.Document).Blocks.Clear();
            StreamReader sr = new StreamReader(path);
            FlowDocument bookContent = new FlowDocument();
            bookContent.ColumnWidth = double.MaxValue;
            bookContent.Background = new SolidColorBrush(Color.FromArgb(255,219,219,219));
            bookContent.AllowDrop = true;
            bookContent.DragOver += doc_DragOver;
            bookContent.Drop += doc_Drop;
            Book data = Library.getBook(path);
            docReader.Zoom = data.FontSize;
            Paragraph p = new Paragraph();
            string book = sr.ReadLine();
            while (book != null)
            {
                book = book + '\n';
                p.Inlines.Add(new Run(book));
                book = sr.ReadLine();
            }
            bookContent.Blocks.Add(p);
            docReader.Document = bookContent;
            sr.Close();
            CurrentBook = path;
            EditRecentMenu();
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
            docReader.GoToPage(data.LastPage);
            return;
        }
        public void fullscreen(object sender, EventArgs e)
        {
            if(this.WindowState != WindowState.Maximized) { 
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
                menuBar.Visibility = Visibility.Collapsed;
            }
        }
        private void Window_Closing(object sender, EventArgs e)
        {
            if (CurrentBook!= null) { 
                Library.BookList[CurrentBook].LastPage = docReader.MasterPageNumber;
                Library.BookList[CurrentBook].FontSize = docReader.Zoom;
            }
            Library.Save();
        }
        public void key_events(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                if(this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    menuBar.Visibility = Visibility.Visible;
                }
            }
            if(e.Key == Key.Subtract)
            {
                if(docReader.CanDecreaseZoom)
                    docReader.DecreaseZoom();
            }
            if (e.Key == Key.Add)
            {
                if (docReader.CanIncreaseZoom)
                    docReader.IncreaseZoom();
            }
        }
        private void OpenRecent(object sender, EventArgs e)
        {
            openBook((string)(((MenuItem)sender).CommandParameter));
        }
        private void EditRecentMenu()
        {
            recentBooksListMenu.Items.Clear();
            foreach(var item in Library.Recent)
            {
                MenuItem mi = new MenuItem();
                mi.Header = item;
                mi.CommandParameter = item;
                mi.Click += OpenRecent;
                recentBooksListMenu.Items.Add(mi);
            }
        }

    }
}
