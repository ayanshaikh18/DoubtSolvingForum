using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }
        public bool IsUpVoted { get; set; }
        public bool IsDownVoted { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
