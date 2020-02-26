using Exchange.Common;
using Exchange.DTO;
using Exchange.EF;
using Exchange.UOW;
using Ninject;
using System;
using System.Linq;

namespace TestConsole
{
    public class Program
    {
        private static IUnitOfWork uow = null;

        static void Main(string[] args)
        {
            try
            {
                IKernel kernel = new StandardKernel();
                UOWRegistration.BindAll(kernel);
                uow = kernel.Get<IUnitOfWork>();
            }
            catch (Exception ex) { Console.WriteLine("Error on Application Start:   " + ex.Message); }

            using (ExchangeEntities db = new ExchangeEntities())
            {
                var accList = db.Accounts.ToList();
                foreach (var acc in accList) {
                    var val = db.Accounts.Where(t => t.AccountId == acc.AccountId).FirstOrDefault();
                    val.RefferenceNumber = val.Email.Substring(0, 3) + val.AccountId;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
            }

        }
    }
}
