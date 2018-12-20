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

            // SEED FOR INCOMECATEGORYICONS
            context.IncomeCategoryIcons.Add(new IncomeCategoryIcon { Name = "fas fa-dollar-sign" });
            context.IncomeCategoryIcons.Add(new IncomeCategoryIcon { Name = "fas fa-coins" });
            context.IncomeCategoryIcons.Add(new IncomeCategoryIcon { Name = "fas fa-piggy-bank" });
            context.IncomeCategoryIcons.Add(new IncomeCategoryIcon { Name = "fas fa-gift" });
            context.IncomeCategoryIcons.Add(new IncomeCategoryIcon { Name = "fas fa-handshake" });
            context.IncomeCategoryIcons.Add(new IncomeCategoryIcon { Name = "fas fa-exchange-alt" });

            // SEED FOR OUTCOMECATEGORIES
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-car" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-tshirt" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-phone-volume" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-mobile-alt" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-utensils" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-cocktail" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-gift" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-briefcase-medical" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-home" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-paw" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-cat" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-dog" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-taxi" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-plane-departure"});
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-mountain" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-music" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-film" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-smoking" });
            context.OutcomeCategoryIcons.Add(new OutcomeCategoryIcon { Name = "fas fa-venus-mars" });






            base.Seed(context);
        }
    }
}