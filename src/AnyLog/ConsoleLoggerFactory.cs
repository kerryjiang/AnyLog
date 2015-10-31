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
    [Export(typeof(ILoggerFactory))]
    [LoggerFactoryMetadata("Console", Priority = 99999)]
    public class ConsoleLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Initializes the specified configuration files, we only use the first found one
        /// </summary>
        /// <param name="configFiles">All the potential configuration files, order by priority.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Initialize(string[] configFiles)
        {
            // Do nothing
            return true;
        }

        /// <summary>
        /// Gets the logger by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ILog GetLogger(string name)
        {
            return new ConsoleLogger(name);
        }


        /// <summary>
        /// Gets the logger from the specific repository.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ILog GetLogger(string repositoryName, string name)
        {
            return new ConsoleLogger(repositoryName + " -> " + name);
        }
    }
}
