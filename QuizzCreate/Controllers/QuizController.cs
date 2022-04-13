using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizzCreate.Data;
using QuizzCreate.Models.Identity;
using QuizzCreate.Models.Quiz;
using QuizzCreate.ViewModels;
using QuizzCreate.ViewModels.QuizViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuizzCreate.Controllers
{
    public class QuizController : Controller
    {
        static List<RecentlyPublishedViewModel> recentlyPublisheds = new List<RecentlyPublishedViewModel>();
        private readonly Data.DbContext _dbContext;

        public QuizController(Data.DbContext dbContext)
        {
            _dbContext = dbContext;
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

        [HttpPost]
        public IActionResult CreateQuiz(QuizViewModel model)
        {
            if (ModelState.IsValid)
            {
                Quiz quiz = new Quiz();
                quiz.Title = model.Title;
                quiz.Paragraphs = model.Paragraphs;
                _dbContext.Add(quiz);
                foreach (QuestionViewModel questionViewModel in model.Questions)
                {
                    Question question = new Question();
                    question.QuizId = quiz.Id;
                    question.QuestionContent = questionViewModel.QuestionContent;
                    question.CorrectAnswer = questionViewModel.CorrectAnswer;
                    _dbContext.Add(question);
                    foreach (OptionViewModel optionViewModel in questionViewModel.Options)
                    {
                        Option option = new Option();
                        option.QuestionId = question.Id;
                        option.Character = optionViewModel.Character;
                        option.OptionContent = optionViewModel.OptionContent;
                        _dbContext.Add(option);
                    }
                    quiz.Questions.Add(question);
                }
                _dbContext.SaveChanges();
            }
            else
            {
                return BadRequest();
            }
            return RedirectToAction("GetQuizzes", "Quiz"); 
        }

        public IActionResult GetQuizzes()
        {

            List<Quiz> quizzes = _dbContext.Quizzes
                            .Include(x => x.Questions)
                            .ThenInclude(x => x.Options).ToList();

            return View(quizzes);
        }

        public IActionResult GetParagraph(string textTitle)
        {
            string text = recentlyPublisheds.Find(x => x.Title == textTitle).Paragraphs;
            
            text += "<input type=\"hidden\" value=\"" + text + "\" id=\"Paragraphs\" name=\"Paragraphs\">"; // post islemi icin

            return Json(text);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Quiz quiz = _dbContext.Quizzes.Find(id);
            if (quiz == null)
            {
                return BadRequest();
            }
            else
            {
                _dbContext.Remove(quiz);
                _dbContext.SaveChanges();
                return RedirectToAction("GetQuizzes");
            }

        }
        [HttpGet]
        public IActionResult EnterQuiz(Guid id)
        {
            Quiz quiz = _dbContext.Quizzes
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .FirstOrDefault(x => x.Id == id);
            if (quiz == null)
            {
                return BadRequest();
            }
            else
            {
                return View(quiz);
            }
        }

        [HttpPost]
        public JsonResult CheckQuestionAnswers(List<QuizAnswerViewModel> request)
        {
            var response = new List<QuizAnswerViewModel>();
            foreach (var questionAnswer in request)
            {
                QuizAnswerViewModel quiz = new QuizAnswerViewModel();
                var question = _dbContext.Questions.FirstOrDefault(x => x.Id == questionAnswer.QuestionId);
                var option = _dbContext.Options.FirstOrDefault(x => x.QuestionId == question.Id && x.Character == question.CorrectAnswer);
                if (questionAnswer.OptionId == option.Id)
                {
                    quiz.isCorrect = true;
                }
                else
                {
                    quiz.isCorrect = false;
                }
                quiz.OptionId = questionAnswer.OptionId;
                quiz.QuestionId = questionAnswer.QuestionId;
                response.Add(quiz);
            }

            return Json(response);
        }
    }
}
