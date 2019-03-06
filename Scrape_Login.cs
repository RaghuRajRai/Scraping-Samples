using System;
using System.Linq;
using AngleSharp;
using AngleSharp.Html.Parser;
using AngleSharp.Dom;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Scrape
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Getting the html of the page
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var doc = await context.OpenAsync("https://yts.am/");
            //Console.Write(document.ToHtml());
            var source = doc.ToHtml();
            // Create a new parser front-end (can be re-used)
            var parser = new HtmlParser();
            //Just get the DOM representation
            var document = parser.ParseDocument(source);

            //Do something with LINQ : using LINQ, we can make queries like we would in SQL 
            var ListItems = document.All.Where(m => m.LocalName == "a" && m.ClassList.Contains("browse-movie-title"));

            foreach (var item in ListItems)
                Console.WriteLine(item.Text());




            Console.ReadLine();
        }
        
     
    }
}
