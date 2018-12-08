using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace eWalletApplication.Models
{
    public class WalletContext : DbContext
    {

        // DbSets for AccountsModel
        public DbSet<AccountIcon> AccountIcons { get; set; }
        public DbSet<Account> Accounts { get; set; }


        //DbSets for IncomeModel
        public DbSet<IncomeCategoryIcon> IncomeCategoryIcons { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<Income> Incomes { get; set; }

        //DbSets for OutcomeModel
        public DbSet<OutcomeCategoryIcon> OutcomeCategoryIcons { get; set; }
        public DbSet<OutcomeCategory> OutcomeCategories { get; set; }
        public DbSet<Outcome> Outcomes { get; set; }

        //DbSets for TranserModel
        public DbSet<Transfer> Transfers { get; set; }

        //DbSets for CurrencyModel
        public DbSet<CurrencyIcon> CurrencyIcons { get; set; }
        public DbSet<Currency> Currencies { get; set; }


        public WalletContext() : base("name=WalletContext") {
            Database.SetInitializer(new WalletDbInitializer());
        }
    }

    public class WalletDbInitializer : CreateDatabaseIfNotExists<WalletContext>
    {
        protected override void Seed(WalletContext context)
        {
            // SEED FOR ACOUNT ICONS
            context.AccountIcons.Add(new AccountIcon { Name = "fas fa-money-bill" });
            context.AccountIcons.Add(new AccountIcon { Name = "fab fa-cc-mastercard" });
            context.AccountIcons.Add(new AccountIcon { Name = "fab fa-cc-visa" });
            context.AccountIcons.Add(new AccountIcon { Name = "fab fa-cc-paypal" });
            context.AccountIcons.Add(new AccountIcon { Name = "fas fa-money-check-alt"});
            context.AccountIcons.Add(new AccountIcon { Name = "fab fa-cc-apple-pay" });



            base.Seed(context);
        }
    }
}