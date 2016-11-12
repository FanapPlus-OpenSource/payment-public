using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.IO;
using System.Linq;

namespace PaymentSample.Common
{
    public static class AppsOnContainer
    {
        public static object SyncObject = new object();
        private static CompositionContainer _container;


        public static CompositionContainer Instance
            => _container ?? Initialize(AppDomain.CurrentDomain.BaseDirectory,
                   Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"));

        #region IContainer Members

        public static CompositionContainer Initialize(params object[] parameters)
        {
            if (!IsInitialized)
                lock (SyncObject)
                {
                    if (!IsInitialized)
                    {
                        var aggregateCatalog =
                            new AggregateCatalog(parameters
                                .Where(m => Directory.Exists(m.ToString()))
                                .Select(m => new DirectoryCatalog(m.ToString())));
                        _container = new CompositionContainer(aggregateCatalog, true);
                        _container.Compose(new CompositionBatch());
                        IsInitialized = true;
                    }
                }
            return _container;
        }


        public static bool IsInitialized { get; private set; }

        public static IList<T> ResolveAll<T>(this CompositionContainer container)
        {
            return GetExportedTypes<T>(container)
                .Select(m =>
                {
                    var attribute =
                        m.GetCustomAttributes(typeof(ExportAttribute), true)
                            .OfType<ExportAttribute>()
                            .FirstOrDefault();
                    if (attribute != null)
                        return attribute.ContractName;
                    return "";
                })
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Select(container.GetExportedValueOrDefault<T>)
                .Where(m => !Equals(default(T), m))
                .ToList();
        }

        public static IEnumerable<Type> GetExportedTypes<T>(this CompositionContainer container)
        {
            return container.Catalog.Parts
                .Select(part => ComposablePartExportType<T>(part))
                .Where(t => t != null);
        }

        private static Type ComposablePartExportType<T>(ComposablePartDefinition part)
        {
            if (part.ExportDefinitions.Any(
                def => def.Metadata.ContainsKey("ExportTypeIdentity") &&
                       def.Metadata["ExportTypeIdentity"].Equals(typeof(T).FullName)))
                return ReflectionModelServices.GetPartType(part).Value;
            return null;
        }

        #endregion
    }
}