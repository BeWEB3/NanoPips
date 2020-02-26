using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoinEXR.Admin.Models
{
    public class AdminAccountModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be atleast 8 character long")]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password must contains an upper letter, lower letter, a number and a symbol")]
        public string Password { get; set; }
        [Required]
        public string AccountType { get; set; }
        public int AccountId { get; set; }
    }
}