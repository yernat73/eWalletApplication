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
        private void LoadView()
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
            var OutcomeCategories = db.OutcomeCategories.Where(c => c.UserId == UserId).ToList<OutcomeCategory>();
            
            foreach (OutcomeCategory outcomeCategory in OutcomeCategories)
            {
                db.Entry(outcomeCategory).Reference(iC => iC.Icon).Load();
            }
            ViewBag.IncomeCategories = IncomeCategories;
            ViewBag.OutcomeCategories = OutcomeCategories;




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

        public ActionResult EditIncomeCategory(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                IncomeCategory Category = db.IncomeCategories.Find(id);
                ViewBag.IncomeCategory = Category;

                if (Category == null)
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

        public ActionResult EditIncome(int? id) {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Income Income = db.Incomes.Find(id);
                ViewBag.Income = Income;

                if (Income == null)
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
                return View("AddIncomeCategory");


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

        [HttpPost, ActionName("EditIncomeCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult EditIncomeCategoryPost(int? id)
        {

            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                IncomeCategory IncomeCategory = db.IncomeCategories.Find(id);
                if (TryUpdateModel(IncomeCategory, "", new string[] { "Name", "IconId" }))
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

                ViewBag.SuccessMessage = "Income Category was edited successfully";

                LoadView();
                return RedirectToAction("Index", "Home", new { id = 0 });


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        [HttpPost, ActionName("EditIncome")]
        [ValidateAntiForgeryToken]
        public ActionResult EditIncomePost(int? id)
        {

            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Income Income = db.Incomes.Find(id);
                if (TryUpdateModel(Income, "", new string[] { "Value", "Notes", "AccountId", "CategoryId" }))
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

                ViewBag.SuccessMessage = "Income was edited successfully";

                LoadView();
                return RedirectToAction("Index", "Home", new { id = 0 });


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }
        [HttpPost]
        public ActionResult DeleteIncome(int id)
        {
            if (Request.IsAuthenticated)
            {
                Income income = db.Incomes.Find(id);
                Account account = db.Accounts.Find(income.AccountId);
                if (income != null)
                {
                    account.Balance -= income.Value;
                    db.Incomes.Remove(income);
                    db.SaveChanges();
                    ViewBag.SuccessMessage = "Income was deleted successfully";
                }
                

                return RedirectToAction("Index", "Home", new { id = account.Id});

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }
        [HttpPost]
        public ActionResult DeleteIncomeCategory(int id)
        {
            if (Request.IsAuthenticated)
            {
                IncomeCategory category = db.IncomeCategories.Find(id);
            
                if (category.UserId.Equals(User.Identity.GetUserId()))
                {
                    category.Active = false;
                    db.SaveChanges();
                }

                ViewBag.SuccessMessage = "Category was deleted successfully";

                LoadView();
                return RedirectToAction("Index", "Home", new { id = 0 });


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }



    }
}