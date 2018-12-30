using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eWalletApplication.Models;
using Microsoft.AspNet.Identity;


namespace eWalletApplication.Controllers
{
    

    public class HomeController : Controller
    {
        public WalletContext db = new WalletContext();

        private void LoadView() {
            string UserId = User.Identity.GetUserId();
            var Accounts = db.Accounts.Where(a => a.UserId == UserId).Where(a => a.Active == true).ToList<Account>();
            ViewBag.Accounts = Accounts;
            var AccountIcons = db.AccountIcons;
            ViewBag.AccountIcons = AccountIcons;
            foreach (Account account in Accounts)
            {
                db.Entry(account).Reference(a => a.Icon).Load();
            }

            var DeletedAccounts = db.Accounts.Where(a => a.UserId == UserId).Where(a => a.Active == false).ToList<Account>();

            ViewBag.DeletedAccounts = DeletedAccounts;
            foreach (Account account in DeletedAccounts)
            {
                db.Entry(account).Reference(a => a.Icon).Load();
            }
            var IncomeCategories = db.IncomeCategories.Where(c => c.UserId == UserId).Where(c => c.Active == true).ToList<IncomeCategory>();
            var OutcomeCategories = db.OutcomeCategories.Where(c => c.UserId == UserId).Where(c => c.Active == true).ToList<OutcomeCategory>();
            foreach(IncomeCategory incomeCategory in IncomeCategories) {
                db.Entry(incomeCategory).Reference(c => c.Icon).Load();
            }
            foreach (OutcomeCategory outcomeCategory in OutcomeCategories)
            {
                db.Entry(outcomeCategory).Reference(c => c.Icon).Load();
            }
            ViewBag.IncomeCategories = IncomeCategories;
            ViewBag.OutcomeCategories = OutcomeCategories;


        }

        public ActionResult Index(int id)
        {
            

            if (Request.IsAuthenticated)
            {
                string UserId = User.Identity.GetUserId();
                Account account = null;

                List<Income> Incomes;
                List<Outcome> Outcomes;
                List<IncomeCategory> IncomeCategories;
                List<OutcomeCategory> OutcomeCategories;
                IncomeCategories = db.IncomeCategories.Where(c => c.UserId == UserId).ToList<IncomeCategory>();
                OutcomeCategories = db.OutcomeCategories.Where(c => c.UserId == UserId).ToList<OutcomeCategory>();

                var IncomePie = new Dictionary<string, double>();
                var OutcomePie = new Dictionary<string, double>();

                if (id != 0)
                {
                    Incomes = db.Incomes.Where(i => i.UserId == UserId).Where(i => i.AccountId == id).ToList<Income>();
                    Outcomes = db.Outcomes.Where(i => i.UserId == UserId).Where(i => i.AccountId == id).ToList<Outcome>();
                    account = db.Accounts.Find(id);
                    if (account.UserId != UserId)
                    {
                        account = null;
                    }


                }
                else
                {
                    Incomes = db.Incomes.Where(i => i.UserId == UserId).ToList<Income>();
                    Outcomes = db.Outcomes.Where(i => i.UserId == UserId).ToList<Outcome>();
                    

                }

               
                foreach (IncomeCategory c in IncomeCategories)
                {
                    double sum = Incomes.Where(i => i.CategoryId == c.Id).Sum(i => i.Value);
                    IncomePie.Add(c.Name, sum);

                }
                foreach (OutcomeCategory c in OutcomeCategories)
                {
                    double sum = Outcomes.Where(o => o.CategoryId == c.Id).Sum(o => o.Value);
                    OutcomePie.Add(c.Name, sum);

                }
                

                foreach (Income income in Incomes)
                {
                    db.Entry(income).Reference(i => i.Category).Load();
                    db.Entry(income.Category).Reference(c => c.Icon).Load();
                }
               
                foreach (Outcome outcome in Outcomes)
                {
                    db.Entry(outcome).Reference(o => o.Category).Load();
                    db.Entry(outcome.Category).Reference(c => c.Icon).Load();
                }
                ViewBag.Incomes = Incomes;
                ViewBag.Outcomes = Outcomes;
                ViewBag.Account = account;
                ViewBag.IncomePie = IncomePie;
                ViewBag.OutcomePie = OutcomePie;
                LoadView();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }



        }
        
        public ActionResult AddAccount()
        {
            if (Request.IsAuthenticated)
            {
                LoadView();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAccount([Bind(Include = "AccountId,Name,IconId,Balance")]Account account) {
            if (Request.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        account.Active = true;
                        account.UserId = User.Identity.GetUserId();
                        account.Icon = db.AccountIcons.Find(account.IconId);

                        db.Accounts.Add(account);
                        db.SaveChanges();
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
                ViewBag.SuccessMessage = "Account was added successfully";
                LoadView();
                return View("AddAccount");


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        [HttpPost]
        public ActionResult DeleteAccount(int id)
        {
            if (Request.IsAuthenticated)
            {
                Account account = db.Accounts.Find(id);
                if(account.UserId.Equals(User.Identity.GetUserId()))
                {
                    account.Active = false;
                    db.SaveChanges();
                }

                ViewBag.SuccessMessage = "Account was deleted successfully";

                LoadView();
                return View("Index");


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        public ActionResult EditAccount(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Account Account = db.Accounts.Find(id);
                ViewBag.Account = Account;

                if (Account == null)
                {
                    return HttpNotFound();
                }
                
                LoadView();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }


        [HttpPost, ActionName("EditAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccountPost(int? id)
        {

            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Account account = db.Accounts.Find(id);
                if(TryUpdateModel(account,"", new string[] {"Name", "IconId" }))
                {
                    try
                    {
                        db.SaveChanges();

                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }

                ViewBag.SuccessMessage = "Account was edited successfully";

                LoadView();
                return View("Index");


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        public ActionResult Retrieve() {
            if (Request.IsAuthenticated)
            {
                LoadView();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        [HttpPost]
        public ActionResult Retrieve(int id)
        {
            if (Request.IsAuthenticated)
            {
                Account account = db.Accounts.Find(id);
                if (account != null)
                {
                    account.Active = true;
                    db.SaveChanges();
                }
                else
                {
                    return HttpNotFound();
                }
                ViewBag.SuccessMessage = "Account was retieved successfully";
                LoadView();
                return View("Retrieve");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

    }
}