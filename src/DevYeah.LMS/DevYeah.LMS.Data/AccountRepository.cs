using System;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace DevYeah.LMS.Data
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Account GetUniqueAccountByEmail(string email) => this.Find(account => account.Email == email);

        public Account GetAccount(Guid key) => this.Get(key);
    }
}
