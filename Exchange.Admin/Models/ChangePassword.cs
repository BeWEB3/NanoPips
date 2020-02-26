using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exchange.Admin.Models
{
    public class ChangePassword
    {
        [Required]
        public string OldPassword { get; set; }
        [MinLength(8)]
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }
}