using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models.Repositories
{
    public interface IAnswerRepository
    {
        IEnumerable<Answer> GetAnswers();
        Answer Add(Answer answer);
        IEnumerable<Answer> GetAnswers(int qid);
        Answer GetAnswer(int id);
        void DeleteAnswer(int id);
        Answer Update(Answer answer);
        Vote VoteAnswer(Vote vote);
        void DeleteVoteByUserId(string userId);
        IEnumerable<Vote> GetVotes();
    }
}
