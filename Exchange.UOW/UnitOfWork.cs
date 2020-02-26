using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exchange.Services.Interface;
using static Exchange.UOW.UOWRegistration;
using Ninject;

namespace Exchange.UOW
{

    internal class UnitOfWork : IUnitOfWork
    {
        public IMarketRatesService MarketRates => GlobalKernel.Get<IMarketRatesService>();

        public IAccountService Accounts => GlobalKernel.Get<IAccountService>();

        public IPaymentService Payment => GlobalKernel.Get<IPaymentService>();
    }
}
