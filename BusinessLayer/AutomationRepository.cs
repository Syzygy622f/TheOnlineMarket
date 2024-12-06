using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Models;

namespace BusinessLayer
{
    public class AutomationRepository
    {
        

        public async Task<ProductDetails> RunAutomation(string Url)
        {
            HttpClient httpClient = new();

            string html = await httpClient.GetStringAsync(Url);
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(html);

            //string titleXPath;
            //string priceXPath;
            //string descriptionXPath;

            string title = "det er er en titel";
            string price = "1000";
            string description = "en beskrivelse";

            //vil have brugt det er til at web scrape fra dba og facebook, jeg har ikke sat det præcis til dem, så det ikke kan blive misbrugt
            //if (Url.Contains("dba.dk")) kan ikke bruges det deres bruger vilkår siger
            //benytte nogen former for robot, søgerobotter, scrapers eller andre automatiske midler til at få adgang til dba.dk og indsamle indhold til noget formål uden vores udtrykkelige tilladelse;
            //{
            //    titleXPath = "//h1[@class='product-title']";
            //    priceXPath = "//span[@class='product-price']";
            //    descriptionXPath = "//div[@class='product-description']";

            //    title = htmlDocument.DocumentNode.SelectSingleNode(titleXPath)?.InnerText.Trim() ?? "Title not found";
            //    price = htmlDocument.DocumentNode.SelectSingleNode(priceXPath)?.InnerText.Trim() ?? "Price not found";
            //    description = htmlDocument.DocumentNode.SelectSingleNode(descriptionXPath)?.InnerText.Trim() ?? "Description not found";
            //}
            //if (Url.Contains("facebook.com")) samme her
            //{
            //     titleXPath = "//h1[@class='product-title']";
            //     priceXPath = "//span[@class='product-price']";
            //     descriptionXPath = "//div[@class='product-description']";

            //     title = htmlDocument.DocumentNode.SelectSingleNode(titleXPath)?.InnerText.Trim() ?? "Title not found";
            //     price = htmlDocument.DocumentNode.SelectSingleNode(priceXPath)?.InnerText.Trim() ?? "Price not found";
            //     description = htmlDocument.DocumentNode.SelectSingleNode(descriptionXPath)?.InnerText.Trim() ?? "Description not found";
            //}

            return new ProductDetails
            {
                Title = title,
                Price = price,
                Description = description
            };


        }


    }
}
