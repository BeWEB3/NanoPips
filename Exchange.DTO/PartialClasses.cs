using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.DTO
{
    [MetadataType(typeof(UserRoleMetadata))]
    public partial class UserRole
    { 
        public string ConfirmPassword { get; set; }
    }
    [MetadataType(typeof(AccountMetadata))]
    public partial class Account
    {
      
    }
}
