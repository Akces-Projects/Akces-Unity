using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;

namespace Akces.Unity.DataAccess.Managers
{
    public class WorkerStatusesManager : IDisposable
    {
        private readonly UnityDbContext unityDbContext;

        public WorkerStatusesManager()
        {
            unityDbContext = new UnityDbContext();
        }

        public WorkerStatus GetCurrent() 
        {
            return unityDbContext.WorkerStatuses.OrderBy(x => x.Created).LastOrDefault();
        }
        public void StopWorker(UnityUser unityUser) 
        {
            using (var context = new UnityDbContext())
            {
                var workerStatus = new WorkerStatus()
                {
                    Created = DateTime.Now,
                    CreatedBy = unityUser.Id,
                    Enabled = false
                };

                context.WorkerStatuses.Add(workerStatus).State = EntityState.Added;
                context.SaveChanges();
            }
        }
        public void StartWorker(UnityUser unityUser)
        {
            using (var context = new UnityDbContext()) 
            {
                var workerStatus = new WorkerStatus()
                {
                    Created = DateTime.Now,
                    CreatedBy = unityUser.Id,
                    Enabled = true
                };

                context.WorkerStatuses.Add(workerStatus).State = EntityState.Added;
                context.SaveChanges();
            }
        }
        public void Dispose() 
        {
            unityDbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}