using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessage = "Passwords Don't match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string email { get; set; }
        public string token { get; set; }
    }
}
