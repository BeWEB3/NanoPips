﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exchange.EF
{
    using Exchange.DTO;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ExchangeEntities : DbContext
    {
        public ExchangeEntities()
            : base("name=ExchangeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<AddressBook> AddressBooks { get; set; }
        public virtual DbSet<AddressStat> AddressStats { get; set; }
        public virtual DbSet<AdminSetting> AdminSettings { get; set; }
        public virtual DbSet<CandlesData> CandlesDatas { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<EmailVerification> EmailVerifications { get; set; }
        public virtual DbSet<FunctionAccess> FunctionAccesses { get; set; }
        public virtual DbSet<FunctionRequest> FunctionRequests { get; set; }
        public virtual DbSet<KYC> KYCs { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Pair> Pairs { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentStatu> PaymentStatus { get; set; }
        public virtual DbSet<SecurityAnswer> SecurityAnswers { get; set; }
        public virtual DbSet<Trade> Trades { get; set; }
        public virtual DbSet<UserActivity> UserActivities { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserRoleType> UserRoleTypes { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
    }
}
