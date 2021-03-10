using DoubtSolvingForum.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.ViewModels
{
    public class FeedbackListViewModel
    {
        public string FeedbackText { get; set; }
        public IEnumerable<Feedback> Feedbacks { get; set; }
        public bool IsAdmin { get; set; }
    }
}
