using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWalletApplication.Models
{
    public class Income
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
        public IncomeCategory Category { get; set; }
        
    }


    public class IncomeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public int IconId { get; set; }
        public IncomeCategoryIcon Icon { get; set; }
        public string UserId { get; set; }
    }

    public class IncomeCategoryIcon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<IncomeCategory> Category { get; set; }

    }
}