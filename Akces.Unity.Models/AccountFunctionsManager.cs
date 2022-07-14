using Akces.Unity.Models.Nexo;
using Akces.Unity.Models.SaleChannels;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Akces.Unity.Models
{
    public class AccountFunctionsManager 
    {
        private readonly Dictionary<int, Script<bool>> matchAssormentScripts;
        private readonly Dictionary<int, Script<string>> concludeProductSymbolScripts;
        private readonly Dictionary<int, Script<decimal>> calculateOrderPositionQuantityScripts;

        public AccountFunctionsManager()
        {
            matchAssormentScripts = new Dictionary<int, Script<bool>>();
            concludeProductSymbolScripts = new Dictionary<int, Script<string>>();
            calculateOrderPositionQuantityScripts = new Dictionary<int, Script<decimal>>();
        }

        public Func<Product, Assortment, bool> GetMatchAssormentMethod(int accountId)
        {
            return (p, a) =>
            {
                var parameters = new MatchAssortmentParameters() { Assortment = a, Product = p };
                var scriptState = matchAssormentScripts[accountId].RunAsync(globals: parameters).Result;
                return scriptState.ReturnValue;
            };
        }
        public Func<Product, string> GetConcludeProductSymbolScriptMethod(int accountId)
        {
            return (p) =>
            {
                var parameters = new ConcludeProductSymbolParameters() { Product = p };
                var scriptState = concludeProductSymbolScripts[accountId].RunAsync(globals: parameters).Result;
                return scriptState.ReturnValue;
            };
        }
        public Func<Product, decimal> GetCalculateOrderPositionQuantityScriptMethod(int accountId)
        {
            return (p) =>
            {
                var parameters = new CalculateOrderPositionQuantityParameters() { Product = p };
                var scriptState = calculateOrderPositionQuantityScripts[accountId].RunAsync(globals: parameters).Result;
                return scriptState.ReturnValue;
            };
        }

        private void InitScripts(Account account) 
        {
            var scriptOptions = ScriptOptions.Default;

            // Add reference to mscorlib
            var mscorlib = typeof(object).GetTypeInfo().Assembly;
            var systemCore = typeof(Enumerable).GetTypeInfo().Assembly;

            var references = new[] { mscorlib, systemCore };
            scriptOptions = scriptOptions.AddReferences(references);

            using (var interactiveLoader = new InteractiveAssemblyLoader())
            {
                foreach (var reference in references)
                {
                    interactiveLoader.RegisterDependency(reference);
                }

                // Add namespaces
                scriptOptions = scriptOptions.AddImports("System");
                scriptOptions = scriptOptions.AddImports("System.Linq");
                scriptOptions = scriptOptions.AddImports("System.Math");
                scriptOptions = scriptOptions.AddImports("System.Collections.Generic");

                // Initialize script with custom interactive assembly loader
                var matchAssormentScript = CSharpScript.Create<bool>(account.MatchAssormentScript, scriptOptions, typeof(MatchAssortmentParameters));
                var concludeProductSymbolScript = CSharpScript.Create<string>(account.ConcludeProductSymbolScript, scriptOptions, typeof(ConcludeProductSymbolParameters));
                var calculateOrderPositionQuantityScript = CSharpScript.Create<decimal>(account.CalculateOrderPositionQuantityScript, scriptOptions, typeof(CalculateOrderPositionQuantityParameters));

                matchAssormentScript.Compile();
                concludeProductSymbolScript.Compile();
                calculateOrderPositionQuantityScript.Compile();

                if (!matchAssormentScripts.ContainsKey(account.Id)) 
                    matchAssormentScripts.Add(account.Id, matchAssormentScript);

                if (!concludeProductSymbolScripts.ContainsKey(account.Id)) 
                    concludeProductSymbolScripts.Add(account.Id, concludeProductSymbolScript);

                if (!calculateOrderPositionQuantityScripts.ContainsKey(account.Id)) 
                    calculateOrderPositionQuantityScripts.Add(account.Id, calculateOrderPositionQuantityScript);
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
}
