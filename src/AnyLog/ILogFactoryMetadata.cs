using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyLog
{
    /// <summary>
    /// Log factory metadata interface
    /// </summary>
    public interface ILogFactoryMetadata
    {
        /// <summary>
        /// Gets the name of the log factory.
        /// </summary>
        /// <value>
        /// The log factory name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the name of the configuration file.
        /// </summary>
        /// <value>
        /// The name of the configuration file.
        /// </value>
        string ConfigFileName { get; }
    }
}
