using MahApps.Metro.Controls;
using ShoppingListAppLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ShoppingListApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CreateColesForm : MetroWindow, INotifyPropertyChanged
    {
        private const string rootPath = @"C:\Users\gunje\Documents\ShoppingListProjectFiles\ShoppingListApp";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private string ConnectionString { get; set; }

        private string _Visible = "Visible";
        public string VisibleNav
        {
            get { return _Visible; }
            set { _Visible = value; OnPropertyChanged(); }
        }

        private string _FirstButtonText = "1";
        public string FirstButtonText
        {
            get { return _FirstButtonText; }
            set { _FirstButtonText = value; OnPropertyChanged(); }
        }

        private string _SecondButtonText;
        public string SecondButtonText
        {
            get { return _SecondButtonText; }
            set { _SecondButtonText = value; OnPropertyChanged(); }
        }

        private string _ThirdButtonText;
        public string ThirdButtonText
        {
            get { return _ThirdButtonText; }
            set { _ThirdButtonText = value; OnPropertyChanged(); }
        }

        private string _ThirdButtonVisible;
        public string ThirdButtonVisible
        {
            get { return _ThirdButtonVisible; }
            set { _ThirdButtonVisible = value; OnPropertyChanged(); }
        }

        private string _FourthButtonText;
        public string FourthButtonText
        {
            get { return _FourthButtonText; }
            set { _FourthButtonText = value; OnPropertyChanged(); }
        }

        private string _FourthButtonVisible;
        public string FourthButtonVisible
        {   get { return _FourthButtonVisible; }
            set { _FourthButtonVisible = value; OnPropertyChanged(); } 
        }

        private string _FifthButtonText;
        public string FifthButtonText
        {
            get { return _FifthButtonText; }
            set { _FifthButtonText = value; OnPropertyChanged(); }
        }

        private string _FifthButtonVisible;
        public string FifthButtonVisible
        {
            get { return _FifthButtonVisible; }
            set { _FifthButtonVisible= value; OnPropertyChanged(); }
        }

        private string _SixthButtonText;
        public string SixthButtonText
        {
            get { return _SixthButtonText; }
            set { _SixthButtonText = value; OnPropertyChanged(); }
        }

        private string _SixthButtonVisible;
        public string SixthButtonVisible
        {
            get { return _SixthButtonVisible; }
            set { _SixthButtonVisible = value; OnPropertyChanged(); }
        }

        private string _SeventhButtonText;
        public string SeventhButtonText
        {
            get { return _SeventhButtonText; }
            set { _SeventhButtonText = value; OnPropertyChanged(); }
        }
        private string _SeventhButtonVisible;
        public string SeventhButtonVisible
        {
            get { return _SeventhButtonVisible; }
            set { _SeventhButtonVisible = value; OnPropertyChanged(); }
        }
        public Dictionary<string, string> ImgKeyValuePair { get; set; } = new Dictionary<string, string>();

        private int _FlyoutCornerRadius = 30;
        public int FlyoutCornerRadius
        { 
            get { return _FlyoutCornerRadius;}
            set { _FlyoutCornerRadius = value; OnPropertyChanged(); }
        }
        private int _numRows = 1;
        public int NumRows
        {
            get { return this._numRows; }
            set
            { this._numRows = value; OnPropertyChanged();}
        }

        private List<string>? _PageNavigationList;
        public List<string>? PageNavigationList
        {
            get { return _PageNavigationList; }
            set { _PageNavigationList = value; OnPropertyChanged(); }
        }

        private List<SupermarketModel> _Products;
        public List<SupermarketModel> Products
        { 
            get { return this._Products; }
            set { this._Products= value; OnPropertyChanged(); }
        }

        private List<SupermarketModel> _ShoppingCartProduct = new List<SupermarketModel>();
        public List<SupermarketModel> ShoppingCartProduct
        {
            get { return this._ShoppingCartProduct;}
            set { this._ShoppingCartProduct = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Load the MainWindow and initialise the searchbar watermark
        /// </summary>
        public CreateColesForm()
        {
            InitializeComponent();
            this.DataContext = this;
            IConfigurationBuilder build = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            GetColesData cd_0 = new GetColesData();
            Products = cd_0.GetPageData(cd_0.CategoryUrls, out List<string>? urlList, category:ColesCategories.Household);
            PageNavigationList = urlList;
           
            //IConfiguration _configuration = build.Build();
            //string ConnectionString = _configuration.GetConnectionString("dbConnection");

            List<string> directories = Directory.EnumerateFiles(rootPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(d => d.EndsWith("png")).ToList();

            int i = 0;
            
            foreach (string dir in directories)
            {
                ImgKeyValuePair.Add(Enum.GetName(typeof(ColesCategories), i), dir);
                i++;
            }
            
            searchBar.Text = "Search";
            searchBar.Foreground = Brushes.DarkSlateGray;
        }

        /// <summary>
        /// Refreshes search bar when enter is pressed and diverts focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBar_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (e.Key == Key.Enter) 
            {
                tb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                string Text = searchBar.Text;
                GetColesData cd = new GetColesData();
                Products = cd.GetPageData(cd.SearchUrls, out List<string>? urlList, searchText:Text);
                PageNavigationList = urlList;
                searchBar.Foreground = Brushes.DarkSlateGray;
                searchBar.Text = "Search";
            }
        }
        /// <summary>
        /// Single mouse click outside any element but on the grid
        /// will divert focus to the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainGrid.Focus();
        }

        /// <summary>
        /// Checks if the search bar just got focused and removes the watermark.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchBar.Text == "Search")
            {
                searchBar.Text = "";
                searchBar.Foreground = Brushes.Black;
            }
            string search = searchBar.Text;
        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Application.Current.MainWindow.ActualHeight < 700)
            {
                FlyoutCornerRadius = 30;
            }
            else if (Application.Current.MainWindow.ActualHeight > 800)
            {
                FlyoutCornerRadius = 50;
            }
            else
            {
                FlyoutCornerRadius = 40;
            }
            if (Application.Current.MainWindow.ActualWidth > 1750)
            {
                NumRows = 3;
            }
            else if (Application.Current.MainWindow.ActualWidth < 1280)
            {
                NumRows = 1;
            }
            else
            {
                NumRows = 2;
            }
        }

        private void shpCartItemButton_Click(object sender, RoutedEventArgs e)
        {
            SupermarketModel currItem = (sender as Button).DataContext as SupermarketModel;
            ShoppingCartProduct.Add(currItem);

        }

        private void centralFlyoutButton_Click(object sender, RoutedEventArgs e)
        {
            CentralFlyout.IsOpen = true;
        }

        private void CentralFlyoutCloseButton_Click(object sender, RoutedEventArgs e)
        {
            CentralFlyout.IsOpen = false;
        }

        private void CategorySelectionButton_Click(object sender, RoutedEventArgs e)
        {

            var currItem = (sender as Button).DataContext.ToString();
            currItem = currItem.Substring(0, currItem.IndexOf(","))
                .Remove(0, 1);
            ColesCategories Category = Enum.Parse<ColesCategories>(currItem);
            GetColesData cd = new GetColesData();
            Products = cd.GetPageData(cd.CategoryUrls, out List<string>? urlList, category: Category);
            PageNavigationList = urlList;

            NavButtonVisibilityChange();

            if (PageNavigationList is not null)
            {
                FirstButtonText = "1";
            }

            if (PageNavigationList.Count >= 7)
            {
                SeventhButtonText = PageNavigationList.Count.ToString();
            }

        }

        private void shpCartNavigationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NavButtonVisibilityChange()
        {
            int? pageCount = PageNavigationList.Count;

            switch (pageCount)
            {
                case 2:
                    SeventhButtonVisible = "Hidden";
                    SixthButtonVisible = "Hidden";
                    FifthButtonVisible = "Hidden";
                    FourthButtonVisible = "Hidden";
                    ThirdButtonVisible = "Hidden";
                    SecondButtonText= "2";
                    break;
                case 3:
                    SeventhButtonVisible = "Hidden";
                    SixthButtonVisible = "Hidden";
                    FifthButtonVisible = "Hidden";
                    FourthButtonVisible = "Hidden";
                    ThirdButtonVisible = "Visible";
                    SecondButtonText = "2";
                    ThirdButtonText= "3";
                    break;
                case 4:
                    SeventhButtonVisible = "Hidden";
                    SixthButtonVisible = "Hidden";
                    FifthButtonVisible = "Hidden";
                    FourthButtonVisible = "Visible";
                    ThirdButtonVisible = "Visible";
                    SecondButtonText = "2";
                    ThirdButtonText= "3";
                    FourthButtonText= "4";
                    break;
                case 5:
                    SeventhButtonVisible = "Hidden";
                    SixthButtonVisible = "Hidden";
                    FifthButtonVisible = "Visible";
                    FourthButtonVisible = "Visible";
                    ThirdButtonVisible = "Visible";
                    SecondButtonText = "2";
                    ThirdButtonText = "3";
                    FourthButtonText = "4";
                    FifthButtonText= "5";
                    break;
                case 6:
                    SeventhButtonVisible = "Hidden";
                    SixthButtonVisible = "Visible";
                    FifthButtonVisible = "Visible";
                    FourthButtonVisible = "Visible";
                    ThirdButtonVisible = "Visible";
                    SecondButtonText = "2";
                    ThirdButtonText = "3";
                    FourthButtonText = "4";
                    FifthButtonText = "5";
                    SixthButtonText= "6";
                    break;
                case >= 7:
                    SeventhButtonVisible = "Visible";
                    SixthButtonVisible = "Visible";
                    FifthButtonVisible = "Visible";
                    FourthButtonVisible = "Visible";
                    ThirdButtonVisible = "Visible";
                    break;
                default:
                    VisibleNav = "Hidden";
                    break;
            }
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            if (SecondButtonText != "..." && SecondButtonText != "")
            {
                GetColesData cd = new GetColesData();
                Products = cd.GetPageData(PageNavigationList[Int16.Parse(SecondButtonText) - 1]);

                SecondButtonText = "...";
                ThirdButtonText = $"{Int16.Parse(ThirdButtonText) + 3}";
                FourthButtonText = $"{Int16.Parse(FourthButtonText) + 3}";
                FifthButtonText = $"{Int16.Parse(FifthButtonText) + 3}";
            }
            else if (SecondButtonText)
            else if (SecondButtonText == "..." && ThirdButtonText == "4")
                    {

                    }
                    else if (SecondButtonText == "...")
                    {
                        ThirdButtonText = $"{Int16.Parse(ThirdButtonText) - 3}";
                        FourthButtonText = $"{Int16.Parse(FourthButtonText) - 3}";
                        FifthButtonText = $"{Int16.Parse(FifthButtonText) - 3}";
                    }
                    else
                    {
                        throw new Exception("The value in ")
                    }
        }

        private void SixthButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
