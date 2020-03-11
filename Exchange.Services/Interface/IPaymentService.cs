using Exchange.DTO;
using System;
using System.Collections.Generic;

namespace Exchange.Services.Interface
{
    public interface IPaymentService
    {
        string GetDepositAddress(string cur);
        List<AddressBook> GetCurrencyAddressBook(string cur, long id);
        List<AddressBook> AddWhitListAddress(string cur, long id, long addressId, string newAddress);
        Boolean DeleteWhitListAddress(string cur, long id, long addressId, string newAddress);
        Boolean AddressAuthorize(string cur, long id, string email);
        Account WithDrawlReqAdmin(Payment payment);
        Account WireWithDrawlReqAdmin(Payment payment);
        string  Sell(long acId, string pair, decimal rate, decimal amount, int type, int expiretime, decimal timeOffset);
        string  Buy(long acId, string pair, decimal rate, decimal amount, int type, int expirytime, decimal timeOffset);
        List<Trade> GetOrders();
        Trade CloseOrder(long tradeid, long accId);
        bool  CreditBalance(string email, string currency, decimal amount, string reason);
        List<Wallet> GetWallets(long id, string currency);
        List<Payment> GetSpecificPayments(long account_id, DateTime dateFrom, DateTime dateTo);
        List<Trade> GetOrdersById(long id);
        bool UpdateST(long tradeId, decimal stopLoss, decimal takeProfit);
        string AddRefferal(string email, long accounId);
        string refferalSubtract(decimal amount, long accounId, string reason);
    }
}
