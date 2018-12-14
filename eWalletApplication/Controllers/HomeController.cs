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
            this.ViewBag.Accounts = Accounts;
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


        public ActionResult Index(int? accountId)
        {
            if (Request.IsAuthenticated)
            {
                if (accountId != null)
                {
                    if (accountId != 0)
                    {
                        Account account = db.Accounts.Find(accountId);
                        if (account != null)
                        {
                            db.Entry(account).Reference(a => a.Icon).Load();
                            ViewBag.Account = account;
                        }
                    }
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