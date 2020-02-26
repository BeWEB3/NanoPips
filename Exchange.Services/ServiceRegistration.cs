using Exchange.EF;
using Exchange.Services.Implementation;
using Exchange.Services.Interface;
using Ninject;

namespace Exchange.Services
{
    public class ServiceRegistration
    {
        public static IKernel GlobalKernel
        {
            get; private set;
        }
        public static void BindAll(IKernel kernel)
        {
            kernel.Bind<IMarketRatesService>().To<MarketRatesService>();
            kernel.Bind<IAccountService>().To<AccountsService>();
            kernel.Bind<IPaymentService>().To<PaymentService>();
            kernel.Bind<ExchangeEntities>().ToSelf();
            GlobalKernel = kernel;
        }

    }
}
