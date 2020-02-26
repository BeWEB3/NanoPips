using Exchange.Common;
using Exchange.DTO;
using Exchange.EF;
using Exchange.Services.Interface;
using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Exchange.Services.Implementation
{
    internal class AccountsService : IAccountService
    {
        private ExchangeEntities _db;

        public AccountsService(ExchangeEntities db)
        {
            _db = db;
        }

        public bool ResetTwoFA(long acId)
        {
            _db.Accounts.Where(m => m.AccountId == acId).First().TwoFactorEnabled = false;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return true;
        }

        public RefferalData GetRefferalData(long accId) {
            var acc = _db.Accounts.Where(a => a.AccountId == accId).SingleOrDefault();
            if (acc == null)
            {
                return null;
            }
            else {
                decimal totalEarn = 0;
                RefferalData refferalData  = new RefferalData();
                refferalData.refferalUsers = new List<userList>();
                refferalData.rankList      = new List<rank>(); 
                var refferalCount = _db.Accounts.Where(r => r.Referral_AccountId == acc.AccountId).ToList();
                refferalData.refferalCount = refferalCount.Count();
                refferalData.referralCode  = acc.RefferenceNumber;
                for (int x = 0; x < refferalCount.Count(); x++) {
                    var id = refferalCount[x].AccountId;
                    var dbWallet =  _db.Wallets.Where(m => m.Account_Id == id && m.Currency == "USD").ToList();
                    if (dbWallet.Count() == 0)
                    {
                        refferalData.refferalUsers.Add(new userList()
                        {
                            email  = refferalCount[x].Email,
                            amount = 0,
                        });
                    }
                    else if (dbWallet.FirstOrDefault().Balance <= 10)
                    {
                        var tradeList = _db.Trades.Where(t => t.Account_Id == id && t.Status == "COMPLETED").ToList();
                        decimal total = 0;
                        foreach (var trade in tradeList)
                        {
                            decimal perTradeProfit = 0;

                            if (trade.Direction == "BUY")
                            {
                                if (trade.PnL >= 0)
                                {
                                    perTradeProfit = (trade.PnL.Value);
                                }
                                else
                                {
                                    perTradeProfit = (trade.PnL.Value);
                                }
                            }
                            else if (trade.Direction == "SELL")
                            {
                                if (trade.PnL >= 0)
                                {
                                    perTradeProfit = (Math.Abs(trade.PnL.Value) * (-1));
                                }
                                else
                                {
                                    perTradeProfit = (Math.Abs(trade.PnL.Value));
                                }
                            }
                            total += (perTradeProfit - (trade.Amount.Value * trade.Value.Value));
                        }
                        totalEarn += Math.Round(((Math.Abs(total) * 15) / 100), 1);
                        refferalData.refferalUsers.Add(new userList()
                        {
                            email  = refferalCount[x].Email,
                            amount = Math.Round(((Math.Abs(total) * 15) / 100), 1),
                        });
                    }
                    else {
                        refferalData.refferalUsers.Add(new userList()
                        {
                            email  = refferalCount[x].Email,
                            amount = 0,
                        });
                    }
                }
                refferalData.earning  = Math.Abs(totalEarn);
                refferalData.dateTime = DateTime.UtcNow;
                refferalData.rankList.Add(new rank("br*****in@*****.com", 1, (decimal)18.89));
                refferalData.rankList.Add(new rank("za*******@*****.com", 2, (decimal)17.03));
                refferalData.rankList.Add(new rank("li********@*****.com", 3, (decimal)16.41));
                refferalData.rankList.Add(new rank("a.***********@*****.com", 4, (decimal)16.2));
                refferalData.rankList.Add(new rank("zk*******@*****.com", 5, (decimal)15.8));
                refferalData.rankList.Add(new rank("to*******@*****.com", 6, (decimal)15.2));
                refferalData.rankList.Add(new rank("ha*******@*****.com", 7, (decimal)14.9));
                refferalData.rankList.Add(new rank("da*******@*****.com", 8, (decimal)14.54));
                refferalData.rankList.Add(new rank("pa*******@*****.com", 9, (decimal)11.24));
                refferalData.rankList.Add(new rank("tt*******@*****.com", 10, (decimal)09.07));
                return refferalData;
            }
        }

        public bool CreateAdminAccount(string accountType, string firstName, string lastName, string email, string password)
        {
            if (_db.UserRoles.Where(m => m.Username == email).Any())
            {
                ExchangeException.Throw(ErrorCode.EMAIL_ALREADY_EXIST, null);
            }
            var account = new Account()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                AccountEnabled = true,
                TwoFactorEnabled = false,
                EmailVerificationStatus = true,
                AccountType_Id = (int) AccountTypes.ADMIN,
            };
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.Accounts.Add(account);
            _db.SaveChanges();
            _db.UserRoles.Add(new UserRole()
            {
                Password = password,
                //PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString() + ":" + password)),
                Username = email,
                Account_Id = account.AccountId,
                UserRoleTypeId = Convert.ToByte(accountType)
            });
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return true;
        }

        public bool SendEmail(string name, string email, string phoneNumber, string subject, string message) {
           return SMTPMailClient.SendRawEmail("clientservice@nanopips.com", message + "<br/>" + email + "<br/>" + phoneNumber + "<br/>", subject);
        }

        public bool EditAdminAccount(long AccountId, string accountType, string firstName, string lastName)
        {
            var ac = _db.Accounts.Find(AccountId);
            ac.FirstName = firstName;
            ac.LastName  = lastName;
            ac.UserRoles.First().UserRoleTypeId = Convert.ToByte(accountType);

            //var account = new Account()
            //{
            //    FirstName = firstName,
            //    LastName = lastName,
            //    Email = email,
            //    AccountEnabled = true,
            //    TwoFactorEnabled = false,
            //    EmailVerificationStatus = true,
            //    AccountType_Id = (int)AccountTypes.ADMIN,
            //};

            //_db.Configuration.ValidateOnSaveEnabled = false;

            //_db.Accounts.Add(account);
            //_db.SaveChanges();
            //_db.UserRoles.Add(new UserRole()
            //{
            //    Password = password,
            //    //PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString() + ":" + password)),
            //    Username = email,
            //    Account_Id = account.AccountId,
            //    UserRoleTypeId = Convert.ToByte(accountType)
            //});

            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return true;
        }

        public bool Suspend(long acId)
        {
            _db.Accounts.Find(acId).AccountEnabled = false;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return true;
        }

        public bool ChangePassword(long acId, string oldPassword, string newPassword)
        {
            var ac = _db.Accounts.Find(acId);
            //var oldPass = GenerateSha256Hash(oldPassword);
            if (ac.UserRoles.First().Password != oldPassword)
            {
                ExchangeException.Throw(ErrorCode.OLD_PASSWORD_IS_INVALID, null);
                return false;
            }
            //var newPass = GenerateSha256Hash(newPassword);
            ac.UserRoles.First().Password = newPassword;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return true;
        }

        public bool Activate(long acId)
        {
            _db.Accounts.Find(acId).AccountEnabled = true;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return true;
        }

        public bool Approve(long acId)
        {
            _db.Accounts.Find(acId).AccountType_Id = (int) AccountTypes.VERIFIED;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.Notifications.Add(new Notification() { Account_Id = acId, IsViewed = false, NotificationDetails = "Your account has been verified", NotificationHeading = "KYC", RedirectAction = "Index", RedirectController = "Dashboard" });
            _db.SaveChanges();
            return true;
        }

        public bool Reject(long acId, string reason)
        {
            var ac = _db.Accounts.Find(acId);
            ac.AccountType_Id = (int)AccountTypes.REJECTED;
            ac.RejectReason = reason;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.Notifications.Add(new Notification() { Account_Id = acId, IsViewed = false, NotificationDetails = "Your documents has been rejected", NotificationHeading = "KYC", RedirectAction = "KYC", RedirectController = "Dashboard" });
            _db.SaveChanges();
            return true;
        }

        public List<Notification> GetNotifications()
        {
            return _db.Notifications.Where(m => m.IsViewed != true && m.Account_Id == null).ToList();
        }

        public List<Account> GetAllUsers() {
            return _db.Accounts.ToList();
        }

        public List<Notification> GetDesktopNotifications()
        {
            _db.Configuration.ProxyCreationEnabled = false;
            return _db.Notifications.Where(m => m.IsViewed != true && m.NotificationType == "Desktop Notification").ToList();
        }

        public IQueryable<Account> Get()
        {
            return _db.Accounts;
        }

        public bool DisableAccount(string code)
        {
            var ac = _db.UserRoles.Where(m => m.PasswordHash == code).ToList();
            if (ac.Count == 0)
            {
                ExchangeException.Throw(ErrorCode.INVALID_CODE, null);
            }
            ac.First().Account.AccountEnabled = false;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return true;
        }

        public List<Notification> GetNotifications(long acId)
        {
            return _db.Notifications.Where(m => m.Account_Id == acId && m.IsViewed != true).ToList();
        }

        public Notification UpdateNotification(long notificationId)
        {
            var not = _db.Notifications.Find(notificationId);
            not.IsViewed = true;
            _db.SaveChanges();
            return not;
        }

        public IQueryable<Payment> GetTransectionHistory(long acId)
        {
            return _db.Payments.Where(m => m.Account_Id == acId);
        }

        public IQueryable<Wallet> GetAllWallets(long acId)
        {
            return _db.Wallets.Where(m => m.Account_Id == acId);
        }

        public List<Wallet> GetAllWallets(long acId, string from, string to)
        {
            List<Wallet> wallets = new List<Wallet>();
            var walletsfrom = _db.Wallets.Where(m => m.Account_Id == acId && m.Currency == from).ToList();
            if (walletsfrom.Count() > 0)
            {
                wallets.Add(walletsfrom.First());
            }
            else
            {
                wallets.Add(new Wallet() { Account_Id = acId, Balance = 0, Currency = from });
            }
            var walletsto = _db.Wallets.Where(m => m.Account_Id == acId && m.Currency == to).ToList();
            if (walletsto.Count() > 0)
            {
                wallets.Add(walletsto.First());
            }
            else
            {
                wallets.Add(new Wallet() { Account_Id = acId, Balance = 0, Currency = to });
            }
            return wallets;
        }

        public List<Wallet> GetWallets()
        {
            return _db.Wallets.Include("Account").ToList();
        }

        public string GenerateSha256Hash(string rawPass)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawPass));

                //Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public Account Login(string username, string password)
        {
            //var passwordHash = GenerateSha256Hash(password);
            var user = _db.UserRoles.Where(m => m.Username == username && m.Password == password).ToList();
            if (user.Count == 0)
            {
                ExchangeException.Throw(ErrorCode.INVALID_LOGIN, null);
            }
            if (user.First().Account.EmailVerificationStatus == false)  
            {
                SendVerificationEmail(user.First().Account_Id.Value);
                ExchangeException.Throw(ErrorCode.PLEASE_VERIFY_YOUR_EMAIL_ADDRESS_WE_HAVE_SENT_YOU_ANOTHER_EMAIL, null);
            }
            return user.First().Account;
        }

        public Account SignUp(string username, string password, string referralNumber)
        {
            if (_db.UserRoles.Where(m => m.Username == username).Any())
            {
                ExchangeException.Throw(ErrorCode.EMAIL_ALREADY_EXIST, null);
            }
            Nullable<long> id = 0;
            try {
                id = long.Parse(referralNumber.Trim().Remove(0, 3));
            }
            catch (Exception ex) {
                id = null;
            }

            var account = new Account()
            {
                Email              = username,
                AccountEnabled     = true,
                TwoFactorEnabled   = false,
                EmailVerificationStatus = false,
                AccountType_Id     = (int) AccountTypes.NEW,
                Referral_AccountId = id,
            };
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.Accounts.Add(account);
            _db.SaveChanges();
            _db.UserRoles.Add(new UserRole()
            {
                Password     = password,
                PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString() + ":" + password)),
                Username     = username,
                Account_Id   = account.AccountId,
            });
            //account.RefferenceNumber = get_Reference(account.AccountId);
            account.RefferenceNumber = username.Substring(0, 3) + account.AccountId;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return account;
        }

        private string get_Reference(long value)
        {
            char[] Alphabetic_Character_row1 = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            char[] Alphabetic_Character_row2 = { 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T' };
            char[] Alphabetic_Character_row3 = { 'U', 'V', 'W', 'X', 'Y', 'Z', 'A', 'K', 'O', 'I' };
            char[] referel = { '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' };
            try
            {
                string value_string = "";
                if (value == 0) { value_string = "000000"; }
                else if (value < 10) { value_string = "00000" + value; }
                else if (value < 100) { value_string = "0000" + value; }
                else if (value < 1000) { value_string = "000" + value; }
                else if (value < 10000) { value_string = "00" + value; }
                else if (value < 100000) { value_string = "0" + value; }
                else if (value >= 100000) { value_string = value.ToString(); }

                referel[0] = Alphabetic_Character_row1[Int32.Parse(value_string[0].ToString())];
                referel[1] = Alphabetic_Character_row2[Int32.Parse(value_string[1].ToString())];
                referel[2] = Alphabetic_Character_row3[Int32.Parse(value_string[2].ToString())];
                referel[3] = Alphabetic_Character_row1[Int32.Parse(value_string[3].ToString())];
                referel[4] = Alphabetic_Character_row2[Int32.Parse(value_string[4].ToString())];
                referel[5] = Alphabetic_Character_row3[Int32.Parse(value_string[5].ToString())];
                if (value > 999999)
                {
                    referel[6] = Alphabetic_Character_row1[Int32.Parse(value_string[6].ToString())];
                    if (value > 9999999) { referel[7] = Alphabetic_Character_row1[Int32.Parse(value_string[7].ToString())]; }
                    if (value > 99999999) { referel[8] = Alphabetic_Character_row2[Int32.Parse(value_string[8].ToString())]; }
                    if (value > 999999999) { referel[9] = Alphabetic_Character_row3[Int32.Parse(value_string[9].ToString())]; }
                    if (value > 9999999999) { referel[10] = Alphabetic_Character_row1[Int32.Parse(value_string[10].ToString())]; }
                    if (value > 99999999999) { referel[11] = Alphabetic_Character_row2[Int32.Parse(value_string[11].ToString())]; }
                    if (value > 999999999999) { referel[12] = Alphabetic_Character_row3[Int32.Parse(value_string[12].ToString())]; }
                    if (value > 9999999999999) { referel[13] = Alphabetic_Character_row1[Int32.Parse(value_string[13].ToString())]; }
                }
            }
            catch (Exception e) { e.GetBaseException(); }
            string y = "";
            for (int t = 0; t < referel.Length; t++)
            {
                if (referel[t] != '-')
                {
                    y += referel[t];
                }
            }
            return y;
        }

        public bool SendVerificationEmail(long acId, string email)
        {
            try
            {
                var ac = _db.Accounts.Find(acId);
                var verficationCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.Ticks + ":" + ac.Email));
                _db.EmailVerifications.Add(new EmailVerification()
                {
                    Account_Id = acId,
                    ExpirationDate = DateTime.Now.AddMinutes(60),
                    IsExpired = false,
                    VerificationCode = verficationCode
                });
                _db.SaveChanges();

                #region body
                string body = @"<div style=""width:500px;margin:auto;background-color:#fbfbfb"">
        <div style=""width:95%;margin:auto;height:60px;background-color:#66B85C"" >
            <div style=""font-family:verdana, calibri;font-size:95%;color:white;font-weight:bold;"">
                <p style=""padding:20px;text-align:center"">  Welcome to Nano Pips</p>
            </div>
        </div>
        <br />
        <div style=""width:95%;margin:auto"">
            <br /><br /><br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px;font-weight:bold "">
                Dear " + ac.Email + @",


               
            </div>
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;text-align:justify"">
            Thank you for signing up with Nano Pips. To secure your account, please verify your email address by clicking below link or copy and paste it into your browser to proceed with your registration.
            </div>
        
  
            <br />
            
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff;text-align:center;font-weight:bold"">
               <a style=""background-color:#66B85C;padding: 15px 32px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;"" class=""btn btn-lg"" href=""" + "" + Constants.Domain + @"/Account/Login?code=" + verficationCode + "&email=" + email + @""" target=""_blank"">Verify Email </a>
            </div>
            <br>
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff;text-align:center;font-weight:bold"">
        If you don't recognise NanoPips, please feel free to ignore this email.            
        </div>
            <br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff"">
               Best regards,<br />
               Nano Pips Team
            </div>
            <br />
            <br />
            <br />

            

            
        </div>


        <div style=""width:95%;margin:auto;height:50px;background-color:#66B85C"">

          

            <div style=""font-family:verdana, calibri; font-size:12.0px; color:rgb(0,0,1); text-align:center; background:rgb(255,255,255); text-transform:uppercase"">
                COPYRIGHT © " + DateTime.Now.Year + @" ALL RIGHTS ARE RESERVED BY Nano Pips
            </div>
        </div>
    </div>";
                #endregion

                new Thread(() => SMTPMailClient.SendEmail(body, "NanoPips, Verify your email address", new string[] { ac.Email })).Start();
            }
            catch (Exception ex)
            {
                ExchangeException.Throw(ErrorCode.OTHER_ERROR, ex);
            }
            return true;
        }

        public bool SendVerificationEmail(long acId)
        {
            try
            {
                var ac = _db.Accounts.Find(acId);
                var verficationCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.Ticks + ":" + ac.Email));
                _db.EmailVerifications.Add(new EmailVerification()
                {
                    Account_Id = acId,
                    ExpirationDate = DateTime.Now.AddMinutes(60),
                    IsExpired  = false,
                    VerificationCode = verficationCode
                });
                _db.SaveChanges();

                #region body
                string body = @"<div style=""width:500px;margin:auto;background-color:#fbfbfb"">
        <div style=""width:95%;margin:auto;height:60px;background-color:#66B85C"">
            <div style=""font-family:verdana, calibri;font-size:95%;color:white;font-weight:bold;"">
                <p style=""padding:20px;text-align:center"">  Welcome to Nano Pips</p>
            </div>
        </div>
        <br />
        <div style=""width:95%;margin:auto"">
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px;font-weight:bold;"">
                Dear " + ac.Email + @",
            </div>
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;text-align:justify"">
            Thank you for signing up with Nano Pips. To secure your account, please verify your email address by clicking below link button to proceed with your registration.
            </div>
            <br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff;text-align:center;font-weight:bold"">
               <a style=""background-color:#66B85C;color:#fff;padding: 15px 32px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;"" href=""" + "" + Constants.Domain + @"/Account/Login?code=" + verficationCode + @""" target=""_blank"">Verify Email</a>
            </div>
            <br>
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff;text-align:center;font-weight:bold"">
            If you don't recognise Nano Pips, please feel free to ignore this email.            
            </div>
            <br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff"">
               Best regards,<br />
               Nano Pips Team
            </div>
            <br />
            <br />
            <br />
        </div>
        <div style=""width:95%;margin:auto;height:50px;background-color:#66B85C"" >
            <div style=""font-family:verdana, calibri; font-size:12.0px; color:rgb(0,0,1); text-align:center; background:rgb(255,255,255); text-transform:uppercase"">
                COPYRIGHT © " + DateTime.Now.Year + @" ALL RIGHTS ARE RESERVED BY Nano Pips
            </div>
        </div>
    </div>";
                #endregion

                new Thread(() => SMTPMailClient.SendEmail(body, "NanoPips, Verify your email address", new string[] { ac.Email })).Start();

            }
            catch (Exception ex)
            {
                ExchangeException.Throw(ErrorCode.OTHER_ERROR, ex);
            }
            return true;
        }

        public bool CheckEmail(string email)
        {
            if (_db.UserRoles.Where(m => m.Username == email).Any())
            {
                return true;
            }
            else return false;
        }

        public bool VerifyEmail(string code)
        {
            var ev = _db.EmailVerifications.Where(m => m.VerificationCode == code && m.IsExpired != true).ToList();
            if (ev.Count == 0)
            {
                ExchangeException.Throw(ErrorCode.LINK_HAS_BEEN_EXPIRED, null);
            }
            if (DateTime.Now.CompareTo(ev.First().ExpirationDate.Value) > 0)
            {
                ExchangeException.Throw(ErrorCode.LINK_HAS_BEEN_EXPIRED, null);
            }
            ev.First().IsExpired = true;
            ev.First().Account.EmailVerificationStatus = true;
            ev.First().Account.EmailVerificationDate = DateTime.Now;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            SendWelcomeEmail(ev.First().Account.Email);
            return true;
        }

        public bool VerifyEmail(string code, string email)
        {
            var userAct = _db.UserActivities.Where(u => u.EmailAddress == email).OrderByDescending(u => u.ActivityId).FirstOrDefault();
            var ev = _db.EmailVerifications.Where(m => m.VerificationCode == code && m.IsExpired != true).ToList();
            if (ev.Count == 0)
            {
                ExchangeException.Throw(ErrorCode.LINK_HAS_BEEN_EXPIRED, null);
            }
            if (DateTime.Now.CompareTo(ev.First().ExpirationDate.Value) > 0)
            {
                ExchangeException.Throw(ErrorCode.LINK_HAS_BEEN_EXPIRED, null);
            }
            if(userAct.IsVerified != true)
            {
                userAct.IsVerified = true;
                ev.First().IsExpired = true;
                ev.First().Account.EmailVerificationStatus = true;
                ev.First().Account.EmailVerificationDate = DateTime.Now;
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
                return true;
            }
            return true;
        }

        public bool SendWelcomeEmail(string email)
        {
            #region body
            string body = @"<div style=""width:500px;margin:auto;background-color:#fbfbfb"">
         <div style=""width:95%;margin:auto;height:60px;background-color:#66B85C"">
            <div style=""font-family:verdana, calibri;font-size:95%;color:white;font-weight:bold;"">
                <p style=""padding:20px;text-align:center"">  Welcome to Nano Pips</p>
            </div>
        </div>
        <br />
        <div style=""width:95%;margin:auto"">
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px;font-weight:bold "">
                Dear " + email + @",
               
            </div>
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;text-align:justify"">
                Welcome to Nano Pips, your email address has been successfully verified.

            </div>

            <br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;text-align:justify"">
                Thanks for choosing  Nano Pips.
            </div>
         
         
            <br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff"">
               Best regards,<br />
                Nano Pips Team
            </div>
            <br />
            <br />
            <br />
            
        </div>


        <div style=""width:95%;margin:auto;height:50px;background-color:#66B85C"">

          

            <div style=""font-family:verdana, calibri; font-size:12.0px; color:rgb(0,0,1); text-align:center; background:rgb(255,255,255); text-transform:uppercase"">
                COPYRIGHT © " + DateTime.Now.Year + @" ALL RIGHTS RESERVED BY <br /> Nano Pips
            </div>
        </div>
    </div>";
            #endregion
            new Thread(() => SMTPMailClient.SendEmail(body, "Welcome to Nano Pips", new string[] { email })).Start();
            return true;
        }

        public bool SendForgotPasswordEmail(string email)
        {
            var user = _db.UserRoles.Where(m => m.Username == email).ToList();
            if (user.Count == 0)
            {
                ExchangeException.Throw(ErrorCode.EMAIL_DOES_NOT_EXIST, null);
            }
            user.First().PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString() + "|" + user.First().Account_Id + "|" + email));
            user.First().HashExpirationDate = DateTime.Now.AddMinutes(20);
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            #region body
            string body = @"<div style=""width:500px;margin:auto;background-color:#fbfbfb"">
         <div style=""width:95%;margin:auto;height:60px;background-color:#66B85C"">
            <div style=""font-family:verdana, calibri;font-size:95%;color:white;font-weight:bold;"">
                <p style=""padding:20px;text-align:center"">  Forgot Password</p>
            </div>
        </div>
        <br />
        <div style=""width:95%;margin:auto"">
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px;font-weight:bold "">
                Dear " + user.First().Username + @",
            </div>
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;text-align:justify"">
                Thanks for choosing  Nano Pips.

            </div>

            <br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;text-align:justify"">
                Someone has requested a link to change your password and you can do this by clicking on the below. If you did not request this password reset, please feel free to ignore this email. This link will expire in 20 minutes.
            </div>
         
         
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff;text-align:center;font-weight:bold"">
                Please click on the link below to reset your password.
            </div>
            <br />
            
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff;text-align:center;font-weight:bold"">
               <a style=""background-color:#66B85C;color:#fff;padding: 15px 32px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;"" href=""" + "" + Constants.Domain + @"/Account/VerifyPasswordReset?code=" + user.First().PasswordHash + @""" target=""_blank"">CLICK HERE TO RESET YOUR PASSWORD</a>
            </div>
            <br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff"">
               Best regards,<br />
                Nano Pips Team
            </div>
            <br />
            <br />
            <br />
        </div>


        <div style=""width:95%;margin:auto;height:50px;background-color:#66B85C"">

          

            <div style=""font-family:verdana, calibri; font-size:12.0px; color:rgb(0,0,1); text-align:center; background:rgb(255,255,255); text-transform:uppercase"">
                COPYRIGHT © " + DateTime.Now.Year + @" ALL RIGHTS RESERVED BY <br /> Nano Pips
            </div>
        </div>
    </div>";
            #endregion
            new Thread(() => SMTPMailClient.SendEmail(body, "NanoPips, Password Reset Instructions", new string[] { user.First().Username })).Start();
            return true;
        }

        public Account VerifyPasswordReset(string hashPassword)
        {
            var user = _db.UserRoles.Where(m => m.PasswordHash == hashPassword).ToList();
            if (user.Count == 0)
            {
                ExchangeException.Throw(ErrorCode.INVALID_LINK, null);
            }
            if (user.First().HashExpirationDate.Value.CompareTo(DateTime.Now) < 0)
            {
                ExchangeException.Throw(ErrorCode.LINK_HAS_BEEN_EXPIRED, null);
            }
            return user.First().Account;
        }

        public bool ResetPassword(long acId, string password)
        {
            var ac = _db.Accounts.Find(acId);
            ac.UserRoles.First().Password = password;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return true;
        }

        public Account UpdateAccount(long id)
        {
            var ac = _db.Accounts.Find(id);
            ac.AccountType_Id = 2;
            //ac.FirstName = model.FirstName;
            //ac.LastName = model.LastName;
            //ac.MiddleName = model.MiddleName;
            //ac.Address1 = model.Address1;
            //ac.Address2 = model.Address2;
            //ac.City = model.City;
            //ac.Country = model.Country;
            //ac.DOBMonth = model.DOBMonth;
            //ac.DOBDay = model.DOBDay;
            //ac.DOBYear = model.DOBYear;
            //ac.PlaceOfBirth = model.PlaceOfBirth;
            //if (ac.AccountType_Id != (int)AccountTypes.VERIFIED)
            //{
            //    ac.IdentityProofURL = model.IdentityProofURL;
            //    ac.IdentityProofURL2 = model.IdentityProofURL2;
            //    ac.IdentityNumber = model.IdentityNumber;
            //    ac.AccountType_Id = (int)AccountTypes.BASIC;
            //}
            //ac.ZipCode = model.ZipCode;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.Notifications.Add(new Notification() { Account_Id = null, IsViewed = false, NotificationDetails = "Documents uploaded, Please verify", NotificationHeading = "KYC", RedirectAction = "AccountDetail?acId=" + id, RedirectController = "Admin", NotificationType = "Desktop Notification" });
            _db.SaveChanges();
            return ac;
        }

        public string Get2FABarCodeURL(string email)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            SetupCode code = tfa.GenerateSetupCode("Nano Pips", email, Constants.TwoFactorSecret, 300, 300);
            return code.QrCodeSetupImageUrl;
        }

        public Account Authenticate2FACode(long acId, string code)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var ac = _db.Accounts.Find(acId);
            bool valid = tfa.ValidateTwoFactorPIN(Constants.TwoFactorSecret, code);
            if (valid)
            {
                if (ac.TwoFactorEnabled != true)
                {
                    ac.TwoFactorEnabled = true;
                    ac.TwoFactorEnableDate = DateTime.Now;
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.SaveChanges();
                }
                return ac;
            }
            else
            {
                ExchangeException.Throw(ErrorCode.INVALID_CODE, null);
                return null;
            }
        }

        public bool CheckAgentAndHost(string userAgent, string ip, string email, long acId)
        {
            var ac = _db.Accounts.Where(a => a.AccountId == acId).FirstOrDefault();
            var luserAct = _db.UserActivities.Where(u => u.EmailAddress == email).OrderByDescending(u => u.ActivityId).FirstOrDefault();
            var userAct  = _db.UserActivities.Where(u => u.EmailAddress == email).OrderByDescending(u => u.ActivityId).ToList();
            if(userAct.Count() > 0)
            {
                if (luserAct.IsVerified != true)
                {
                    if (userAct.Any(u => u.UserAgent == userAgent))
                    { }
                    else
                    {
                        SendVerificationEmail(acId, email);
                        return false;
                    }
                    if (userAct.Any(u => u.IPAddress == ip))
                    { }
                    else
                    {
                        SendVerificationEmail(acId, email);
                        return false;
                    }
                }
                else return true;
            }
            return true;
        }

        public bool SaveUserActivity(string userAgent, string ip, string email, string type)
        {
            try
            {
                _db.UserActivities.Add(new UserActivity() { ActivityDate = DateTime.Now, ActivityType = type, EmailAddress = email, IPAddress = ip, UserAgent = userAgent });
                _db.SaveChanges();
                return true;
            }
            catch
            {
                ExchangeException.Throw(ErrorCode.OTHER_ERROR, null);
                return false;
            }
        }

        public List<UserActivity> GetTop5Activities(string email)
        {
            return _db.UserActivities.Where(m => m.EmailAddress == email).OrderByDescending(m => m.ActivityId).Take(5).ToList();
        }

        public void ReadAllNotification()
        {
            var notifications = _db.Notifications.Where(m => m.IsViewed == false && m.Account_Id == null).ToList();

            foreach (var notification in notifications)
            {
                notification.IsViewed = true;
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
            }
        }

        public void ReadDesktopNotification(long id)
        {
            var notification = _db.Notifications.Where(m => m.NotificationId == id).FirstOrDefault();
            notification.NotificationType = null;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
        }

        public bool EditUserAccount(long AccountId, string firstName, string lastName, string address, string city, string country, string pob)
        {
                var ac = _db.Accounts.Find(AccountId);
                ac.FirstName = firstName;
                ac.LastName = lastName;
                ac.Address1 = address;
                ac.City = city;
                ac.Country = country;
                ac.PlaceOfBirth = pob;
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
                return true;           
        }

        public SecurityAnswer securityAnswer(long AccountId, string firstQuestion, string SecAns1, string secondQuestion, string SecAns2, string thirdQuestion, string SecAns3)
        {
            var kycDetail = new SecurityAnswer()
            {
                AccId=AccountId,
                SecQuest1=firstQuestion,
                SecAns1=SecAns1,
                SecQuest2 = secondQuestion,
                SecAns2 = SecAns2,
                SecQuest3 = thirdQuestion,
                SecAns3 = SecAns3,
            };
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SecurityAnswers.Add(kycDetail);
            _db.SaveChanges();
            return kycDetail;
        }
        public Account GetAccount(long acId)
        {
            return _db.Accounts.Where(m => m.AccountId == acId).First();
        }

        public bool DeleteWhiteListAddress(long addId)
        {
            var obj=_db.AddressBooks.Where(x => x.Enabled == true && x.AdressId == addId).FirstOrDefault();
            obj.Enabled = false;
            _db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public List<Payment> GetRequestedWithdrawals()
        {
            return _db.Payments.Where(m => m.PaymentStatus_Id == (int)1 && m.StatusMessage == "Withdrawl Requested").ToList();
        }

        public List<Pair> GetPairs()
        {
            try { return _db.Pairs.Where(p => p.Status == true).ToList(); }
            catch (Exception) { }
            return new List<Pair>();
        }

        public List<Payment> GetDepositHistory(long acId)
        {
            return _db.Payments.Where(p => p.Account_Id == acId && p.PaymentType == "DEPOSIT").ToList();
        }

        public List<Payment> GetWithdrawHistory(long acId)
        {
            return _db.Payments.Where(p => p.Account_Id == acId && p.PaymentType == "WITHDRAW").ToList();
        }

        public List<Trade> GetTradeHistory(long acId)
        {
            return _db.Trades.Where(x => x.Account_Id == acId).ToList();
        }
        public List<Trade> GetPendingTrades(long acId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            return _db.Trades.Where(x => x.Account_Id == acId && x.Status == "PENDING").ToList();
        }
        public List<Trade> GetPendingTrades(long acId, string pair)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            return _db.Trades.Where(x => x.Account_Id == acId && x.Status == "PENDING" && x.Symbol == pair).ToList();
        }

        public List<Currency> GetCurrencyList()
        {
            return _db.Currencies.Where(x => x.Status == true).ToList();
        }

        public KYC kyc(KYC model, long acId, string firstDocumentPath, string secondDocumentPath)
        {
            var acc = _db.Accounts.Where(a => a.AccountId == acId).FirstOrDefault();

            var kycDetail = new KYC()
            {
                AccId          = acId,
                FirstDocument  = model.FirstDocument,
                SecondDocument = model.SecondDocument,
                IsEnable       = true
            };
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.KYCs.Add(kycDetail);
            _db.SaveChanges();
            #region body
            string body = @"<div style=""width:500px;margin:auto;background-color:#fbfbfb"">
        <div style=""width:95%;margin:auto;height:60px;background-color:#66B85C"" >
            <div style=""font-family:verdana, calibri;font-size:95%;color:white;font-weight:bold;"">
                <p style=""padding:20px;text-align:center"">  Welcome to Nano Pips</p>
            </div>
        </div>
        <br />
        <div style=""width:95%;margin:auto"">
            <br /><br /><br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px;font-weight:bold "">
                Dear Admin, 
            </div>
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;text-align:justify"">
            A User with Email: " + acc.Email + @" Upload Documents, Please Check this.</div>
            <br /><br /><br />
            <a href='"  + Constants.Domain + model.FirstDocument  + "'>Document 1</a> <br />" +
            "<a href='" + Constants.Domain + model.SecondDocument + "'>Document 2</a> <br /><br />" +
            @"<div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff;text-align:center;font-weight:bold"">                           
        </div>
            <br />
            <div style=""font-family:verdana, calibri; color:rgb(0,0,1); font-size:14.0px; line-height:20.0px; padding-top:10px;border-bottom:1px dashed #007bff"">
               Best regards,<br />
               Nano Pips Team
            </div>
            <br />
            <br />
            <br />

        </div>

        <div style=""width:95%;margin:auto;height:50px;background-color:#66B85C"">      
            <div style=""font-family:verdana, calibri; font-size:12.0px; color:rgb(0,0,1); text-align:center; background:rgb(255,255,255); text-transform:uppercase"">
                COPYRIGHT © " + DateTime.Now.Year + @" ALL RIGHTS ARE RESERVED BY Nano Pips
            </div>
        </div>
    </div>";
            #endregion
            //"clientservice@nanopips.com"
            new Thread(() => SMTPMailClient.SendEmail(body, "NanoPips, Documents Uploaded", new string[] { "clientservice@nanopips.com", "zabi.butt1989@gmail.com" })).Start();
            return kycDetail;
        }

        public List<Trade> GetTradeHistory(long account_id, DateTime dateFrom, DateTime dateTo)
        {
            try {
                _db.Configuration.ProxyCreationEnabled = false;
               return _db.Trades.Where(x => x.Account_Id == account_id && DbFunctions.TruncateTime(x.TradeDate) >= dateFrom && DbFunctions.TruncateTime(x.TradeDate) <= dateTo && x.Status == "COMPLETED").ToList();
            }
            catch(Exception ex) {
                return new List<Trade>();
            }
         }

        public Account SetTerm_Conditions(long account_id, bool value)
        {
            try
            {
                var acc = _db.Accounts.Where(x => x.AccountId == account_id).FirstOrDefault();
                acc.IsAgreeTermServices = value;
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
                return acc;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public decimal? GetTotalUsersLoses()
        {
            decimal? loses = 0;
            try
            {
                var accountList = _db.Accounts.Where(x => x.AccountType_Id == 1).ToList();
                for (int t = 0; t < accountList.Count; t++) {
                    var id = accountList[t].AccountId;
                    List<Wallet> wallet = _db.Wallets.Where(z => z.Account_Id == id && z.Currency == "USD").ToList();
                    if (wallet.Count() != 0) {
                        if (wallet[0].Balance <= 10) {
                            var sumDeposit  = _db.Payments.Where(v => v.Account_Id == id && v.PaymentType == "DEPOSIT").Sum(g => g.Amount);
                            var sumWithdraw = _db.Payments.Where(w => w.Account_Id == id && w.PaymentType == "WITHDRAW").Sum(g => g.Amount);
                            loses += (((sumDeposit.HasValue) ? sumDeposit : 0) - ((sumWithdraw.HasValue) ? Math.Abs(sumWithdraw.Value) : 0));
                        }
                    }
                }
                return loses;
            }
            catch (Exception ex) {
                return loses;
            }
        }
    }
}
