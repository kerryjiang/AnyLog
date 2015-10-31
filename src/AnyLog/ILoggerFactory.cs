using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyLog
{
    /// <summary>
    /// LogFactory Interface
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Initializes the specified configuration files, we only use the first found one
        /// </summary>
        /// <param name="configFiles">All the potential configuration files, order by priority.</param>
        /// <returns></returns>
        bool Initialize(string[] configFiles);

        /// <summary>
        /// Gets the logger by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        ILog GetLogger(string name);

        /// <summary>
        /// Gets the logger from the specific repository.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        ILog GetLogger(string repositoryName, string name);
    }
}
