using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.UnitTest.Mocks
{
    class MockAccountRepository : IAccountRepository
    {
        private readonly Dictionary<Guid, Account> accountDictionary;
        public MockAccountRepository()
        {
            accountDictionary = new Dictionary<Guid, Account>();
        }

        public void Add(Account entity) => accountDictionary.Add(entity.Id, entity);

        public void Delete(Account entity) => accountDictionary.Remove(entity.Id);

        public Account Find(Expression<Func<Account, bool>> expression)
        {
            // resolve lambda expression
            BinaryExpression subExpression = (BinaryExpression)expression.Body;
            var keyExpression = Expression.Lambda<Func<string>>(subExpression.Left);
            var valueExpression = Expression.Lambda<Func<string>>(subExpression.Right);
            Func<string> keyFunc = keyExpression.Compile();
            Func<string> valueFunc = valueExpression.Compile();
            var key = keyFunc();
            var value = valueFunc();

            foreach(KeyValuePair<Guid, Account> entry in accountDictionary)
            {
                object instance = entry.Value;
                Type type = instance.GetType();
                System.Reflection.PropertyInfo propertyInfo = type.GetProperty(key);
                var instanceValue = propertyInfo.GetValue(instance);
                if (value.Equals(instanceValue))
                    return instance as Account;
            }

            return null;
        }

        public IEnumerable<Account> FindAll(Expression<Func<Account, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Account Get(Guid key)
        {
            accountDictionary.TryGetValue(key, out Account account);
            return account;
        }
        public Account GetAccountByEmail(string email) => Find(account => account.Email == email);

        public void SaveChanges()
        {
            
        }

        public void Update(Account entity)
        {
            accountDictionary.Add(entity.Id, entity);
        }
    }
}
