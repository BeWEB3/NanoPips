using Exchange.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.UOW
{
    public interface IUnitOfWork
    {
        IMarketRatesService MarketRates { get; }
        IAccountService Accounts { get; }
        IPaymentService Payment { get; }
    }
}
