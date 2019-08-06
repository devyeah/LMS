using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.BusinessTest.Mock
{
    public class AccountRepositoryMocker : IAccountRepository
    {
        private readonly List<Account> _accounts;
        public AccountRepositoryMocker()
        {
            _accounts = new List<Account>();
        }
        public void Add(Account entity)
        {
            _accounts.Add(entity);
        }

        public int Count(Expression<Func<Account, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Delete(Account entity)
        {
            _accounts.Remove(entity);
        }

        public Account Find(Expression<Func<Account, bool>> expression)
        {
            return null;
        }

        public IEnumerable<Account> FindAll(Expression<Func<Account, bool>> expression)
        {
            return null;
        }

        public Account Get(Guid key)
        {
            foreach(var account in _accounts)
            {
                if (key == account.Id)
                    return account;
            }
            return null;
        }

        public Account GetAccount(Guid key)
        {
            return Get(key);
        }

        public Account GetUniqueAccountByEmail(string email)
        {
            foreach(var account in _accounts)
            {
                if (email.Equals(account.Email))
                    return account;
            }
            return null;
        }

        public void SaveChanges()
        {
            
        }

        public void Update(Account entity)
        {
            foreach(var account in _accounts)
            {
                if (entity.Id == account.Id)
                {
                    _accounts.Remove(account);
                    _accounts.Add(entity);
                    break;
                }
            }
        }
    }
}
