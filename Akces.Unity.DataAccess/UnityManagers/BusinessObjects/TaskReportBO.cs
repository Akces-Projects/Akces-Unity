using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;

namespace Akces.Unity.DataAccess.Managers.BusinessObjects
{
    public interface ITaskReport : IDisposable
    {
        TaskReport Data { get; }
        bool Save();
        bool Delete();
        void Validate();
        TaskReportPosition AddInfo(string objectName, string description);
        TaskReportPosition AddWarn(string objectName, string description);
        TaskReportPosition AddError(string objectName, string description);
        void RemovePosition(TaskReportPosition operationReportPosition);
    }

    internal class TaskReportBO : ITaskReport
    {
        public TaskReport Data { get; private set; }
        private readonly UnityDbContext unityDbContext;

        internal TaskReportBO(TaskReport data, UnityDbContext unityDbContext = null)
        {
            this.Data = data;
            this.unityDbContext = unityDbContext ?? new UnityDbContext();
        }

        public TaskReportPosition AddInfo(string objectName, string description)
        {
            var position = new TaskReportPosition()
            {
                ObjectName = objectName,
                Description = description,
                Type = ReportPositionType.Info
            };

            var entity = unityDbContext.Attach(position);
            entity.State = EntityState.Added;
            Data.Positions.Add(entity.Entity);
            return entity.Entity;
        }
        public TaskReportPosition AddWarn(string objectName, string description)
        {
            var position = new TaskReportPosition()
            {
                ObjectName = objectName,
                Description = description,
                Type = ReportPositionType.Warn
            };

            var entity = unityDbContext.Attach(position);
            entity.State = EntityState.Added;
            Data.Positions.Add(entity.Entity);
            return entity.Entity;
        }
        public TaskReportPosition AddError(string objectName, string description)
        {
            var position = new TaskReportPosition()
            {
                ObjectName = objectName,
                Description = description,
                Type = ReportPositionType.Error
            };

            var entity = unityDbContext.Attach(position);
            entity.State = EntityState.Added;
            Data.Positions.Add(entity.Entity);
            return entity.Entity;
        }
        public void RemovePosition(TaskReportPosition operationReportPosition)
        {
            var entity = unityDbContext.Entry(operationReportPosition);
            entity.State = EntityState.Deleted;
            Data.Positions.Remove(entity.Entity);
        }
        public bool Save()
        {
            Validate();

            Data.PositionsCount = Data.Positions.Count;
            Data.InfosCount = Data.Positions.Count(x => x.Type == ReportPositionType.Info);
            Data.WarningsCount = Data.Positions.Count(x => x.Type == ReportPositionType.Warn);
            Data.ErrorsCount = Data.Positions.Count(x => x.Type == ReportPositionType.Error);

            if (Data.Id == default)
            {
                Data.Created = DateTime.Now;
                unityDbContext.TaskReports.Add(Data).State = EntityState.Added;
            }
            else
            {
                unityDbContext.TaskReports.Update(Data).State = EntityState.Modified;
            }

            return unityDbContext.SaveChanges() > 0;
        }
        public bool Delete()
        {
            unityDbContext.TaskReports.Remove(Data).State = EntityState.Deleted;
            return unityDbContext.SaveChanges() > 0;
        }
        public void Validate()
        {
        }
        public void Dispose()
        {
            unityDbContext.Dispose();
        }
    }
}