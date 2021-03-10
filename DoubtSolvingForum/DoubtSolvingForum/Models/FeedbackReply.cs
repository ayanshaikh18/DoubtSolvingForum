using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models
{
    public class FeedbackReply
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public Feedback Feedback { get; set; }
        public string Reply { get; set; }
    }
}
