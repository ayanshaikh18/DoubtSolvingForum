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

        public IEnumerable<Answer> GetAnswers(int qid)
        {
            return context.Answers.Where(a => a.QuestionId == qid).ToList();
        }
    }
}
