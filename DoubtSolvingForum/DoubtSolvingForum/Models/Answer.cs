using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string AnswerText { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public IEnumerable<Vote> Votes { get; set; }
    }
}
