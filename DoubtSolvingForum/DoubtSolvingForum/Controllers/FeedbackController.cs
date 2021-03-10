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
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepository feedbackRepository;
        private readonly UserManager<IdentityUser> userManager;
        public FeedbackController(IFeedbackRepository feedbackRepository,
                                    UserManager<IdentityUser> userManager)
        {
            this.feedbackRepository = feedbackRepository;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return RedirectToAction("list", "feedback");
        }

        [HttpPost]
        public async Task<IActionResult> PostFeedback(FeedbackListViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Feedback = new Feedback()
                {
                    Text = model.FeedbackText,
                    User = await userManager.GetUserAsync(HttpContext.User)
                };
                feedbackRepository.Add(Feedback);
                return RedirectToAction("list", "feedback");
            }
            return View(model);
        }

        public async Task<IActionResult> List()
        {
            FeedbackListViewModel model = new FeedbackListViewModel();
            var fetchedFeedbacks = feedbackRepository.GetFeedbacks();
            var feedbacks = new List<Feedback>();
            foreach(var fb in fetchedFeedbacks)
            {
                fb.User = await userManager.FindByIdAsync(fb.UserId);
            }
            model.Feedbacks = fetchedFeedbacks;
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (await userManager.IsInRoleAsync(user,"Admin"))
                model.IsAdmin = true;
            else
                model.IsAdmin = false;

            return View(model);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult PostReply(int fid,string feedbackText)
        {
            if (ModelState.IsValid) 
            {
                var reply = new FeedbackReply()
                {
                    FeedbackId = fid,
                    Reply = feedbackText
                };
                feedbackRepository.PostReply(reply);
            }
            return RedirectToAction("list", "feedback");
        }
    }
}
