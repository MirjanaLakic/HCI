using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

namespace ReadingIsFun
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double MarginSmall = 0.03125,MarginMedium = 0.125,MarginBig = 0.25;
        private const int LineSpaceSmall = 12, LineSpaceMedium = 24, LineSpaceBig = 36;
        private const int TinyFontSize = 16, SmallFontSize = 20, MediumFontSize = 24, LargeFontSize=28, HugeFontSize = 32;
        private Books Library { get; set; }
        private string CurrentBook { get; set; }
        private double MarginPercent { get; set; }
        private int LineSpacing { get; set; }
        private string BookFont { get; set; }
        private Theme AppTheme { get; set; }
        private int BookFontSize { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += this.key_events;
            Library = new Books();
            CurrentBook = null;
            this.DataContext = this;
            EditRecentMenu();
            EditBooksMenu();
            LoadData();
            
            this.SizeChanged += ExpandMargins;
        }
        //Otvaranje knjige
            //klasicno
        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                openBook(openFileDialog.FileName);
            }
        }
            ///Drop event
        private void doc_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }
        private void doc_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files[0].Split('.').Last() == "txt")
                    openBook(files[0]);
                else
                {
                    if (CurrentBook != null)
                    {
                        Library.BookList[CurrentBook].LastPage = docReader.MasterPageNumber / (docReader.PageCount * 1.0);
                    }
                    FlowDocument bookContent = new FlowDocument();
                    bookContent.ColumnWidth = double.MaxValue;
                    bookContent.Background = new SolidColorBrush(Color.FromArgb(255, 219, 219, 219));
                    bookContent.AllowDrop = true;
                    bookContent.DragOver += doc_DragOver;
                    bookContent.Drop += doc_Drop;
                    bookContent.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#" + BookFont);
                    bookContent.LineHeight = LineSpacing;
                    bookContent.FontSize = BookFontSize;
                    bookContent.Background = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookBackgroundColor);
                    bookContent.Foreground = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookTypingColor);
                    double margin = MarginPercent * Width;
                    bookContent.PagePadding = new Thickness(margin, 10, margin, 10);
                    Paragraph p = new Paragraph();
                    p.Inlines.Add(new Run("File that has been droped is not supported by this program.\nPlease use books that have .txt format."));
                    bookContent.Blocks.Add(p);
                    docReader.Document = bookContent;
                    CurrentBook = null;
                }
            }
        }
            ///Recent knjige
        private void OpenRecent(object sender, EventArgs e)
        {
            string path = (string)(((MenuItem)sender).CommandParameter);
            if (!File.Exists(path))
            {
                if (CurrentBook != null)
                {
                    Library.BookList[CurrentBook].LastPage = docReader.MasterPageNumber / (docReader.PageCount * 1.0);
                }
                FlowDocument bookContent = new FlowDocument();
                bookContent.ColumnWidth = double.MaxValue;
                bookContent.Background = new SolidColorBrush(Color.FromArgb(255, 219, 219, 219));
                bookContent.AllowDrop = true;
                bookContent.DragOver += doc_DragOver;
                bookContent.Drop += doc_Drop;
                bookContent.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#" + BookFont);
                bookContent.LineHeight = LineSpacing;
                bookContent.FontSize = BookFontSize;
                bookContent.Background = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookBackgroundColor);
                bookContent.Foreground = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookTypingColor);
                double margin = MarginPercent * Width;
                bookContent.PagePadding = new Thickness(margin, 10, margin, 10);
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Run("Path to the book has been changed, or book got deleted.\nBook will be removed from recent books and all books list.\nPlease open the book, or drag and drop it, from its new location."));
                bookContent.Blocks.Add(p);
                docReader.Document = bookContent;

                CurrentBook = null;
                Library.Recent.Remove(path);
                Library.BookList.Remove(path);
                EditRecentMenu();
                EditBooksMenu();
                return;
            }
            openBook(path);
        }
            ///funkcija otvaranja
        public void openBook(string path)
        {
            if (CurrentBook != null)
            {
                Library.BookList[CurrentBook].LastPage = docReader.MasterPageNumber / (docReader.PageCount * 1.0);
            }
            ((FlowDocument)docReader.Document).Blocks.Clear();
            StreamReader sr = new StreamReader(path);
            FlowDocument bookContent = new FlowDocument();
            bookContent.ColumnWidth = double.MaxValue;
            bookContent.Background = new SolidColorBrush(Color.FromArgb(255, 219, 219, 219));
            bookContent.AllowDrop = true;
            bookContent.DragOver += doc_DragOver;
            bookContent.Drop += doc_Drop;
            bookContent.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#"+BookFont);
            bookContent.LineHeight = LineSpacing;
            bookContent.FontSize = BookFontSize;
            bookContent.Background = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookBackgroundColor);
            bookContent.Foreground = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookTypingColor);
            double margin = MarginPercent * Width;
            bookContent.PagePadding = new Thickness(margin, 10,margin, 10);
            Book data = Library.getBook(path);
            Paragraph p = new Paragraph();
            string book = sr.ReadLine();
            while (book != null)
            {
                if(book=="")
                    book = "\n";
                p.Inlines.Add(new Run(book));
                book = sr.ReadLine();
            }
            bookContent.Blocks.Add(p);
            docReader.Document = bookContent;
            sr.Close();
            CurrentBook = path;
            EditRecentMenu();
            EditBooksMenu();
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
            int page = (int)(data.LastPage * docReader.PageCount);
            docReader.GoToPage(page);
            docReader.Focus();
            return;
        }
        //Upravljanje prozorom
            //fullscreen opcija
        public void fullscreen(object sender, EventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                menuBar.Visibility = Visibility.Collapsed;
            }
            else if(this.WindowState == WindowState.Maximized && this.WindowStyle != WindowStyle.None)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                menuBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                menuBar.Visibility = Visibility.Visible;
            }
            docReader.Focus();
        }
            //zatvaranje prozora
        private void Window_Closing(object sender, EventArgs e)
        {
            if (CurrentBook != null)
            {
                Library.BookList[CurrentBook].LastPage = docReader.MasterPageNumber / (docReader.PageCount * 1.0);
            }
            Library.Save();
            SaveData();
        }
            //key eventovi
        public void key_events(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {
                fullscreen(sender, e);
            }
            if (e.Key == Key.Escape)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    menuBar.Visibility = Visibility.Visible;
                }
            }
            if (e.Key == Key.Subtract)
            {
                if (docReader.CanDecreaseZoom) { 
                    docReader.DecreaseZoom();
                }
            }
            if (e.Key == Key.Add)
            {
                if (docReader.CanIncreaseZoom) { 
                    docReader.IncreaseZoom();
                }
            }
            if((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) { 
                if (e.Key == Key.O)
                {
                    OpenMenuItem_Click(sender, e);
                }
            }
            if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                if (this.WindowState == WindowState.Maximized && this.WindowStyle == WindowStyle.None) {
                    if (menuBar.Visibility == Visibility.Visible)
                        menuBar.Visibility = Visibility.Collapsed;
                    else
                        menuBar.Visibility = Visibility.Visible;
                }
            }

            }
            //recent meni kreiranje
        private void EditRecentMenu()
        {
            recentBooksListMenu.Items.Clear();
            foreach (var item in Library.Recent)
            {
                MenuItem mi = new MenuItem();
                mi.Header = item.Split('\\').Last().Split('.')[0];
                mi.CommandParameter = item;
                mi.Click += OpenRecent;
                mi.ToolTip = "Opens book at " + item;
                recentBooksListMenu.Items.Add(mi);
            }
        }
        //all books menu
        private void EditBooksMenu()
        {
            booksListMenu.Items.Clear();
            List<string> temp = new List<string>();
            foreach (var item in Library.BookList)
            {
                temp.Add((string)(item.Key));
            }
            Sorter srt = new Sorter(temp);
            SortedDictionary<string, List<Tuple<string, string>>> result = srt.Sort();
            foreach(var pair in result)
            {
                MenuItem mi = new MenuItem();
                mi.Header = pair.Key;
                foreach(var listItem in pair.Value)
                {
                    MenuItem smi = new MenuItem();
                    smi.Header = listItem.Item1;
                    smi.CommandParameter = listItem.Item2;
                    smi.Click += OpenRecent;
                    smi.ToolTip = "Opens book at: " + listItem.Item2;
                    mi.Items.Add(smi);
                }
                booksListMenu.Items.Add(mi);
            }
        }
        //snimanje podataka
        private void SaveData()
        {
            if (!Directory.Exists(".\\ProgramData"))
                Directory.CreateDirectory(".\\ProgramData");
            BinaryWriter bw = new BinaryWriter(new FileStream(".\\ProgramData\\settings.bin", FileMode.Create));
            bw.Write(MarginPercent);
            bw.Write(LineSpacing);
            bw.Write(BookFont);
            bw.Write(BookFontSize);
            AppTheme.Save(bw);
            bw.Close();
        }
            //ucitavanje podataka
        private void LoadData()
        {
            if (!Directory.Exists(".\\ProgramData"))
                Directory.CreateDirectory(".\\ProgramData");
            BinaryReader br = new BinaryReader(new FileStream(".\\ProgramData\\settings.bin", FileMode.OpenOrCreate));
            if (br.PeekChar() != -1)
            {
                MarginPercent = br.ReadDouble();
                LineSpacing = br.ReadInt32();
                BookFont = br.ReadString();
                BookFontSize = br.ReadInt32();
                AppTheme = new Theme();
                AppTheme.Load(br);
            }
            else
            {
                MarginPercent = MarginSmall;
                LineSpacing = LineSpaceSmall;
                BookFontSize = MediumFontSize;
                BookFont = "Nunito Sans Regular";
                AppTheme = new Theme();
            }
            br.Close();

            //pronadji i stikliraj
            ((FlowDocument)docReader.Document).Background = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookBackgroundColor);
            ((FlowDocument)docReader.Document).Foreground = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookTypingColor);
            docReader.Background = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookToolBarColor);
            docReader.Foreground = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookTypingColor);
            foreach (MenuItem item in fontPickerMenu.Items)
            {
                if (item.Header.Equals(BookFont))
                {
                    item.IsChecked = true;
                    break;
                }
            }
            string search = "";
            switch (MarginPercent)
            {
                case MarginSmall:
                    search = "Small";
                    break;
                case MarginMedium:
                    search = "Medium";
                    break;
                case MarginBig:
                    search = "Large";
                    break;
            }
            foreach (MenuItem item in marginsMenu.Items)
            {
                if (item.Header.Equals(search))
                {
                    item.IsChecked = true;
                    break;
                }
            }
            switch (LineSpacing)
            {
                case LineSpaceSmall:
                    search = "Small";
                    break;
                case LineSpaceMedium:
                    search = "Medium";
                    break;
                case LineSpaceBig:
                    search = "Large";
                    break;
            }
            foreach (MenuItem item in spacingMenu.Items)
            {
                if (item.Header.Equals(search))
                {
                    item.IsChecked = true;
                    break;
                }
            }
            foreach (MenuItem item in themeMenu.Items)
            {
                if (item.Header.Equals(AppTheme.Name))
                {
                    item.IsChecked = true;
                    break;
                }
            }
            switch (BookFontSize)
            {
                case TinyFontSize:
                    search = "Tiny";
                    break;
                case SmallFontSize:
                    search = "Small";
                    break;
                case MediumFontSize:
                    search = "Medium";
                    break;
                case LargeFontSize:
                    search = "Large";
                    break;
                case HugeFontSize:
                    search = "Huge";
                    break;

            }
            foreach (MenuItem item in fontSizesMenu.Items)
            {
                if (item.Header.Equals(search))
                {
                    item.IsChecked = true;
                    break;
                }
            }
        }
        
        private void ExpandMargins(object sender, EventArgs e)
        {
                SetMargin(MarginPercent);
        }
        private void FontMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var menu in fontPickerMenu.Items)
            {
                if (((MenuItem)menu).IsChecked == true)
                {
                    ((MenuItem)menu).IsChecked = false;
                    break;
                }
            }
            ((MenuItem)sender).IsChecked = true;
            SetFont(((string)((MenuItem)sender).Header));

        }
        private void FontSizeMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var menu in fontSizesMenu.Items)
            {
                if (((MenuItem)menu).IsChecked == true)
                {
                    ((MenuItem)menu).IsChecked = false;
                    break;
                }
            }
            ((MenuItem)sender).IsChecked = true;
            switch (((MenuItem)sender).Header){
                case "Tiny":
                    SetFontSize(TinyFontSize);
                    break;
                case "Small":
                    SetFontSize(SmallFontSize);
                    break;
                case "Medium":
                    SetFontSize(MediumFontSize);
                    break;
                case "Large":
                    SetFontSize(LargeFontSize);
                    break;
                case "Huge":
                    SetFontSize(HugeFontSize);
                    break;
            }
        }

        

        private void MarginsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var menu in marginsMenu.Items)
            {
                if (((MenuItem)menu).IsChecked == true)
                {
                    ((MenuItem)menu).IsChecked = false;
                    break;
                }
            }
            ((MenuItem)sender).IsChecked = true;
            switch (((string)((MenuItem)sender).Header))
            {
                case "Small":
                    SetMargin(MarginSmall);
                    break;
                case "Medium":
                    SetMargin(MarginMedium);
                    break;
                case "Large":
                    SetMargin(MarginBig);
                    break;
            }
            
        }
        private void SpacingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var menu in spacingMenu.Items)
            {
                if (((MenuItem)menu).IsChecked == true)
                {
                    ((MenuItem)menu).IsChecked = false;
                    break;
                }
            }
            ((MenuItem)sender).IsChecked = true;
            switch (((string)((MenuItem)sender).Header))
            {
                case "Small":
                    SetSpacing(LineSpaceSmall);
                    break;
                case "Medium":
                    SetSpacing(LineSpaceMedium);
                    break;
                case "Large":
                    SetSpacing(LineSpaceBig);
                    break;
            }
        }
        private void ThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var menu in themeMenu.Items)
            {
                if (((MenuItem)menu).IsChecked == true)
                {
                    ((MenuItem)menu).IsChecked = false;
                    break;
                }
            }
            ((MenuItem)sender).IsChecked = true;
            SetTheme(((string)((MenuItem)sender).Header));
        }

        private void SetFont(string font)
        {
            BookFont = font;
            ((FlowDocument)docReader.Document).FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#" + font);
            
        }
        private void SetMargin(double percent)
        {
            MarginPercent = percent;
            double margin = percent * Width;
            ((FlowDocument)docReader.Document).PagePadding = new Thickness(margin, 10, margin, 10);
            if(percent == MarginBig)
            {
                if (docReader.Zoom > 150)
                    docReader.Zoom = 150;
                docReader.MaxZoom = 150;
            }
            else
            {
                docReader.MaxZoom = 200;
            }
        }
        private void SetSpacing(int spacing)
        {
            LineSpacing = spacing;
            ((FlowDocument)docReader.Document).LineHeight = spacing;
        }
        private void SetTheme(string theme)
        {
            AppTheme = new Theme(theme);
            ((FlowDocument)docReader.Document).Background = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookBackgroundColor);
            ((FlowDocument)docReader.Document).Foreground = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookTypingColor);
            docReader.Background = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookToolBarColor);
            docReader.Foreground = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookTypingColor);
        }
        private void SetFontSize(int size)
        {
            BookFontSize = size;
            ((FlowDocument)docReader.Document).FontSize = size;
        }
        //Help menus
        ///Reader options help
        private void MenuItemHelpOptions_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBook != null)
            {
                Library.BookList[CurrentBook].LastPage = docReader.MasterPageNumber / (docReader.PageCount * 1.0);
            }
            FlowDocument bookContent = new FlowDocument();
            bookContent.ColumnWidth = double.MaxValue;
            bookContent.Background = new SolidColorBrush(Color.FromArgb(255, 219, 219, 219));
            bookContent.AllowDrop = true;
            bookContent.DragOver += doc_DragOver;
            bookContent.Drop += doc_Drop;
            bookContent.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#" + BookFont);
            bookContent.LineHeight = LineSpacing;
            bookContent.FontSize = BookFontSize;
            bookContent.Background = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookBackgroundColor);
            bookContent.Foreground = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookTypingColor);
            double margin = MarginPercent * Width;
            bookContent.PagePadding = new Thickness(margin, 10, margin, 10);
            //
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run("Reading options help book\n"));
            bookContent.Blocks.Add(p);
            p = new Paragraph();
            p.Inlines.Add(new Run("Theme\n\n"));
            p.Inlines.Add(new Run("Original - Black letters on gray background. This theme is trying to emulate e-book readers.\n"));
            p.Inlines.Add(new Run("Clasic - Black letters on light yellow background trying to emulate experience of reading a book.\n"));
            p.Inlines.Add(new Run("Black - White letters on black background. This theme is trying to minimize stress on your eyes during night time.\n"));
            p.Inlines.Add(new Run("Blue Light Filter - Dark brown letters on dark grey background, trying to reduce effects of blue light during night time.\n"));
            bookContent.Blocks.Add(p);
            p = new Paragraph();
            p.Inlines.Add(new Run("Fonts\n\n"));
            p.Inlines.Add(new Run("Serif fonts\n"));
            p.Inlines.Add(new Run("Serif fonts are fonts that contain small line attached to the end of a stroke in a letter or symbol. These fonts are mostly optimized for reading on high pixel density screen. Serif fonts supported in our program are Alegreya, Libre Baskreville, and Lora.\n"));
            p.Inlines.Add(new Run("Sans-Serif fonts\n"));
            p.Inlines.Add(new Run("Sans-Serif fonts are fonts that do not contain small line attached to the end of a stroke in a letter or symbol. These fonts are mostly optimized for reading on low pixel density screen. Sans-Serif fonts supported in our program are Lato, NunitoSans, and Source Sans Pro.\n"));
            p.Inlines.Add(new Run("\nFor every font there is a bold version provided to help with easier reading.\n"));
            bookContent.Blocks.Add(p);
            p = new Paragraph();
            p.Inlines.Add(new Run("Font size, margins, line spacing\n\n"));
            p.Inlines.Add(new Run("Font size alows you to customize size of letters. \n"));
            p.Inlines.Add(new Run("Margins defines position from which text will start in each row, while keeping your text centered.\n"));
            p.Inlines.Add(new Run("Line spacing helps you define density of text. With small line spacing there will be more lines of text displayed on screen.\n"));
            bookContent.Blocks.Add(p);
            //
            docReader.Document = bookContent;
            CurrentBook = null;
            docReader.Focus();
        }

        ///Commands help
        private void MenuItemHelpCommands_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBook != null)
            {
                Library.BookList[CurrentBook].LastPage = docReader.MasterPageNumber / (docReader.PageCount * 1.0);
            }
            FlowDocument bookContent = new FlowDocument();
            bookContent.ColumnWidth = double.MaxValue;
            bookContent.Background = new SolidColorBrush(Color.FromArgb(255, 219, 219, 219));
            bookContent.AllowDrop = true;
            bookContent.DragOver += doc_DragOver;
            bookContent.Drop += doc_Drop;
            bookContent.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#" + BookFont);
            bookContent.LineHeight = LineSpacing;
            bookContent.FontSize = BookFontSize;
            bookContent.Background = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookBackgroundColor);
            bookContent.Foreground = (Brush)new BrushConverter().ConvertFromString(AppTheme.BookTypingColor);
            double margin = MarginPercent * Width;
            bookContent.PagePadding = new Thickness(margin, 10, margin, 10);
            //
            Paragraph p = new Paragraph();
            p.Inlines.Add(new Run("Reading is fun commands list. Commands are listed in format: Command - shortcut;shortcut2;\n"));
            bookContent.Blocks.Add(p);
            p = new Paragraph();
            string array = "Next page - arrow right; arrow down; scroll down; page down;\n"+
                "Previous page - arrow left; arrow up; scroll up; page up;\n"+
                "Go to first page - home;\n"+
                "Go to last page - end;\n"+
                "Enter fullscreen - F4;\n"+
                "Exit fullscreen - Esc; F4;\n"+
                "Zoom in - \'+\';Ctrl + \'+\';\n"+
                "Zoom out - \'-\';Ctrl + \'-\';\n"+
                "Open book menu - Ctrl + O;\n"+
                "Open/Hide menu in fullscreen - Alt;\n";
            p.Inlines.Add(new Run(array));
            bookContent.Blocks.Add(p);
            //
            docReader.Document = bookContent;
            CurrentBook = null;
            docReader.Focus();
        }
    }
}
