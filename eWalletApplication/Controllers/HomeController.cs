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

        public void LoadView() {
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


        }

        public ActionResult MorrisData()
        {
            Stack<string> stack = new Stack<string>();

            var data = "$(function() { Morris.Area({element: 'morris-area-chart',data: [";
            string UserId = User.Identity.GetUserId();
            DateTime today = DateTime.Today;

            DateTime start = new DateTime(today.Year, today.Month, 1);
            DateTime end = start.AddMonths(1);
            while (true)
            {
                string tmp = "";
                double income = 0;
                double outcome = 0;
                var Incomes = db.Incomes.Where(i => i.UserId == UserId).Where(i => i.Date >= start && i.Date < end).ToList();
                var Outcomes = db.Outcomes.Where(i => i.UserId == UserId).Where(i => i.Date >= start && i.Date < end).ToList();
                if(Incomes.Count == 0 || Outcomes.Count == 0)
                {
                    break;
                }
                foreach(Income i in Incomes)
                {
                    income += i.Value;
                }
                foreach (Outcome o in Outcomes)
                {
                    outcome += o.Value;
                }
                stack.Push("{Period: "+start.Year+"/"+start.Month+", Income: "+income+", Outcome: "+outcome+"},");

                start.AddMonths(-1);
                end.AddMonths(-1);


            }
            while (stack.Peek() != null)
            {
                data += stack.Pop();
            }
            data += "], xkey: 'Period', ykeys: ['Income', 'Outcome'], labels: ['Income', 'Outcome'], pointSize: 2, hideHover: 'auto', resize: true }); });";
            


            return JavaScript(data);
        } 



        public ActionResult Index(int? id)
        {
            if (Request.IsAuthenticated)
            {
                string UserId = User.Identity.GetUserId();
                if (id != null)
                {
                    
                    if (id != 0)
                    {
                        Account Account = db.Accounts.Find(id);
                        if (Account != null)
                        {
                            db.Entry(Account).Reference(a => a.Icon).Load();
                            ViewBag.Account = Account;
                            var Incomes = db.Incomes.Where(i => i.UserId == UserId).Where(i => i.AccountId == id).ToList<Income>();
                            foreach (Income income in Incomes)
                            {
                                db.Entry(income).Reference(i => i.Category).Load();
                                db.Entry(income.Category).Reference(c => c.Icon).Load();
                            }
                            var Outcomes = db.Outcomes.Where(i => i.UserId == UserId).Where(i => i.AccountId == id).ToList<Outcome>();
                            foreach (Outcome outcome in Outcomes)
                            {
                                db.Entry(outcome).Reference(o => o.Category).Load();
                                db.Entry(outcome.Category).Reference(c => c.Icon).Load();
                            }
                            ViewBag.Incomes = Incomes;
                            ViewBag.Outcomes = Outcomes;
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Account not Found";
                        }
                    }
                    else
                    {
                        var Incomes = db.Incomes.Where(i => i.UserId == UserId).ToList<Income>();
                        foreach (Income income in Incomes)
                        {
                            db.Entry(income).Reference(i => i.Category).Load();
                            db.Entry(income.Category).Reference(c => c.Icon).Load();
                        }
                        var Outcomes = db.Outcomes.Where(i => i.UserId == UserId).ToList<Outcome>();
                        foreach (Outcome outcome in Outcomes)
                        {
                            db.Entry(outcome).Reference(o => o.Category).Load();
                            db.Entry(outcome.Category).Reference(c => c.Icon).Load();
                        }
                        ViewBag.Incomes = Incomes;
                        ViewBag.Outcomes = Outcomes;
                    }

                }
                else
                {
                    var Incomes = db.Incomes.Where(i => i.UserId == UserId).ToList<Income>();
                    foreach (Income income in Incomes)
                    {
                        db.Entry(income).Reference(i => i.Category).Load();
                        db.Entry(income.Category).Reference(c => c.Icon).Load();
                    }
                    var Outcomes = db.Outcomes.Where(i => i.UserId == UserId).ToList<Outcome>();
                    foreach (Outcome outcome in Outcomes)
                    {
                        db.Entry(outcome).Reference(o => o.Category).Load();
                        db.Entry(outcome.Category).Reference(c => c.Icon).Load();
                    }
                    ViewBag.Incomes = Incomes;
                    ViewBag.Outcomes = Outcomes;
                }
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