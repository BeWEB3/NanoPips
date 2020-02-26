using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.DTO
{
    public class UserRoleMetadata
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be atleast 8 character long")]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password must contains an upper letter, lower letter, a number and a symbol")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
    public class AccountMetadata
    {
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string ZipCode { get; set; }

    }
}
