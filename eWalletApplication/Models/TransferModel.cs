using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWalletApplication.Models
{
    public class Transfer
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }


        // ForeignKeys
        public int From { get; set; }
        public int To { get; set; }
    }
}