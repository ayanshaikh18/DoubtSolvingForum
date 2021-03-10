using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext context;
        public AnswerRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Answer Add(Answer answer)
        {
            context.Answers.Add(answer);
            context.SaveChanges();
            return answer;
        }

        public void DeleteAnswer(int id)
        {
            var ans = context.Answers.Find(id);
            if (ans != null)
            {
                context.Answers.Remove(ans);
                context.SaveChanges();
            }
        }

        public void DeleteVoteByUserId(string userId)
        {
            var vote = context.Votes.Where(v=>v.UserId==userId).FirstOrDefault();
            if (vote != null)
            {
                context.Votes.Remove(vote);
                context.SaveChanges();
            }
        }

        public Answer GetAnswer(int id)
        {
            return context.Answers.Where(ans => ans.Id == id).FirstOrDefault();
        }

        public IEnumerable<Answer> GetAnswers(int qid)
        {
            var answers = context.Answers.Where(a => a.QuestionId == qid).Select(a => new Answer()
            {
                Id = a.Id,
                AnswerText = a.AnswerText,
                QuestionId = a.QuestionId,
                UserId = a.UserId,
                Votes = context.Votes.Where(vote=>vote.AnswerId==a.Id).ToList()
            });
            return answers.ToList();
        }

        public Answer Update(Answer answer)
        {
            var ans = context.Answers.Attach(answer);
            ans.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return answer;
        }

        public Vote VoteAnswer(Vote vote)
        {
            context.Votes.Add(vote);
            context.SaveChanges();
            return vote;
        }
    }
}
