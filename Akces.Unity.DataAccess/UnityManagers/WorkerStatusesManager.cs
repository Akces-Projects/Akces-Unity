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
            var workerStatus = new WorkerStatus()
            {
                Created = DateTime.Now,
                CreatedBy = unityUser.Id,
                Enabled = false
            };

            unityDbContext.WorkerStatuses.Add(workerStatus).State = EntityState.Added;
            unityDbContext.SaveChanges();
        }
        public void StartWorker(UnityUser unityUser)
        {
            var workerStatus = new WorkerStatus()
            {
                Created = DateTime.Now,
                CreatedBy = unityUser.Id,
                Enabled = true
            };

            unityDbContext.WorkerStatuses.Add(workerStatus).State = EntityState.Added;
            unityDbContext.SaveChanges();
        }
        public void Dispose() 
        {
            unityDbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}