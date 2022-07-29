using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers.BusinessObjects;
using System;

namespace Akces.Unity.DataAccess.Managers
{
    public class TaskReportsManager
    {
        public List<TaskReport> Get(DateTime? from = null, DateTime? to = null)
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var taskReports = unityDbContext.TaskReports
                    .Where(x => !from.HasValue || x.Created >= from)
                    .Where(x => !to.HasValue || x.Created <= to)
                    .AsNoTracking()
                    .ToList();

                return taskReports;
            }
        }
        public TaskReport Get(object id)
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var operationReport = unityDbContext.TaskReports
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
                var taskReports = unityDbContext.TaskReports
                    .Where(x => x.HarmonogramPositionId == harmonogramPosition.Id)
                    .Include(unityDbContext.GetIncludePaths(typeof(TaskReport)))
                    .AsNoTracking()
                    .ToList();

                return taskReports;
            }
        }
        public ITaskReport Create(TaskType taskType)
        {
            var data = new TaskReport();
            var bo = new TaskReportBO(data);
            bo.Data.OperationType = taskType;
            return bo;
        }
        public ITaskReport Find(TaskReport entity)
        {
            var unityDbContext = new UnityDbContext();

            var report = unityDbContext.TaskReports
                   .Include(unityDbContext.GetIncludePaths(typeof(TaskReport)))
                   .FirstOrDefault(x => x.Id == entity.Id);

            var bo = new TaskReportBO(report, unityDbContext);
            return bo;
        }
    }
}