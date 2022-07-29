using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Akces.Unity.Models;
using Akces.Unity.Models.Nexo;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Managers.BusinessObjects;

namespace Akces.Unity.DataAccess.Managers
{
    public class AccountFunctionsManager
    {
        internal static readonly Dictionary<int, Dictionary<Guid, Script<object>>> compiledScripts;
         
        internal static Script<object> defaultMatchAssormentScript;
        internal static Script<object> defaultConcludeProductSymbolScript;
        internal static Script<object> defaultCalculateOrderPositionQuantityScript;

        static AccountFunctionsManager()
        {
            compiledScripts = new Dictionary<int, Dictionary<Guid, Script<object>>>();
        }

        public void Initialize() 
        {
            defaultMatchAssormentScript = InitScript(AccountFunctionType.MatchAssormentFunction.DefaultScript, typeof(MatchAssortmentParameters));
            defaultConcludeProductSymbolScript = InitScript(AccountFunctionType.ConcludeProductSymbolFunction.DefaultScript, typeof(ConcludeProductSymbolParameters));
            defaultCalculateOrderPositionQuantityScript = InitScript(AccountFunctionType.CalculateOrderPositionQuantityFunction.DefaultScript, typeof(CalculateOrderPositionQuantityParameters));

            foreach (var accountFunction in Get())
            {
                Script<object> script = null;

                if (accountFunction.AccountFunctionType.Id == AccountFunctionType.MatchAssormentFunction.Id)
                    script = InitScript(accountFunction.Script, typeof(MatchAssortmentParameters));
                else if (accountFunction.AccountFunctionType.Id == AccountFunctionType.ConcludeProductSymbolFunction.Id)
                    script = InitScript(accountFunction.Script, typeof(ConcludeProductSymbolParameters));
                else if (accountFunction.AccountFunctionType.Id == AccountFunctionType.CalculateOrderPositionQuantityFunction.Id)
                    script = InitScript(accountFunction.Script, typeof(CalculateOrderPositionQuantityParameters));

                if (!compiledScripts.ContainsKey(accountFunction.Account.Id)) 
                    compiledScripts.Add(accountFunction.Account.Id, new Dictionary<Guid, Script<object>>());

                var dict = compiledScripts[accountFunction.Account.Id];
                dict.Add(accountFunction.AccountFunctionType.Id, script);
            }
        }
        public List<AccountFunction> Get()
        {
            using (var unityDbContext = new UnityDbContext())
            {
                var accountFunctions = unityDbContext.AccountFunctions
                    .Include(unityDbContext.GetIncludePaths(typeof(AccountFunction)))
                    .AsNoTracking()
                    .ToList();

                return accountFunctions;
            }
        }
        public IAccountFunction Create()
        {
            var data = new AccountFunction();
            var bo = new AccountFunctionBO(data);
            return bo;
        }
        public IAccountFunction Find(AccountFunction entity)
        {
            var unityDbContext = new UnityDbContext();

            var accountFunction = unityDbContext.AccountFunctions
                .Include(unityDbContext.GetIncludePaths(typeof(AccountFunction)))
                .FirstOrDefault(x => x.Id == entity.Id);

            var bo = new AccountFunctionBO(accountFunction, unityDbContext);
            return bo;
        }

        [Obsolete]
        public Func<Product, Assortment, bool> GetMatchAssormentMethod(int accountId)
        {
            return (p, a) =>
            {
                var parameters = new MatchAssortmentParameters() { Assortment = a, Product = p };
                Script<object> script = null;
                var functionTypeId = AccountFunctionType.MatchAssormentFunction.Id;

                if (!compiledScripts.ContainsKey(accountId) || !compiledScripts[accountId].ContainsKey(AccountFunctionType.MatchAssormentFunction.Id))
                    script = defaultMatchAssormentScript;
                else
                    script = compiledScripts[accountId][functionTypeId];

                var scriptState = script.RunAsync(globals: parameters).Result;
                return (bool)scriptState.ReturnValue;
            };
        }
        public Func<Product, string> GetConcludeProductSymbolMethod(int accountId)
        {
            return (p) =>
            {
                var parameters = new ConcludeProductSymbolParameters() { Product = p };
                Script<object> script = null;
                var functionTypeId = AccountFunctionType.ConcludeProductSymbolFunction.Id;

                if (!compiledScripts.ContainsKey(accountId) || !compiledScripts[accountId].ContainsKey(AccountFunctionType.ConcludeProductSymbolFunction.Id))
                    script = defaultConcludeProductSymbolScript;
                else
                    script = compiledScripts[accountId][functionTypeId];

                var scriptState = script.RunAsync(globals: parameters).Result;
                return (string)scriptState.ReturnValue;
            };
        }
        public Func<Product, decimal> GetCalculateOrderPositionQuantityScriptMethod(int accountId)
        {
            return (p) =>
            {
                var parameters = new CalculateOrderPositionQuantityParameters() { Product = p };
                Script<object> script = null;
                var functionTypeId = AccountFunctionType.CalculateOrderPositionQuantityFunction.Id;

                if (!compiledScripts.ContainsKey(accountId) || !compiledScripts[accountId].ContainsKey(AccountFunctionType.CalculateOrderPositionQuantityFunction.Id))
                    script = defaultCalculateOrderPositionQuantityScript;
                else
                    script = compiledScripts[accountId][functionTypeId];

                var scriptState = script.RunAsync(globals: parameters).Result;
                return (decimal)scriptState.ReturnValue;
            };
        }
        internal Script<object> InitScript(AccountFunction accountFunction)
        {
            Type globalsType = null;

            if (accountFunction.AccountFunctionType.Id == AccountFunctionType.MatchAssormentFunction.Id)
                globalsType = typeof(MatchAssortmentParameters);
            else if (accountFunction.AccountFunctionType.Id == AccountFunctionType.ConcludeProductSymbolFunction.Id) 
                globalsType = typeof(ConcludeProductSymbolParameters);
            else if (accountFunction.AccountFunctionType.Id == AccountFunctionType.CalculateOrderPositionQuantityFunction.Id) 
                globalsType = typeof(CalculateOrderPositionQuantityParameters);

            var scriptOptions = ScriptOptions.Default;
            var mscorlib = typeof(object).GetTypeInfo().Assembly;
            var systemCore = typeof(Enumerable).GetTypeInfo().Assembly;
            var references = new[] { mscorlib, systemCore };
            scriptOptions = scriptOptions.AddReferences(references);

            using (var interactiveLoader = new InteractiveAssemblyLoader())
            {
                foreach (var reference in references)
                    interactiveLoader.RegisterDependency(reference);

                scriptOptions = scriptOptions.AddImports("System");
                scriptOptions = scriptOptions.AddImports("System.Linq");
                scriptOptions = scriptOptions.AddImports("System.Collections.Generic");
                var script = CSharpScript.Create(accountFunction.Script, scriptOptions, globalsType);
                script.Compile();
                return script;
            }
        }
        internal Script<object> InitScript(string scriptText, Type globalsType)
        {
            var scriptOptions = ScriptOptions.Default;
            var mscorlib = typeof(object).GetTypeInfo().Assembly;
            var systemCore = typeof(Enumerable).GetTypeInfo().Assembly;
            var references = new[] { mscorlib, systemCore };
            scriptOptions = scriptOptions.AddReferences(references);

            using (var interactiveLoader = new InteractiveAssemblyLoader())
            {
                foreach (var reference in references)
                    interactiveLoader.RegisterDependency(reference);

                scriptOptions = scriptOptions.AddImports("System");
                scriptOptions = scriptOptions.AddImports("System.Linq");
                scriptOptions = scriptOptions.AddImports("System.Collections.Generic");
                var script = CSharpScript.Create(scriptText, scriptOptions, globalsType);
                script.Compile();
                return script;
            }
        }
    }

    public class MatchAssortmentParameters
    {
        public Product Product { get; set; }
        public Assortment Assortment { get; set; }
    }
    public class ConcludeProductSymbolParameters
    {
        public Product Product { get; set; }
    }
    public class CalculateOrderPositionQuantityParameters
    {
        public Product Product { get; set; }
    }
}
