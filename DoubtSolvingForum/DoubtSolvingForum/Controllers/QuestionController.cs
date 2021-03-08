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
        private readonly UserManager<IdentityUser> userManager;
        public QuestionController(IQuestionRepository questionRepository,
                                    UserManager<IdentityUser> userManager)
        {
            this.questionRepository = questionRepository;
            this.userManager = userManager;
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

        public IActionResult List()
        {
            var model = new QuestionListViewModel();
            model.Questions = questionRepository.GetQuestions();
            return View(model);
        }

        public async Task<IActionResult> View(int id)
        {
            var question = questionRepository.GetQuestion(id);
            question.User = await userManager.FindByIdAsync(question.UserId);
            return View(question);
        }
    }
}
