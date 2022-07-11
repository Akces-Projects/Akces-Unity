using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;

namespace Akces.Unity.DataAccess.Managers.BusinessObjects
{
    public interface IUnityUser : IDisposable
    {
        UnityUser Data { get; }
        bool Save();
        bool Delete();
        void Validate();
    }

    internal class UnityUserBO : IUnityUser 
    {
        public UnityUser Data { get; private set; }
        private readonly UnityDbContext unityDbContext;

        internal UnityUserBO(UnityUser data, UnityDbContext unityDbContext = null)
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
                unityDbContext.UnityUsers.Add(Data).State = EntityState.Added;
            }
            else
            {
                Data.Modified = DateTime.Now;
                unityDbContext.UnityUsers.Update(Data).State = EntityState.Modified;
            }

            return unityDbContext.SaveChanges() > 0;
        }
        public bool Delete()
        {
            unityDbContext.UnityUsers.Remove(Data).State = EntityState.Deleted;
            return unityDbContext.SaveChanges() > 0;
        }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Data.Login))
                throw new Exception("Należy podać nazwę użytkownika");

            if (unityDbContext.UnityUsers.Any(x => x.Login == Data.Login && x.Id != Data.Id))
                throw new Exception("Istnieje już użytkownik o podanej nazwie");
        }
        public void Dispose()
        {
            unityDbContext.Dispose();
        }
    }
}