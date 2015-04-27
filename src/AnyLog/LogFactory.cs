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
    public static class LogFactory
    {
        private static ILogFactory m_Current;

        /// <summary>
        /// Gets the current log factory which is in use in the system.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static ILogFactory Current
        {
            get { return m_Current; }
        }

        /// <summary>
        /// Configurates the specified log factory name.
        /// </summary>
        /// <param name="logFactoryName">Name of the log factory.</param>
        public static void Configurate(string logFactoryName = null)
        {
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(LogFactory).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, "*.dll"));

            //Create the CompositionContainer with the parts in the catalog
            var container = new CompositionContainer(catalog);

            Configurate(container, logFactoryName);
        }

        /// <summary>
        /// Configurates the specified export provider.
        /// </summary>
        /// <param name="exportProvider">The export provider.</param>
        /// <param name="logFactoryName">Name of the log factory.</param>
        /// <exception cref="System.ArgumentNullException">exportProvider</exception>
        public static void Configurate(ExportProvider exportProvider, string logFactoryName = null)
        {
            if (exportProvider == null)
                throw new ArgumentNullException("exportProvider");

            var lazyLogFactory = string.IsNullOrEmpty(logFactoryName)
                ? exportProvider.GetExports<ILogFactory, ILogFactoryMetadata>().FirstOrDefault()
                : exportProvider.GetExports<ILogFactory, ILogFactoryMetadata>().Where(
                    f => f.Metadata.Name.Equals(logFactoryName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (lazyLogFactory == null)
                return;

            var metadata = lazyLogFactory.Metadata;
            var configFiles = new List<string>();
            configFiles.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, metadata.ConfigFileName));
            configFiles.Add(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config"), metadata.ConfigFileName));

            var facotry = lazyLogFactory.Value;

            if (!facotry.Initialize(configFiles.ToArray()))
                return;

            m_Current = facotry;
        }
    }
}
