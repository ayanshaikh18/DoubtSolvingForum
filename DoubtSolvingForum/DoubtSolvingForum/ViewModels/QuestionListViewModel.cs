using DoubtSolvingForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.ViewModels
{
    public class QuestionListViewModel
    {
        public IEnumerable<Question> Questions { get; set; }
    }
}
