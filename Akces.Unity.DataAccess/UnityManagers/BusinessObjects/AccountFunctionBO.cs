using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;

namespace Akces.Unity.DataAccess.Managers.BusinessObjects
{
    public interface IAccountFunction : IDisposable
    {
        AccountFunction Data { get; }
        bool Save();
        bool Delete();
        void Validate();
    }

    internal class AccountFunctionBO : IAccountFunction
    {
        public AccountFunction Data { get; private set; }
        private readonly UnityDbContext unityDbContext;

        internal AccountFunctionBO(AccountFunction data, UnityDbContext unityDbContext = null)
        {
            this.Data = data;
            this.unityDbContext = unityDbContext ?? new UnityDbContext();
        }

        public bool Save()
        {
            Validate();

            if (Data.Id == default)
            {
                Data.Created = DateTime.Now;
                Data.Modified = Data.Created;
                unityDbContext.AccountFunctions.Add(Data).State = EntityState.Added;
            }
            else
            {
                Data.Modified = DateTime.Now;
                unityDbContext.AccountFunctions.Update(Data).State = EntityState.Modified;
            }

            return unityDbContext.SaveChanges() > 0;
        }
        public bool Delete()
        {
            unityDbContext.AccountFunctions.Remove(Data).State = EntityState.Deleted;
            return unityDbContext.SaveChanges() > 0;
        }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Data.Name))
                throw new Exception("Należy podać nazwę funkcji");

            if (unityDbContext.AccountFunctions.Any(x => x.Name == Data.Name && x.Id != Data.Id))
                throw new Exception("Istnieje już funkcja o podanej nazwie");
        }
        public void Dispose()
        {
            unityDbContext.Dispose();
        }
    }
}