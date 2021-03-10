using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AppDbContext context;
        public FeedbackRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Feedback Add(Feedback feedback)
        {
            context.Feedbacks.Add(feedback);
            context.SaveChanges();
            return feedback;
        }

        public IEnumerable<Feedback> GetFeedbacks()
        {
            var feedbacks = context.Feedbacks.Select(f => new Feedback
            {
                Id = f.Id,
                Text = f.Text,
                UserId = f.UserId,
                FeedbackReplies = context.FeedbackReplies.Where(reply=>reply.FeedbackId==f.Id).ToList()
            });
            return feedbacks.ToList();
        }

        public FeedbackReply PostReply(FeedbackReply feedbackReply)
        {
            context.FeedbackReplies.Add(feedbackReply);
            context.SaveChanges();
            return feedbackReply;
        }
    }
}
