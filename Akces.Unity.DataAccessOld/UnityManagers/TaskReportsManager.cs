using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers.BusinessObjects;

namespace Akces.Unity.DataAccess.Managers
{
    public class TaskReportsManager
    {
        public List<TaskReport> Get()
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var accounts = unityDbContext.OperationReports
                    .AsNoTracking()
                    .ToList();

                return accounts;
            }
        }
        public TaskReport Get(object id)
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var operationReport = unityDbContext.OperationReports
                    .Include(unityDbContext.GetIncludePaths(typeof(TaskReport)))
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == (int)id);

                return operationReport;
            }
        }
        public List<TaskReport> GetForHarmonogramPosition(HarmonogramPosition harmonogramPosition)
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var operationReports = unityDbContext.OperationReports
                    .Where(x => x.HarmonogramPositionId == harmonogramPosition.Id)
                    .Include(unityDbContext.GetIncludePaths(typeof(TaskReport)))
                    .AsNoTracking()
                    .ToList();

                return operationReports;
            }
        }
        public ITaskReport Create(OperationType operationType)
        {
            var data = new TaskReport();
            var bo = new TaskReportBO(data);
            bo.Data.OperationType = operationType;
            return bo;
        }
        public ITaskReport Find(TaskReport entity)
        {
            var unityDbContext = new UnityDbContext();

            var report = unityDbContext.OperationReports
                   .Include(unityDbContext.GetIncludePaths(typeof(TaskReport)))
                   .FirstOrDefault(x => x.Id == entity.Id);

            var bo = new TaskReportBO(report, unityDbContext);
            return bo;
        }
    }
}