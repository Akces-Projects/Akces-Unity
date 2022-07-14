using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;

namespace Akces.Unity.DataAccess.Managers.BusinessObjects
{
    public interface IHarmonogram : IDisposable
    {
        Harmonogram Data { get; }
        bool Save();
        bool Delete();
        void Validate();
        void Activate();
        HarmonogramPosition AddPosition();
        void RemovePosition(HarmonogramPosition harmonogramPosition);
    }

    internal class HarmonogramBO : IHarmonogram
    {
        public Harmonogram Data { get; private set; }
        private readonly UnityDbContext unityDbContext;

        internal HarmonogramBO(Harmonogram data, UnityDbContext unityDbContext = null)
        {
            this.Data = data;
            this.unityDbContext = unityDbContext ?? new UnityDbContext();
        }

        public HarmonogramPosition AddPosition()
        {
            var position = new HarmonogramPosition();
            var entity = unityDbContext.Attach(position);
            entity.State = EntityState.Added;
            Data.Positions.Add(entity.Entity);
            return entity.Entity;
        }
        public void RemovePosition(HarmonogramPosition harmonogramPosition)
        {
            if (harmonogramPosition.Id != default)
            {
                var entity = unityDbContext.Entry(harmonogramPosition);
                entity.State = EntityState.Deleted;
            }

            Data.Positions.Remove(harmonogramPosition);
        }
        public bool Save()
        {
            Validate();

            var accounts = unityDbContext.Accounts.ToList();

            foreach (var position in Data.Positions)
            {
                if (position.Account == null)
                    continue;

                position.Account = accounts.FirstOrDefault(x => x.Id == position.Account.Id);
            }

            if (Data.Id == default)
            {
                Data.Created = DateTime.Now;
                Data.Modified = Data.Created;
                unityDbContext.Harmonograms.Add(Data).State = EntityState.Added;
            }
            else
            {
                Data.Modified = DateTime.Now;
                unityDbContext.Harmonograms.Update(Data).State = EntityState.Modified;
            }

            return unityDbContext.SaveChanges() > 0;
        }
        public bool Delete()
        {
            unityDbContext.Harmonograms.Remove(Data).State = EntityState.Deleted;
            return unityDbContext.SaveChanges() > 0;
        }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Data.Name))
                throw new Exception("Należy podać nazwę dla harmonogramu");

            if (unityDbContext.Harmonograms.Any(x => x.Name == Data.Name && x.Id != Data.Id))
                throw new Exception("Istnieje już harmonogram o podanej nazwie");
        }
        public void Activate()
        {
            var harmonogramsManager = new HarmonogramsManager();
            var currentActiveHarmonogram = harmonogramsManager.Get().FirstOrDefault(x => x.Active);

            if (currentActiveHarmonogram != null)
            {
                using (var harmonogramBO = harmonogramsManager.Find(currentActiveHarmonogram))
                {
                    harmonogramBO.Data.Active = false;
                    harmonogramBO.Save();
                }
            }

            using (var harmonogramBO = harmonogramsManager.Find(Data))
            {
                harmonogramBO.Data.Active = true;
                harmonogramBO.Save();
            }

            Data.Active = true;
        }
        public void Dispose()
        {
            unityDbContext.Dispose();
        }
    }
}