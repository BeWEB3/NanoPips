using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoinEXR.Admin.Models
{
    public class ERCTokenModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Short Name")]
        public string ThreeDigitName { get; set; }
        [Required]
        public string SmartContractAddress { get; set; }
        [Required]
        [Display(Name = "Price In Ethereum")]
        public float PriceETH { get; set; }
        [Required]
        public string Type { get; set; }
    }
}