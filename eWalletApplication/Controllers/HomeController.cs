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
        WalletContext db = new WalletContext();

        private ActionResult Default() {
            if (Request.IsAuthenticated)
            {
                string UserId = User.Identity.GetUserId();
                var Accounts = db.Accounts.Where(a => a.UserId == UserId).Where(a => a.Active == true).ToList<Account>();
                ViewBag.Accounts = Accounts;

                foreach(Account account in Accounts)
                {
                    db.Entry(account).Reference(a => a.Icon).Load();
                }

                
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                if (id != 0)
                {
                    Account account = db.Accounts.Find(id);
                    if (account != null)
                    {
                        db.Entry(account).Reference(a => a.Icon).Load();
                        ViewBag.Account = account;
                    }
                }

            }


            return Default();
        }




        public ActionResult About()
        {
            return Default();
        }

        public ActionResult Contact()
        {
            return Default();
        }



        public ActionResult AddAccount()
        {
            if (Request.IsAuthenticated)
            {
                string UserId = User.Identity.GetUserId();
                var Accounts = db.Accounts.Where(a => a.UserId == UserId).Where(a => a.Active == true).ToList<Account>();
                var AccountIcons = db.AccountIcons;

                ViewBag.Accounts = Accounts;
                ViewBag.AccountIcons = AccountIcons;

                foreach (Account account in Accounts)
                {
                    db.Entry(account).Reference(a => a.Icon).Load();
                }
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
                
                return RedirectToAction("Index", "Home");


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
                

                return RedirectToAction("Index", "Home");


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
                string UserId = User.Identity.GetUserId();
                var Accounts = db.Accounts.Where(a => a.UserId == UserId).Where(a => a.Active == true).ToList<Account>();
                var AccountIcons = db.AccountIcons;
                db.Entry(Account).Reference(a => a.Icon).Load();
                ViewBag.Account = Account;
                ViewBag.Accounts = Accounts;
                ViewBag.AccountIcons = AccountIcons;

                foreach (Account account in Accounts)
                {
                    db.Entry(account).Reference(a => a.Icon).Load();
                }
                if (Account == null)
                {
                    return HttpNotFound();
                }

                
                
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
                Account Account = db.Accounts.Find(id);
                if(TryUpdateModel(Account,"", new string[] {"Name", "IconId" }))
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

                

                return RedirectToAction("Index", "Home", new { id });


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }


    }
}