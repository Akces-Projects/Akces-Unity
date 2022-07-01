using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers.BusinessObjects;

namespace Akces.Unity.DataAccess.Managers
{
    public class OperationReportsManager
    {
        public List<OperationReport> Get()
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var accounts = unityDbContext.OperationReports
                    .AsNoTracking()
                    .ToList();

                return accounts;
            }
        }
        public OperationReport Get(object id)
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var operationReport = unityDbContext.OperationReports
                    .Include(unityDbContext.GetIncludePaths(typeof(OperationReport)))
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == (int)id);

                return operationReport;
            }
        }
        public List<OperationReport> GetForHarmonogramPosition(HarmonogramPosition harmonogramPosition)
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var operationReports = unityDbContext.OperationReports
                    .Where(x => x.HarmonogramPositionId == harmonogramPosition.Id)
                    .Include(unityDbContext.GetIncludePaths(typeof(OperationReport)))
                    .AsNoTracking()
                    .ToList();

                return operationReports;
            }
        }
        public IOperationReport Create(OperationType operationType)
        {
            var bo = new OperationReportBO();
            bo.Data = new OperationReport();
            bo.Data.OperationType = operationType;
            return bo;
        }
    }
}