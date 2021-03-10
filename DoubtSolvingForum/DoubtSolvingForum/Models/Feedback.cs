using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public string Text { get; set; }

        public IEnumerable<FeedbackReply> FeedbackReplies { get; set; }
    }
}
