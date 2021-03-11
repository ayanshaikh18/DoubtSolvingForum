using DoubtSolvingForum.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<IdentityUser> Users { get; set; }

        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public IEnumerable<Vote> Votes { get; set; }
    }
}
