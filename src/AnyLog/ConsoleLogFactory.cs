using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace AnyLog
{
    /// <summary>
    /// Console log factory
    /// </summary>
    [Export(typeof(ILogFactory))]
    [ExportMetadata("Name", "ConsoleLog")]
    [ExportMetadata("ConfigFileName", "")]
    public class ConsoleLogFactory : ILogFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogFactory"/> class.
        /// </summary>
        public ConsoleLogFactory()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogFactory"/> class.
        /// </summary>
        /// <param name="configFiles">The configuration files.</param>
        public ConsoleLogFactory(string[] configFiles)
        {

        }

        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ILog GetLog(string name)
        {
            return new ConsoleLog(name);
        }


        /// <summary>
        /// Gets the log from the specific repository.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ILog GetLog(string repositoryName, string name)
        {
            return new ConsoleLog(repositoryName + " -> " + name);
        }
    }
}
