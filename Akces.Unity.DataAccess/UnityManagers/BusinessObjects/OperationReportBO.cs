using System;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using System.Linq;

namespace Akces.Unity.DataAccess.Managers.BusinessObjects
{
    public interface IOperationReport : IDisposable
    {
        OperationReport Data { get; }
        bool Save();
        bool Delete();
        void Validate();
        OperationReportPosition AddInfo(string objectName, string description);
        OperationReportPosition AddWarn(string objectName, string description);
        OperationReportPosition AddError(string objectName, string description);
        void RemovePosition(OperationReportPosition operationReportPosition);
    }

    internal class OperationReportBO : IOperationReport
    {
        public OperationReport Data { get; internal set; }
        private readonly UnityDbContext unityDbContext;

        internal OperationReportBO(UnityDbContext unityDbContext = null)
        {
            this.unityDbContext = unityDbContext ?? new UnityDbContext();
        }

        public OperationReportPosition AddInfo(string objectName, string description)
        {
            var position = new OperationReportPosition()
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
        public OperationReportPosition AddWarn(string objectName, string description)
        {
            var position = new OperationReportPosition()
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
        public OperationReportPosition AddError(string objectName, string description)
        {
            var position = new OperationReportPosition()
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
        public void RemovePosition(OperationReportPosition operationReportPosition)
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
                unityDbContext.OperationReports.Add(Data).State = EntityState.Added;
            }
            else
            {
                unityDbContext.OperationReports.Update(Data).State = EntityState.Modified;
            }

            return unityDbContext.SaveChanges() > 0;
        }
        public bool Delete()
        {
            unityDbContext.OperationReports.Remove(Data).State = EntityState.Deleted;
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