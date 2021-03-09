using DoubtSolvingForum.Models;
using DoubtSolvingForum.Models.Repositories;
using DoubtSolvingForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IAnswerRepository answerRepository;
        private readonly UserManager<IdentityUser> userManager;
        public QuestionController(IQuestionRepository questionRepository,
                                    UserManager<IdentityUser> userManager,
                                    IAnswerRepository answerRepository)
        {
            this.questionRepository = questionRepository;
            this.userManager = userManager;
            this.answerRepository = answerRepository;
        }
        public IActionResult Index()
        {
            return RedirectToAction("list","question");
        }

        [HttpGet]
        public IActionResult NewQuestion()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> NewQuestion(Question model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(HttpContext.User);
                model.User = user;
                questionRepository.Add(model);
                return RedirectToAction("list", "question");
            }
            return View(model);
        }

        public async Task<IActionResult> List(string searchString,bool myQuestions)
        {
            var model = new QuestionListViewModel();
            var fetchedQuestions = questionRepository.GetQuestions();
            if (searchString != null)
            {
                fetchedQuestions = fetchedQuestions.Where(q => q.Title.Contains(searchString));
                ViewBag.searchString = searchString;
            }
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (myQuestions)
            {
                fetchedQuestions = fetchedQuestions.Where(q => q.UserId == user.Id);
                ViewBag.myQuestions = true;
            }
            var questions = new List<Question>();
            foreach(var ques in fetchedQuestions)
            {
                var question = new Question()
                {
                    Id = ques.Id,
                    Title = ques.Title,
                    QuestionText = ques.QuestionText,
                    UserId = ques.UserId,
                    User = await userManager.FindByIdAsync(ques.UserId),
                    Answers = ques.Answers
                };
                questions.Add(question);
            }
            model.Questions = questions;
            return View(model);
        }

        public async Task<IActionResult> View(int id)
        {
            var question = questionRepository.GetQuestion(id);
            question.User = await userManager.FindByIdAsync(question.UserId);
            var model = new ViewQuestionViewModel();
            model.Question = question;
            var fetchedAnswers = answerRepository.GetAnswers(id);
            var answers = new List<Answer>();
            foreach(var ans in fetchedAnswers)
            {
                var answer = new Answer()
                {
                    AnswerText = ans.AnswerText,
                    UserId = ans.UserId,
                    QuestionId = ans.QuestionId,
                };
                answer.User = await userManager.FindByIdAsync(ans.UserId);
                answers.Add(answer);
            }
            model.Answers = answers;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostAnswer(ViewQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var answer = new Answer()
                {
                    AnswerText = model.MyAnswer
                };
                answer.QuestionId = model.QuestionId;
                answer.User = await userManager.GetUserAsync(HttpContext.User);
                answerRepository.Add(answer);
                return RedirectToAction("view", "question",new { id=model.QuestionId });
            }
            return View(model);
        }
    }
}
