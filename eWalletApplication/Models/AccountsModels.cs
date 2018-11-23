using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eWalletApplication.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public bool Active { get; set; }

        // ForeignKeys
        public int IconId { get; set; }
        public AccountIcon Icon { get; set; }
        public string UserId { get; set; }

    }

    public class AccountIcon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Account> Account { get; set; }

    }
}