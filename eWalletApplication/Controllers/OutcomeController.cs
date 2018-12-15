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
    public class OutcomeController : Controller
    {
        public WalletContext db = new WalletContext();
        public void LoadView()
        {
            string UserId = User.Identity.GetUserId();
            var Accounts = db.Accounts.Where(a => a.UserId == UserId).Where(a => a.Active == true).ToList<Account>();
            var OutcomeCategories = db.OutcomeCategories.Where(c => c.UserId == UserId).ToList<OutcomeCategory>();
            var OutcomeCategoryIcons = db.OutcomeCategoryIcons;

            foreach (OutcomeCategory category in OutcomeCategories)
            {
                db.Entry(category).Reference(c => c.Icon).Load();
            }
            foreach (Account account in Accounts)
            {
                db.Entry(account).Reference(a => a.Icon).Load();
            }




            ViewBag.Accounts = Accounts;
            ViewBag.OutcomeCategories = OutcomeCategories;
            ViewBag.OutcomeCategoryIcons = OutcomeCategoryIcons;



        }


        // GET: Outcome
        public ActionResult AddOutcomeCategory()
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
        public ActionResult AddOutcome()
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
        public ActionResult AddOutcomeCategory([Bind(Include = "Id,Name,IconId")]OutcomeCategory outcomeCategory)
        {
            if (Request.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        outcomeCategory.Active = true;
                        outcomeCategory.UserId = User.Identity.GetUserId();
                        outcomeCategory.Icon = db.OutcomeCategoryIcons.Find(outcomeCategory.IconId);

                        db.OutcomeCategories.Add(outcomeCategory);
                        db.SaveChanges();
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
                ViewBag.SuccessMessage = "Outcome Category was added successfully";
                LoadView();
                return View("AddOutcome");


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOutcome([Bind(Include = "Id, Value, Notes, AccountId, CategoryId")]Outcome outcome)
        {
            if (Request.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        outcome.UserId = User.Identity.GetUserId();
                        outcome.Date = DateTime.Now;
                        Account account = db.Accounts.Find(outcome.AccountId);
                        account.Balance -= outcome.Value;

                        db.Outcomes.Add(outcome);
                        db.SaveChanges();
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
                ViewBag.SuccessMessage = "Outcome was added successfully";
                LoadView();
                return View("AddOutcome");


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }
    }
}