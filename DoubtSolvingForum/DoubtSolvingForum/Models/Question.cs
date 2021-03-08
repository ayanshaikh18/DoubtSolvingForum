using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Models
{
    public class Question
    {
        [Required]
        public string Title { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        [Required]
        public string QuestionText { get; set; }
    }
}
