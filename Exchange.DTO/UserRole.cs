//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exchange.DTO
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserRole
    {
        public long UserRoleId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public Nullable<System.DateTime> HashExpirationDate { get; set; }
        public Nullable<long> Account_Id { get; set; }
        public Nullable<byte> UserRoleTypeId { get; set; }
        public string RefferenceNumber { get; set; }
        public string Name { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual UserRoleType UserRoleType { get; set; }
    }
}
