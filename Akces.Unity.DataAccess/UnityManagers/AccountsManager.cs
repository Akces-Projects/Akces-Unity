using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers.BusinessObjects;

namespace Akces.Unity.DataAccess.Managers
{
    public class UnityUsersManager 
    {
        public UnityUser Get(string name)
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var unityUser = unityDbContext.UnityUsers
                    .AsNoTracking()
                    .FirstOrDefault(x => x.UserName == name);

                return unityUser;
            }
        }
    }

    public class AccountsManager
    {
        public List<Account> Get()
        {
            using (var unityDbContext = new UnityDbContext()) 
            {
                var accounts = unityDbContext.Accounts
                    .AsNoTracking()
                    .ToList();

                return accounts;
            }
        }
        public Account Get(object id)
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var account = unityDbContext.Accounts
                    .Include(unityDbContext.GetIncludePaths(typeof(Account)))
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == (int)id);

                return account;
            }
        }
        public T Get<T>(object id) where T : Account, new()
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var configurationName = typeof(T).Name.Replace("Account", "Configuration");

                var account = unityDbContext.Accounts
                    .Include(unityDbContext.GetIncludePaths(typeof(Account)))
                    .OfType<T>()
                    .Include(configurationName)
                    .FirstOrDefault(x => x.Id == (int)id);

                return account;
            }
        }
        public IAccount<T> Create<T>() where T : Account, new()
        {
            var data = new T();
            var bo = new AccountBO<T>(data);            
            return bo;
        }
        public IAccount<Account> Find(Account entity)
        {
            var unityDbContext = new UnityDbContext();

            var account = unityDbContext.Accounts
                .Include(unityDbContext.GetIncludePaths(typeof(Account)))
                .FirstOrDefault(x => x.Id == entity.Id);

            var bo = new AccountBO<Account>(account, unityDbContext);
            return bo;
        }
        public IAccount<T> Find<T>(Account entity) where T : Account, new()
        {
            var unityDbContext = new UnityDbContext();

            var configurationName = typeof(T).Name.Replace("Account", "Configuration");

            var account = unityDbContext.Accounts
                .Include(unityDbContext.GetIncludePaths(typeof(Account)))
                .OfType<T>()
                .Include(configurationName)
                .FirstOrDefault(x => x.Id == entity.Id);

            var bo = new AccountBO<T>(account, unityDbContext);
            return bo;
        }
    }
}