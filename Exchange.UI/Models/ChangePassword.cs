
using System.ComponentModel.DataAnnotations;

namespace Exchange.UI.Models
{
    public class ChangePassword
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be atleast 8 character long")]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password must contains an upper letter, lower letter, a number and a symbol")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }
}