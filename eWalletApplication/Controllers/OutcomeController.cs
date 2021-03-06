﻿using System;
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
        private void LoadView()
        {
            string UserId = User.Identity.GetUserId();
            var Accounts = db.Accounts.Where(a => a.UserId == UserId).Where(a => a.Active == true).ToList<Account>();
            var OutcomeCategories = db.OutcomeCategories.Where(c => c.UserId == UserId).ToList<OutcomeCategory>();

            var IncomeCategories = db.IncomeCategories.Where(c => c.UserId == UserId).ToList<IncomeCategory>();
            var OutcomeCategoryIcons = db.OutcomeCategoryIcons;

            foreach (OutcomeCategory category in OutcomeCategories)
            {
                db.Entry(category).Reference(c => c.Icon).Load();
            }
            foreach (Account account in Accounts)
            {
                db.Entry(account).Reference(a => a.Icon).Load();
            }
           
            foreach (IncomeCategory incomeCategory in IncomeCategories)
            {
                db.Entry(incomeCategory).Reference(c => c.Icon).Load();
            }
            ViewBag.IncomeCategories = IncomeCategories;
            ViewBag.OutcomeCategories = OutcomeCategories;




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

        public ActionResult EditOutcomeCategory(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                OutcomeCategory Category = db.OutcomeCategories.Find(id);
                ViewBag.OutcomeCategory = Category;

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

        public ActionResult EditOutcome(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Outcome Outcome = db.Outcomes.Find(id);
                ViewBag.Outcome = Outcome;

                if (Outcome == null)
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



        [HttpPost, ActionName("EditOutcomeCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult EditOutcomeCategoryPost(int? id)
        {

            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                OutcomeCategory OutcomeCategory = db.OutcomeCategories.Find(id);
                
                if (TryUpdateModel(OutcomeCategory, "", new string[] { "Name", "IconId" }))
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

                ViewBag.SuccessMessage = "Outcome Category was edited successfully";

                LoadView();
                return RedirectToAction("Index", "Home", new { id = 0 });


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        [HttpPost, ActionName("EditOutcome")]
        [ValidateAntiForgeryToken]
        public ActionResult EditOutcomePost(int? id)
        {

            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Outcome Outcome = db.Outcomes.Find(id);
                Account Account = db.Accounts.Find(Outcome.AccountId);
                if (Account != null && Outcome != null)
                {
                    Account.Balance += Outcome.Value;
                }
                if (TryUpdateModel(Outcome, "", new string[] { "Value", "Notes", "AccountId", "CategoryId" }))
                {
                    try
                    {
                        Account.Balance -= Outcome.Value;
                        db.SaveChanges();

                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }

                ViewBag.SuccessMessage = "Outcome was edited successfully";

                LoadView();
                return RedirectToAction("Index", "Home", new { id = 0 });


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }
        [HttpPost]
        public ActionResult DeleteOutcome(int id)
        {
            if (Request.IsAuthenticated)
            {
                Outcome outcome = db.Outcomes.Find(id);
                Account account = db.Accounts.Find(outcome.AccountId);
                if (outcome != null)
                {
                    account.Balance += outcome.Value;
                    db.Outcomes.Remove(outcome);
                    db.SaveChanges();
                    ViewBag.SuccessMessage = "Outcome was deleted successfully";
                }


                return RedirectToAction("Index", "Home", new { id = account.Id });

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }
        [HttpPost]
        public ActionResult DeleteOutcomeCategory(int id)
        {
            if (Request.IsAuthenticated)
            {
                OutcomeCategory category = db.OutcomeCategories.Find(id);

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