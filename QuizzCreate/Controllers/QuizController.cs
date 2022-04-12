using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using QuizzCreate.Data;
using QuizzCreate.ViewModels;
using System.Collections.Generic;
using System.Net;

namespace QuizzCreate.Controllers
{
    public class QuizController : Controller
    {
        static List<RecentlyPublishedViewModel> recentlyPublisheds = new List<RecentlyPublishedViewModel>();
        private readonly DbContext _dbContext;

        public QuizController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateQuiz()
        {
            string urlHome = "https://www.wired.com";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(urlHome);

            recentlyPublisheds.Clear();

            for (int i = 1; i <= 5; i++)
            {
                string paragraphs = "";

                HtmlNode titleNode = doc.DocumentNode.SelectSingleNode("//*[@id='main-content']/div[1]/div[1]/section/div[3]/div/div/div/div/div[" + i + "]/div[2]/a/h2");
                if (titleNode != null)
                {
                    string title = WebUtility.HtmlDecode(titleNode.InnerText); ;
                    // Get Content
                    HtmlNode contentLinkNode = doc.DocumentNode.SelectSingleNode("//*[@id='main-content']/div[1]/div[1]/section/div[3]/div/div/div/div/div[" + i + "]/div[2]/a[@href]");
                    string contentHrefValue = contentLinkNode.GetAttributeValue("href", string.Empty);
                    string contentUrl = urlHome + contentHrefValue;
                    HtmlDocument contentDoc = web.Load(contentUrl);
                    HtmlNodeCollection contentNodes = contentDoc.DocumentNode.SelectNodes("//div[@class='body__inner-container'][1]/p");
                    if (contentNodes != null)
                    {
                        foreach (HtmlNode contentNode in contentNodes)
                        {
                            string content = contentNode.InnerText;
                            string decodedstring = WebUtility.HtmlDecode(content); // Fix the special characters
                            string text = "<p>" + decodedstring + "</p>";
                            paragraphs += string.Concat(text);
                        }
                        recentlyPublisheds.Add(new RecentlyPublishedViewModel
                        {
                            Title = title,
                            Paragraphs = paragraphs
                        });
                    }
                }
            }


            ViewBag.RecentlyPublisheds = recentlyPublisheds;
            return View();
        }

        public IActionResult GetParagraph(string textTitle)
        {
            string text = recentlyPublisheds.Find(x => x.Title == textTitle).Paragraphs;
            
            text += "<input type=\"hidden\" value=\"" + text + "\" id=\"Paragraphs\" name=\"Paragraphs\">"; // post islemi icin

            return Json(text);
        }
    }
}
