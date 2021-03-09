using DoubtSolvingForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.ViewModels
{
    public class ViewQuestionViewModel
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public string MyAnswer { get; set; }
    }
}
