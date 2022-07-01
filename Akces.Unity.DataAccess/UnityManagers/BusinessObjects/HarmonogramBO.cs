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
        HarmonogramPosition AddPosition();
        void RemovePosition(HarmonogramPosition harmonogramPosition);
    }

    internal class HarmonogramBO : IHarmonogram
    {
        public Harmonogram Data { get; internal set; }
        private readonly UnityDbContext unityDbContext;

        internal HarmonogramBO(UnityDbContext unityDbContext = null)
        {
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
            var entity = unityDbContext.Entry(harmonogramPosition);
            entity.State = EntityState.Deleted;
            Data.Positions.Remove(entity.Entity);
        }
        public bool Save()
        {
            Validate();

            foreach (var position in Data.Positions.Where(x => x.Account != null)) 
                unityDbContext.Entry(position.Account).State = EntityState.Unchanged;

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
        public void Dispose()
        {
            unityDbContext.Dispose();
        }
    }
}