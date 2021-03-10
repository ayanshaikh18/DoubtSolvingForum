using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models.Repositories
{
    public interface IFeedbackRepository
    {
        Feedback Add(Feedback feedback);
        IEnumerable<Feedback> GetFeedbacks();
        FeedbackReply PostReply(FeedbackReply feedbackReply);
    }
}
