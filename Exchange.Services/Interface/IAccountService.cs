using Exchange.Common;
using Exchange.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exchange.Services.Interface
{
    public interface IAccountService
    {
        Account GetAccount(long acId);
        List<Account> GetAllUsers();
        void ReadAllNotification();
        void ReadDesktopNotification(long id);
        bool ResetTwoFA(long acId);
        bool CreateAdminAccount(string accountType, string firstName, string lastName, string email, string password);
        bool EditAdminAccount(long AccountId, string accountType, string firstName, string lastName);
        bool EditUserAccount(long AccountId, string firstName, string lastName, string address, string city, string country, string pob);
        bool DisableAccount(string code);
        bool DeleteWhiteListAddress(long addId);
        List<Trade> GetPendingTrades(long acId);
        List<Trade> GetPendingTrades(long acId, string pair);
        bool Suspend(long acId);
        bool ChangePassword(long acId, string oldPassword, string newPassword);
        bool Activate(long acId);
        bool Approve(long acId);
        bool Reject(long acId, string reason);
        IQueryable<Account> Get();
        List<Notification> GetNotifications();
        List<Wallet> GetAllWallets(long acId, string from, string to);
        Notification UpdateNotification(long notificationId);
        List<Notification> GetNotifications(long acId);
        List<Notification> GetDesktopNotifications();
        IQueryable<Payment> GetTransectionHistory(long acId);
        IQueryable<Wallet> GetAllWallets(long acId);
        List<Wallet> GetWallets();
        Account SignUp(string username, string password, string referralNumber);
        Account Login(string username, string password);
        bool SendVerificationEmail(long acId);
        bool SendVerificationEmail(long acId, string email);
        bool CheckEmail(string email);
        bool VerifyEmail(string code);
        bool VerifyEmail(string code, string email);
        bool SendForgotPasswordEmail(string email);
        Account VerifyPasswordReset(string hashPassword);
        bool ResetPassword(long acId, string password);
        Account UpdateAccount(long id);
        string Get2FABarCodeURL(string email);
        Account Authenticate2FACode(long acId, string code);
        bool CheckAgentAndHost(string userAgent, string ip, string email, long acId);
        bool SaveUserActivity(string userAgent, string ip, string email, string type);
        List<UserActivity> GetTop5Activities(string email);
        List<Payment> GetRequestedWithdrawals();
        string GenerateSha256Hash(string rawPass);
        List<Pair> GetPairs();
        KYC kyc(KYC model, long acId, string firstDocumentPath, string secondDocumentPath);
        List<Trade> GetTradeHistory(long account_id, DateTime dateFrom, DateTime dateTo);
        Account SetTerm_Conditions (long account_id, bool value);
        bool SendEmail(string name, string email, string phoneNumber, string subject, string message);
        decimal? GetTotalUsersLoses();
        RefferalData GetRefferalData(long accId);
        //// New /////
        ///
        List<Payment>    GetDepositHistory(long acId);
        List<Payment>    GetWithdrawHistory(long acId);
        List<Trade>      GetTradeHistory(long acId);
        List<Currency>   GetCurrencyList();
        SecurityAnswer   securityAnswer(long AccountId, string firstQuestion, string SecAns1, string secondQuestion, string SecAns2, string thirdQuestion, string SecAns3);
    }
}
