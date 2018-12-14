using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWalletApplication.Models
{
    public class Outcome
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }

        // ForeignKeys
        public string UserId { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int CategoryId { get; set; }
        public OutcomeCategory Category { get; set; }
        
    }


    public class OutcomeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public int IconId { get; set; }
        public OutcomeCategoryIcon Icon { get; set; }
        public string UserId { get; set; }
    }

    public class OutcomeCategoryIcon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<OutcomeCategory> Category { get; set; }
    }
}