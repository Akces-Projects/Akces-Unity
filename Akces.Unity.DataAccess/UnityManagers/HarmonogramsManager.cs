using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers.BusinessObjects;

namespace Akces.Unity.DataAccess.Managers
{
    public class HarmonogramsManager
    {
        public List<Harmonogram> Get()
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var harmonograms = unityDbContext.Harmonograms
                    .Include(x => x.Positions)
                    .AsNoTracking()
                    .ToList();

                return harmonograms;
            }
        }
        public Harmonogram Get(object id)
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var harmonogram = unityDbContext.Harmonograms
                    .Include(unityDbContext.GetIncludePaths(typeof(Harmonogram)))
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == (int)id);

                return harmonogram;
            }
        }
        public IHarmonogram Create()
        {
            var bo = new HarmonogramBO();
            bo.Data = new Harmonogram();
            return bo;
        }
        public IHarmonogram Find(Harmonogram entity)
        {
            var unityDbContext = new UnityDbContext();

            var harmonogram = unityDbContext.Harmonograms
                   .Include(unityDbContext.GetIncludePaths(typeof(Harmonogram)))
                   .FirstOrDefault(x => x.Id == entity.Id);

            var bo = new HarmonogramBO(unityDbContext);
            bo.Data = harmonogram;
            return bo;
        }

        public bool SaveHarmonogramPosition(HarmonogramPosition harmonogramPosition) 
        {
            var unityDbContext = new UnityDbContext();
            var position = unityDbContext.Set<HarmonogramPosition>().Find(harmonogramPosition.Id);

            if (position == null)
                return false;

            position.LastLaunchTime = harmonogramPosition.LastLaunchTime;

            unityDbContext.Update(position).State = EntityState.Modified;
            var result = unityDbContext.SaveChanges();
            unityDbContext.Dispose();
            return result > 0;
        }
    }
}