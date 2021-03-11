
using DoubtSolvingForum.Models;
using DoubtSolvingForum.Models.Repositories;
using DoubtSolvingForum.Utilities;
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
                return RedirectToAction("list", "question", new { notification = "Question Posted Successfully!!!" });
            }
            return View(model);
        }

        public async Task<IActionResult> List(string searchString,bool myQuestions,int? pageNumber, string notification)
        {
            IList<Question> fetchedQuestions = questionRepository.GetQuestions();
            if (searchString != null)
            {
                fetchedQuestions = fetchedQuestions.Where(q => q.Title.Contains(searchString)).ToList();
                ViewBag.searchString = searchString;
            }
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (myQuestions)
            {
                fetchedQuestions = fetchedQuestions.Where(q => q.UserId == user.Id).ToList();
                ViewBag.myQuestions = true;
            }
            foreach(var ques in fetchedQuestions)
            {
                ques.User = await userManager.FindByIdAsync(ques.UserId);
            }
            if (notification != null)
            {
                ViewBag.notification = notification;
            }
            int pageSize = 5;
            return View(PaginatedList<Question>.Create(fetchedQuestions ,pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> View(int id, string notification)
        {
            var question = questionRepository.GetQuestion(id);
            question.User = await userManager.FindByIdAsync(question.UserId);
            var model = new ViewQuestionViewModel();
            model.Question = question;
            var answers = answerRepository.GetAnswers(id);
            foreach(var ans in answers)
            {
                ans.User = await userManager.FindByIdAsync(ans.UserId);
            }
            model.Answers = answers;
            var user = await userManager.GetUserAsync(HttpContext.User);
            model.UserId = user.Id;
            if (await userManager.IsInRoleAsync(user, "Admin"))
                model.IsAdmin = true;
            else
                model.IsAdmin = false;

            if (notification != null)
            {
                ViewBag.notification = notification;
            }
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
                return RedirectToAction("view", "question",new { id=model.QuestionId, notification = "Answer Posted Successfully!!!" });
            }
            return View(model);
        }

        public async Task<IActionResult> VoteAnswer(int ansId,bool? IsUpVoted,bool? isDownVoted)
        {
            var answer = answerRepository.GetAnswer(ansId);
            var vote = new Vote()
            {
                AnswerId = ansId,
                IsUpVoted = IsUpVoted ?? false,
                IsDownVoted = isDownVoted ?? false,
                User = await userManager.GetUserAsync(HttpContext.User)
            };
            answerRepository.VoteAnswer(vote);
            return RedirectToAction("view", "question", new { id = answer.QuestionId });
        }

        public async Task<IActionResult> DeleteVote(int qid)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            answerRepository.DeleteVoteByUserId(user.Id);
            return RedirectToAction("view", "question", new { id = qid });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var question = questionRepository.GetQuestion(id);
            if (question.UserId != user.Id)
                return View("AccessDenied","Account");
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Question question)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid && question.UserId == user.Id)
            {
                questionRepository.Update(question);
                return RedirectToAction("view", "question", new {id=question.Id, notification = "Question Updated Successfully!!!" });
            }
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var question = questionRepository.GetQuestion(id);
            if (question.UserId != user.Id)
                return View("AccessDenied","Account");
            questionRepository.Delete(id);
            return RedirectToAction("list", "question",new { notification = "Question Deleted Successfully!!!"});
        }

        [HttpGet]
        public async Task<IActionResult> EditAnswer(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var answer = answerRepository.GetAnswer(id);
            if (answer.UserId != user.Id)
                return View("AccessDenied","Account");
            answer.Question = questionRepository.GetQuestion(answer.QuestionId);
            return View("EditAnswer", answer);
        }

        [HttpPost]
        public async Task<IActionResult> EditAnswer(Answer answer)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid && answer.UserId == user.Id)
            {
                answerRepository.Update(answer);
                return RedirectToAction("view", "question", new { id = answer.QuestionId,notification="Answer Edited Successfully" });
            }
            return View(answer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var answer = answerRepository.GetAnswer(id);
            if (answer.UserId != user.Id)
                return View("AccessDenied","Account");
            answerRepository.DeleteAnswer(id);
            return RedirectToAction("view", "question", new { id = answer.QuestionId, notification = "Answer Deleted Successfully!!!" });
        }
    }
}
