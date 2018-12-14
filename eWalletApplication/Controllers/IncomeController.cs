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
    public class IncomeController : Controller
    {
        public WalletContext db = new WalletContext();
        public void LoadView()
        {
            string UserId = User.Identity.GetUserId();
            var Accounts = db.Accounts.Where(a => a.UserId == UserId).Where(a => a.Active == true).ToList<Account>();
            var IncomeCategories = db.IncomeCategories.Where(c => c.UserId == UserId).ToList<IncomeCategory>();
            var IncomeCategoryIcons = db.IncomeCategoryIcons;

            foreach (IncomeCategory category in IncomeCategories) {
                db.Entry(category).Reference(c => c.Icon).Load();
            }
            foreach (Account account in Accounts)
            {
                db.Entry(account).Reference(a => a.Icon).Load();
            }




            ViewBag.Accounts = Accounts;
            ViewBag.IncomeCategories = IncomeCategories;
            ViewBag.IncomeCategoryIcons = IncomeCategoryIcons;



        }


        // GET: Income
        public ActionResult AddIncomeCategory()
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
        public ActionResult AddIncome()
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
        public ActionResult AddIncomeCategory([Bind(Include = "Id,Name,IconId")]IncomeCategory incomeCategory)
        {
            if (Request.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        incomeCategory.Active = true;
                        incomeCategory.UserId = User.Identity.GetUserId();
                        incomeCategory.Icon = db.IncomeCategoryIcons.Find(incomeCategory.IconId);

                        db.IncomeCategories.Add(incomeCategory);
                        db.SaveChanges();
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
                ViewBag.SuccessMessage = "Income Category was added successfully";
                LoadView();
                return View("AddIncome");


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddIncome([Bind(Include = "Id, Value, Notes, AccountId, CategoryId")]Income income)
        {
            if (Request.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        income.UserId = User.Identity.GetUserId();
                        income.Date =  DateTime.Today;
                        Account account = db.Accounts.Find(income.AccountId);
                        account.Balance += income.Value; 
                        
                        db.Incomes.Add(income);
                        db.SaveChanges();
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
                ViewBag.SuccessMessage = "Income was added successfully";
                LoadView();
                return View("AddIncome");


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

    }
}