using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ProductsLibrary.Models;
using System.Windows.Media.Imaging;

namespace ProductsLibrary.WebscraperMethods
{
    public class GetColesData
    {
        /// <summary>
        /// delegate used for getting the number of navigation pages
        /// from a search or browsing a category
        /// </summary>
        /// <param name="url">submitted website address of the first page</param>
        /// <param name="finalPg">number of pages</param>
        /// <returns>List of all the url pages from the given search or category</returns>
        public delegate List<string> GetPageURL(string url, int finalPg);


        /// <summary>
        /// Gets the http client string of the submitted url
        /// </summary>
        /// <param name="url">website address</param>
        /// <returns>Gets the html client string</returns>
        private Task<string> GetHtmlClient(string url)
        {
            #region Get the website html string
            HttpClient httpClient = new HttpClient();
            return httpClient.GetStringAsync(url);
            #endregion
        }
        /// <summary>
        /// Gets the data from each product tile
        /// </summary>
        /// <param name="url">Submitted website address</param>
        /// <returns></returns>
        private List<HtmlNode> GetHtmlNode(string url)
        {
            #region Parse the html into a html document for data manipulation
            string htmlClient = GetHtmlClient(url).Result;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlClient);
            #endregion

            #region Get the descendants of every product tile
            var htmlItems = htmlDocument.DocumentNode.Descendants("section")
            .Where(node => node.GetAttributeValue("data-testid", "")
            .Equals("product-tile")).ToList();

            return htmlItems;
            #endregion
        }

        /// <summary>
        ///     Gets the data from each product tile as well as the number of navigation pages
        /// </summary>
        /// <param name="url">submitted website address</param>
        /// <param name="pgNum">returns the number of a navigation pages</param>
        /// <returns>Gets the data list of descendants of each product tile</returns> 
        private List<HtmlNode> GetHtmlNode(string url, out int? pgNum)
        {
            #region Parse the html into a html document for data manipulation
            string htmlClient = GetHtmlClient(url).Result;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlClient);
            #endregion

            #region Output the number of navigation pages for the specific search or category
            // get the navigation bar data
            var navBar = htmlDocument.DocumentNode.Descendants("nav")
            .Where(node => node.GetAttributeValue("data-testid", "")
            .Equals("pagination")).ToList();
            // return null if no navigation bar else return the number of pages
            if (navBar.Count == 0)
            {
                pgNum = null;
            }
            else
            {
                // return the second last element of the html list item
                // (which is always the number of pages)
                string? ulListElements = navBar[0].Descendants("li")
                .Reverse()
                .Skip(1)
                .FirstOrDefault()
                .InnerText;

                pgNum = short.Parse(ulListElements);
            }
            #endregion

            #region Get the descendants of every product tile
            var htmlItems = htmlDocument.DocumentNode.Descendants("section")
            .Where(node => node.GetAttributeValue("data-testid", "")
            .Equals("product-tile")).ToList();

            return htmlItems;
            #endregion
        }


        /// <summary>
        ///  Uses product tile data to get product names
        /// </summary>
        /// <param name="htmlData">List of the data from every product tile in the page</param>
        /// <returns>Gets the list of product names</returns>
        private List<string> GetProductName(List<HtmlNode> htmlData)
        {

            #region Get data from each product tile node and instantiate useful variables
            var productProperties = htmlData;

            List<string> productNamesList = new List<string>();
            string? currProd;
            int ind;
            #endregion

            #region Parse each product tile data and get the product names
            foreach (var product in productProperties)
            {
                // Get the text inside h2 header
                currProd = product.Descendants("h2")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Contains("product__title"))
                    .FirstOrDefault()?
                    .InnerText ?? string.Empty;

                // Include apostrophes
                if (currProd.Contains("&#x27;"))
                {
                    currProd = currProd.Replace("&#x27;", $"{(char)39}");
                }
                if (currProd.Contains("&amp;"))
                {
                    currProd = currProd.Replace("&amp;", "&");
                }
                // Trim the header for better data quality
                ind = currProd.IndexOf('|') - 1;
                currProd = currProd.Remove(ind);
                productNamesList.Add(currProd);
            }


            return productNamesList;
            #endregion

        }


        /// <summary>
        /// Uses product tile data to get product prices
        /// </summary>
        /// <param name="htmlData">List of data from every product tile in the page</param>
        /// <returns>Gets the list product prices</returns>
        private List<decimal> GetProductPrice(List<HtmlNode> htmlData)
        {
            #region Get data from each product tile node and instantiate useful variables
            var productProperties = htmlData;

            string? currProd;
            List<decimal> priceList = new List<decimal>();
            decimal price;
            #endregion

            #region Parse each product tile data and get the price of each item
            foreach (var product in productProperties)
            {
                // get the inner text value of span elements, if there
                // is none assume price is zero
                currProd = product.Descendants("span")
                        .Where(node => node.GetAttributeValue("class", "")
                        .Equals("price__value"))
                        .FirstOrDefault()?
                        .InnerText;
                if (currProd is null)
                {
                    currProd = " 0";
                }
                currProd = currProd.Substring(1);
                price = Convert.ToDecimal(currProd);

                priceList.Add(price);
            }
            return priceList;
            #endregion 
        }


        /// <summary>
        /// Uses product tile data to get product quantities
        /// </summary>
        /// <param name="htmlData">List of data from every product tile in the page</param>
        /// <returns>Gets the list of product quantities</returns>
        private List<string> GetProductQuantity(List<HtmlNode> htmlData)
        {
            #region Get data from each product tile node and instantiate useful variables
            var productProperties = htmlData;

            string? currProd;
            List<string> quantityList = new List<string>();
            int ind;
            #endregion

            #region Parse each product tile data and get the quantity of each item
            foreach (var product in productProperties)
            {
                // Get the inner text items of each h2 header element,
                // if none then assume it is an empty string 
                currProd = product.Descendants("h2")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Contains("product__title"))
                    .FirstOrDefault()?
                    .InnerText;

                if (currProd is null)
                {
                    quantityList.Add("");
                }
                else
                {
                    ind = currProd.IndexOf('|') + 2;
                    currProd = currProd.Remove(0, ind);
                    quantityList.Add(currProd);
                }
            }
            return quantityList;
            #endregion
        }


        /// <summary>
        /// Uses product tile data to get relative product prices
        /// </summary>
        /// <param name="htmlData">List of data from every product tile in the page</param>
        /// <returns>Gets the list of relative product prices</returns>
        private List<string> GetPricePerQuantity(List<HtmlNode> htmlData)
        {
            #region Get data from each product tile node and instantiate useful variables
            var productProperties = htmlData;
            string? currProd;

            List<string> pricePerQuantityList = new List<string>();
            #endregion

            #region Parse each product tile data and get the relative price for each item
            foreach (var product in productProperties)
            {
                // Get the span element list items from the price__calculation
                // nodes, if it is empty assume it is an empty string
                currProd = product.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("price__calculation_method"))
                    .FirstOrDefault()?
                    .InnerText;

                if (currProd is null)
                {
                    pricePerQuantityList.Add("");
                }
                else
                {
                    pricePerQuantityList.Add(currProd);
                }
            }

            return pricePerQuantityList;
            #endregion
        }
        private List<string> GetProductAvailability(List<HtmlNode> htmlData)
        {
            var productProperties = htmlData;

            List<string> availablity = new List<string>();

            foreach (var product in productProperties)
            {
                // Get the span element list items from the price__calculation
                // nodes, if it is empty assume it is an empty string
                var currProd = product.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("product__unavailable"))
                    .FirstOrDefault();

                if (currProd is null)
                {
                    availablity.Add("Visible");
                }
                else
                {
                    availablity.Add("Hidden");
                }

            }
            return availablity;
        }

        /// <summary>
        ///     Checks whether a search has occured or a category has been selected
        ///     and then accordingly gets the base url (the first page that appears)
        ///     of either the search or the category selection.
        /// </summary>
        /// <param name="searchText">The search implemented from the search bar</param>
        /// <param name="category">The category selected from the sidebar</param>
        /// <returns>Gets the base url (the first page that appears)</returns>
        /// <exception cref="Exception">If neither a category or a search is made then 
        /// an exception is called</exception>
        private string GetBaseUrl(string? searchText = null, ColesCategories category = ColesCategories.None)
        {
            #region Return the url of the first page of the category selected
            if (searchText is null && category != ColesCategories.None)
            {
                return category.GetDescription();
            }
            #endregion
            #region Return the url for the search made
            else if (searchText is not null && category == ColesCategories.None)
            {
                List<string> splitText = searchText.Split(' ').ToList();
                string url = "https://www.coles.com.au/search?q=";
                url += $"{splitText.First()}";
                splitText.RemoveAt(0);
                splitText.ForEach(text =>
                {
                    url += "%20" + text;
                });
                return url;
            }
            #endregion
            #region Throw an error if neither a category is selected or a search is made
            else
            {
                throw new Exception("Select a category or complete a search");
            }
            #endregion
        }


        /// <summary>
        ///     Uses the given url page from a category
        ///     to get pages from the navigation bar to select to
        /// </summary>
        /// <param name="url">Website Address to a page in a category</param>
        /// <param name="finalPg">The final page number in the navigation bar</param>
        /// <returns>The list of pages from the navigation bar of the given category</returns>
        public List<string> CategoryUrls(string url, int finalPg)
        {
            #region Use the number of pages and create the url to navigate to each page
            List<string> urlList = new List<string>();
            urlList.Add(url);

            for (int i = 1; i < finalPg; i++)
            {
                urlList.Add($"{url}?page={i + 1}");
            }
            return urlList;
            #endregion
        }


        /// <summary>
        ///     Uses the given url page from a search
        ///     to get the pages from the navigation bar to select to
        /// </summary>
        /// <param name="url">Website address to a page gotten from a search</param>
        /// <param name="finalPg">The final page number in the navigation bar</param>
        /// <returns>The list of pages from the navigation bar of the given category</returns>
        public List<string> SearchUrls(string url, int finalPg)
        {
            #region Use the number of pages and create the url to navigate to each page
            List<string> urlList = new List<string>();
            urlList.Add(url);

            for (int i = 1; i < finalPg; i++)
            {
                urlList.Add($"{url}&page={i + 1}");
            }
            return urlList;
            #endregion
        }

        /// <summary>
        /// Runs through all every product tile to get the bitmap image of each product
        /// </summary>
        /// <param name="htmlData">List of data from every product tile</param>
        /// <returns>List of Bitmap Images</returns>
        private List<BitmapImage> RunImages(List<HtmlNode> htmlData)
        {
            #region Get the url of each image on display for each item and add to a list of bitmap images

            List<BitmapImage> bitmapImages = new List<BitmapImage>();

            htmlData.ForEach(node =>
            {
                string imgUrl = node.Descendants("img")
                .Select(node => node.GetAttributeValue("src", "")).Last();

                var image = new BitmapImage(new Uri(imgUrl));
                bitmapImages.Add(image);
            });
            return bitmapImages;

            #endregion
        }

        /// <summary>
        ///     Parses all the data into from each tile into its own cluster and returns it for
        ///     use on the display
        /// </summary>
        /// <param name="getPage">Delegate method to get navigation pages of the category or search</param>
        /// <param name="urlList">Outputted url list for navigation to each page in the navigation bar</param>
        /// <param name="searchText">The search text from the search bar</param>
        /// <param name="category">The selected category from the flyview</param>
        /// <returns>List of all the product item information for each item</returns>
        public List<SupermarketModel> GetPageData(GetPageURL getPage, out List<string>? urlList,
            string? searchText = null, ColesCategories category = ColesCategories.None)
        {
            #region Get the basic url and then the product tile information from the page
            string url = GetBaseUrl(searchText, category);
            var htmlItems = GetHtmlNode(url, out int? finalPg);
            #endregion

            #region Returns the list of navigation pages or nothing if there are none
            urlList = finalPg is not null ? getPage(url, (int)finalPg) : null;
            #endregion

            #region Get the product tile information from the page and sort them into a website data colleciton
            WebsiteDataCollection tasks = new WebsiteDataCollection()
            {
                pdNames = GetProductName(htmlItems),
                pdPrices = GetProductPrice(htmlItems),
                pdQuantity = GetProductQuantity(htmlItems),
                pdPricePerQuantity = GetPricePerQuantity(htmlItems),
                bitmapImages = RunImages(htmlItems),
                productAvailabilities = GetProductAvailability(htmlItems)
            };

            #endregion

            #region Sort data into supermarket model list corresponding to each product tile item on the page
            List<SupermarketModel> smData = new List<SupermarketModel>(tasks.pdNames.Count);
            for (int i = 0; i < smData.Capacity; i++)
            {
                SupermarketModel smModel = new SupermarketModel
                {
                    ProductName = tasks.pdNames[i],
                    Price = tasks.pdPrices[i],
                    Quantity = tasks.pdQuantity[i],
                    PricePerQuantity = tasks.pdPricePerQuantity[i],
                    Image = tasks.bitmapImages[i],
                    AvailabilityVisibility = tasks.productAvailabilities[i]
                };
                smData.Add(smModel);
            }
            return smData;
            #endregion 
        }

        /// <summary>
        /// Gets basic html information for a website
        /// </summary>
        /// <param name="url">Website address</param>
        /// <returns>List of Product Item data</returns>
        public List<SupermarketModel> GetPageData(string url)
        {
            #region Get the basic url and then the product tile information from the page
            var htmlItems = GetHtmlNode(url, out int? finalPg);
            #endregion

            #region Get the product tile information from the page and sort them into a website data colleciton
            WebsiteDataCollection tasks = new WebsiteDataCollection()
            {
                pdNames = GetProductName(htmlItems),
                pdPrices = GetProductPrice(htmlItems),
                pdQuantity = GetProductQuantity(htmlItems),
                pdPricePerQuantity = GetPricePerQuantity(htmlItems),
                bitmapImages = RunImages(htmlItems),
                productAvailabilities = GetProductAvailability(htmlItems)
            };
            #endregion

            #region Sort data into supermarket model list corresponding to each product tile item on the page
            List<SupermarketModel> smData = new List<SupermarketModel>(tasks.pdNames.Count);
            for (int i = 0; i < smData.Capacity; i++)
            {
                SupermarketModel smModel = new SupermarketModel
                {
                    ProductName = tasks.pdNames[i],
                    Price = tasks.pdPrices[i],
                    Quantity = tasks.pdQuantity[i],
                    PricePerQuantity = tasks.pdPricePerQuantity[i],
                    Image = tasks.bitmapImages[i],
                    AvailabilityVisibility = tasks.productAvailabilities[i]
                };
                smData.Add(smModel);
            }
            return smData;
            #endregion 
        }

    }
}
