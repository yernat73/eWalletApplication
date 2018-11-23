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

        // ForeignKeys
        public string UserId { get; set; }
        public int AccountId { get; set; }
        public OutcomeCategory Category { get; set; }
        
    }


    public class OutcomeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public OutcomeCategoryIcon Icon { get; set; }
        public string UserId { get; set; }
    }

    public class OutcomeCategoryIcon
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}