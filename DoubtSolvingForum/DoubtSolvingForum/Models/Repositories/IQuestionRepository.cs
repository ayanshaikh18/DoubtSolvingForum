using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models.Repositories
{
    public interface IQuestionRepository
    {
        Question Add(Question question);
        Question GetQuestion(int id);
        IEnumerable<Question> GetQuestions();
        Question Update(Question question);
        Question Delete(int id);
    }
}
