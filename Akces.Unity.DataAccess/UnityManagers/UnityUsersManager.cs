using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers.BusinessObjects;

namespace Akces.Unity.DataAccess.Managers
{
    public class UnityUsersManager 
    {
        public List<UnityUser> Get()
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var unityUsers = unityDbContext.UnityUsers
                    .Include(unityDbContext.GetIncludePaths(typeof(UnityUser)))
                    .AsNoTracking()
                    .ToList();

                return unityUsers;
            }
        }
        public UnityUser Get(object id)
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var unityUser = unityDbContext.UnityUsers
                    .Include(unityDbContext.GetIncludePaths(typeof(UnityUser)))
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == (int)id);

                return unityUser;
            }
        }
        public IUnityUser Create()
        {
            var data = new UnityUser
            {
                Authorisations = Modules.List.Select(x => new Authorisation()
                {
                    Module = x,
                    AuthorisationType = AuthorisationType.AllowRead
                }).ToList()
            };

            var bo = new UnityUserBO(data);
            return bo;
        }
        public IUnityUser Find(UnityUser entity) 
        {
            var unityDbContext = new UnityDbContext();

            var data = unityDbContext.UnityUsers
                .Include(unityDbContext.GetIncludePaths(typeof(UnityUser)))
                .FirstOrDefault(x => x.Id == entity.Id);

            var bo = new UnityUserBO(data, unityDbContext);
            return bo;
        }
    }
}