using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;

namespace Akces.Unity.DataAccess.Managers.BusinessObjects
{
    public interface IAccount<T> : IDisposable where T : Account
    {
        T Data { get; }
        bool Save();
        bool Delete();
        void Validate();
    }

    internal class AccountBO<T> : IAccount<T> where T : Account
    {
        public T Data { get; private set; }
        private readonly UnityDbContext unityDbContext;

        internal AccountBO(T data, UnityDbContext unityDbContext = null)
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
                unityDbContext.Accounts.Add(Data).State = EntityState.Added;
            }
            else
            {
                Data.Modified = DateTime.Now;
                unityDbContext.Accounts.Update(Data).State = EntityState.Modified;
            }

            return unityDbContext.SaveChanges() > 0;
        }
        public bool Delete()
        {
            var harmonograms = unityDbContext.Harmonograms
                .Where(h => h.Positions.Any(p => p.Account.Id == Data.Id))
                .Select(x => x.Name)
                .ToArray();

            if (harmonograms.Any())
                throw new Exception($"Wskazane konto jest wykorzystywane przez harmonogram: " +
                    $"{Environment.NewLine}{Environment.NewLine}" +
                    $"{string.Join(", ", harmonograms)} " +
                    $"{Environment.NewLine }{Environment.NewLine}" +
                    "Przed usunięciem konta, należy usunąć wszystkie jego wystąpienia");

            unityDbContext.Accounts.Remove(Data).State = EntityState.Deleted;
            return unityDbContext.SaveChanges() > 0;
        }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Data.Name))
                throw new Exception("Należy podać nazwę dla konta");

            if (unityDbContext.Accounts.Any(x => x.Name == Data.Name && x.Id != Data.Id))
                throw new Exception("Istnieje już konto o podanej nazwie");
        }
        public void Dispose() 
        {
            unityDbContext.Dispose();
        }
    }
}