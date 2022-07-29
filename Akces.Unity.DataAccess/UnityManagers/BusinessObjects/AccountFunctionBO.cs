using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Scripting;

namespace Akces.Unity.DataAccess.Managers.BusinessObjects
{
    public interface IAccountFunction : IDisposable
    {
        AccountFunction Data { get; }
        bool Save();
        bool Delete();
        void Validate();
        void InitScript();
    }

    internal class AccountFunctionBO : IAccountFunction
    {
        public AccountFunction Data { get; private set; }
        private readonly UnityDbContext unityDbContext;

        internal AccountFunctionBO(AccountFunction data, UnityDbContext unityDbContext = null)
        {
            this.Data = data;
            this.unityDbContext = unityDbContext ?? new UnityDbContext();
        }

        public bool Save()
        {
            Validate();

            Data.AccountFunctionType = unityDbContext.AccountFunctionTypes.First(x => x.Id == Data.AccountFunctionType.Id);
            Data.Account = unityDbContext.Accounts.First(x => x.Id == Data.Account.Id);

            if (Data.Id == default)
            {
                Data.Created = DateTime.Now;
                Data.Modified = Data.Created;
                unityDbContext.AccountFunctions.Add(Data).State = EntityState.Added;
            }
            else
            {
                Data.Modified = DateTime.Now;
                unityDbContext.AccountFunctions.Update(Data).State = EntityState.Modified;
            }

            return unityDbContext.SaveChanges() > 0;
        }
        public bool Delete()
        {
            unityDbContext.AccountFunctions.Remove(Data).State = EntityState.Deleted;
            var result = unityDbContext.SaveChanges() > 0;

            if (result) 
            {
                var dict = AccountFunctionsManager.compiledScripts[Data.Account.Id];
                dict.Remove(Data.AccountFunctionType.Id);
            }

            return result;
        }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Data.Name))
                throw new Exception("Należy podać nazwę dla funkcji");

            if (unityDbContext.AccountFunctions.Any(x => x.Name == Data.Name && x.Id != Data.Id))
                throw new Exception("Istnieje już funkcja o podanej nazwie");

            if (Data.AccountFunctionType == null)
                throw new Exception("Należy wskazać rodzaj funkcji");

            if (Data.Account == null)
                throw new Exception("Należy wskazać konto");

            if (unityDbContext.AccountFunctions.Any(x => 
                x.Account.Id == Data.Account.Id && 
                x.AccountFunctionType.Id == Data.AccountFunctionType.Id && 
                x.Id != Data.Id))

                throw new Exception("Istnieje już funkcja tego rodzaju dla wybranego konta." + Environment.NewLine + "Należy edytować lub usunąć istniejącą funkcję.");          
        }
        public void Dispose()
        {
            unityDbContext?.Dispose();
        }
        public void InitScript()
        {
            var manager = new AccountFunctionsManager();
            var script = manager.InitScript(Data);

            if (!AccountFunctionsManager.compiledScripts.ContainsKey(Data.Account.Id))
                AccountFunctionsManager.compiledScripts.Add(Data.Account.Id, new Dictionary<Guid, Script<object>>());

            var dict = AccountFunctionsManager.compiledScripts[Data.Account.Id];

            if (dict.ContainsKey(Data.AccountFunctionType.Id))
                dict[Data.AccountFunctionType.Id] = script;
            else
                dict.Add(Data.AccountFunctionType.Id, script);
        }
    }
}