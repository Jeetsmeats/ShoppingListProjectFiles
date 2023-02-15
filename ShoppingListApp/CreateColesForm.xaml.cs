using MahApps.Metro.Controls;
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
using System.IO;
using ProductsLibrary.WebscraperMethods;
using ProductsLibrary.Models;
using ProductsLibrary.Data;

namespace ShoppingListApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CreateColesForm : MetroWindow, INotifyPropertyChanged
    {
        private const string rootPath = @"C:\Users\gunje\Documents\ShoppingListProjectFiles\ShoppingListApp\coles-png-images";

        private readonly IProductData dataAccess;

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

        private string _SeventhButtonText = "";
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
        public CreateColesForm(IProductData dataAccess)
        {
            InitializeComponent();
            this.DataContext = this;
            this.dataAccess = dataAccess;

            Products = GetColesData.GetPageData(GetColesData.CategoryUrls, out List<string>? urlList, category:ColesCategories.Household);
            PageNavigationList = urlList;
            NavButtonVisibilityChange();

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
                
                Products = GetColesData.GetPageData(GetColesData.SearchUrls, out List<string>? urlList, searchText:Text);
                PageNavigationList = urlList;
                NavButtonVisibilityChange();

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
            dataAccess.InsertProduct(currItem);
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

            Products = GetColesData.GetPageData(GetColesData.CategoryUrls, out List<string>? urlList, category: Category);
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
            int? pageCount = PageNavigationList is not null ? PageNavigationList.Count : null;
            VisibleNav = "Visible";
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

                    SecondButtonText = "2";
                    ThirdButtonText = "3";
                    FourthButtonText = "4";
                    FifthButtonText= "5";
                    SixthButtonText = "...";
                    SeventhButtonText = $"{pageCount}";

                    break;
                default:
                    VisibleNav = "Hidden";
                    break;
            }
            Debug.WriteLine("leg");
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (SecondButtonText == "2")
            {
                Products = GetColesData.GetPageData(PageNavigationList[Int16.Parse(SecondButtonText) - 1]);

                ThirdButtonText = "3";
                FourthButtonText = "4";
                FifthButtonText = "5";
                SixthButtonText = "...";
            }
            else if (SecondButtonText == "..." && (ThirdButtonText == "6" || ThirdButtonText == "5"))
            {
                SecondButtonText = "2";
                ThirdButtonText = "3";
                FourthButtonText = "4";
                FifthButtonText = "5";
                SixthButtonText = "...";
            }
            else if (SecondButtonText == "..." && (SixthButtonText == $"{PageNavigationList.Count - 1}" || SixthButtonText == "..."))
            {
                SixthButtonText = "...";
                ThirdButtonText = $"{Int16.Parse(ThirdButtonText) - 3}";
                FourthButtonText = $"{Int16.Parse(FourthButtonText) - 3}";
                FifthButtonText = $"{Int16.Parse(FifthButtonText) - 3}";
            }
            else if (SecondButtonText == "..." && (SixthButtonText == "..." && FifthButtonText == "8"))
            {
                SecondButtonText = "2";
                ThirdButtonText = $"{Int16.Parse(ThirdButtonText) - 3}";
                FourthButtonText = $"{Int16.Parse(FourthButtonText) - 3}";
                FifthButtonText = $"{Int16.Parse(FifthButtonText) - 3}";
            }
            
        }

        private void SixthButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (SixthButtonText == $"{PageNavigationList.Count - 1}")
            {
                Products = GetColesData.GetPageData(PageNavigationList[Int16.Parse(SixthButtonText) - 1]);

                SecondButtonText = "...";
                ThirdButtonText = $"{PageNavigationList.Count - 4}";
                FourthButtonText = $"{PageNavigationList.Count - 3}";
                FifthButtonText = $"{PageNavigationList.Count - 2}";
               
            }
            else if (SixthButtonText == "..." && (FifthButtonText == $"{PageNavigationList.Count - 4}" || FifthButtonText == $"{PageNavigationList.Count - 3}" ||
                FifthButtonText == $"{PageNavigationList.Count - 2}"))
            {
                SecondButtonText = "...";
                ThirdButtonText = $"{PageNavigationList.Count - 4}";
                FourthButtonText = $"{PageNavigationList.Count - 3}";
                FifthButtonText = $"{PageNavigationList.Count - 2}";
                SixthButtonText = $"{PageNavigationList.Count - 1}";
            }
            else if (SixthButtonText == "..." && (SecondButtonText == "2" || SecondButtonText == "..."))
            {
                SecondButtonText = "...";
                ThirdButtonText = $"{Int16.Parse(ThirdButtonText) + 3}";
                FourthButtonText = $"{Int16.Parse(FourthButtonText) + 3}";
                FifthButtonText = $"{Int16.Parse(FifthButtonText) + 3}";
            }
            else if (SixthButtonText == "..." && (SecondButtonText == "..." && ThirdButtonText == $"{PageNavigationList.Count - 6}"))
            {
                SixthButtonText = $"{PageNavigationList.Count - 1}";
                ThirdButtonText = $"{Int16.Parse(ThirdButtonText) - 3}";
                FourthButtonText = $"{Int16.Parse(FourthButtonText) - 3}";
                FifthButtonText = $"{Int16.Parse(FifthButtonText) - 3}";
            }
        }

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            Products = GetColesData.GetPageData(PageNavigationList[Int16.Parse(FirstButtonText) - 1]);
            NavButtonVisibilityChange();
        }

        private void ThirdButton_Click(object sender, RoutedEventArgs e)
        {
            Products = GetColesData.GetPageData(PageNavigationList[Int16.Parse(ThirdButtonText) - 1]);
        }

        private void FourthButton_Click(object sender, RoutedEventArgs e)
        {
            Products = GetColesData.GetPageData(PageNavigationList[Int16.Parse(FourthButtonText) - 1]);
        }

        private void FifthButton_Click(object sender, RoutedEventArgs e)
        {
            Products = GetColesData.GetPageData(PageNavigationList[Int16.Parse(FifthButtonText) - 1]);
        }

        private void SeventhButton_Click(object sender, RoutedEventArgs e)
        {
            Products = GetColesData.GetPageData(PageNavigationList[Int16.Parse(SeventhButtonText) - 1]);

            SecondButtonText = "...";
            ThirdButtonText = $"{PageNavigationList.Count - 4}";
            FourthButtonText = $"{PageNavigationList.Count - 3}";
            FifthButtonText = $"{PageNavigationList.Count - 2}";
            SixthButtonText = $"{PageNavigationList.Count - 1}";
        }
    }
}
