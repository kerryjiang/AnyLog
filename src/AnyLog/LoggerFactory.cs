using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;

namespace AnyLog
{
    /// <summary>
    /// the static logfactory allow user to configurate and get the logfactory
    /// </summary>
    public static class LoggerFactory
    {
        private static ILoggerFactory m_Current;

        /// <summary>
        /// Gets the current log factory which is in use in the system.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static ILoggerFactory Current
        {
            get { return m_Current; }
        }

        /// <summary>
        /// Configurates the specified logger factory name.
        /// </summary>
        /// <param name="loggerFactoryName">Name of the logger factory.</param>
        public static void Configurate(string loggerFactoryName = null)
        {
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(LoggerFactory).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, "*.dll"));

            //For website, all assemblies locate in bin directory
            var binDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

            if(Directory.Exists(binDir))
                catalog.Catalogs.Add(new DirectoryCatalog(binDir, "*.dll"));

            //Create the CompositionContainer with the parts in the catalog
            var container = new CompositionContainer(catalog);

            Configurate(container, loggerFactoryName);
        }

        /// <summary>
        /// Configurates the specified export provider.
        /// </summary>
        /// <param name="exportProvider">The export provider.</param>
        /// <param name="loggerFactoryName">Name of the logger factory.</param>
        /// <exception cref="System.ArgumentNullException">exportProvider</exception>
        public static void Configurate(ExportProvider exportProvider, string loggerFactoryName = null)
        {
            if (exportProvider == null)
                throw new ArgumentNullException("exportProvider");

            var lazyLogFactory = exportProvider.GetExports<ILoggerFactory, ILoggerFactoryMetadata>()
                    .Where(f => string.IsNullOrEmpty(loggerFactoryName) || f.Metadata.Name.Equals(loggerFactoryName, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(f => f.Metadata.Priority)
                    .FirstOrDefault();

            if (lazyLogFactory == null)
                return;

            var metadata = lazyLogFactory.Metadata;
            var configFiles = new List<string>();

            if (!string.IsNullOrEmpty(metadata.ConfigFileName))
            {
                configFiles.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, metadata.ConfigFileName));
                configFiles.Add(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config"), metadata.ConfigFileName));
            }

            var facotry = lazyLogFactory.Value;

            if (!facotry.Initialize(configFiles.ToArray()))
                return;

            m_Current = facotry;
        }
    }
}
