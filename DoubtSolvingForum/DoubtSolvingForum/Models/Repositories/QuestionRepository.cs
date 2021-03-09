using DoubtSolvingForum.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext context;
        public QuestionRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Question Add(Question question)
        {
            context.Questions.Add(question);
            context.SaveChanges();
            return question;
        }

        public Question Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Question GetQuestion(int id)
        {
            return context.Questions.Where(q => q.Id == id).FirstOrDefault();
        }

        public IEnumerable<Question> GetQuestions()
        {
            var questions = context.Questions.Select(q => new Question
            {
                QuestionText = q.QuestionText,
                Title = q.Title,
                Id = q.Id,
                UserId = q.UserId,
                Answers = context.Answers.Where(a => a.QuestionId == q.Id).ToList()
            });
            return questions.ToList();
        }

        public Question Update(Question question)
        {
            throw new NotImplementedException();
        }
    }
}
