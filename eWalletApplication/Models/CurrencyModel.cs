using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWalletApplication.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Name { get; set; }

        // ForeignKeys
        public CurrencyIcon Icon { get; set; }
    }

    public class CurrencyIcon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
    }
}