using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyLog
{
    /// <summary>
    /// LogFactory Interface
    /// </summary>
    public interface ILogFactory
    {
        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        ILog GetLog(string name);
    }
}
