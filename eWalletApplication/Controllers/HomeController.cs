using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult AddAccount(Account account) {
            if (Request.IsAuthenticated)
            {
                account.Active = true;
                account.UserId = User.Identity.GetUserId();

                account.Icon = db.AccountIcons.Find(account.IconId);


                db.Accounts.Add(account);
                db.SaveChanges();

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

        public ActionResult EditAccount(int id)
        {
            Account account = db.Accounts.Find(id);
            if (account != null)
            {
                db.Entry(account).Reference(a => a.Icon).Load();
                ViewBag.Account = account;
            }


            return Default();
        }




    }
}