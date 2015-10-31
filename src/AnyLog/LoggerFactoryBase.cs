using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace AnyLog
{
    /// <summary>
    /// LogFactory Base class
    /// </summary>
    public abstract class LoggerFactoryBase : ILoggerFactory
    {
        /// <summary>
        /// Initializes with the specified configuration files, we only use the first found one
        /// </summary>
        /// <param name="configFiles">All the potential configuration files, order by priority.</param>
        /// <returns></returns>
        public abstract bool Initialize(string[] configFiles);

        /// <summary>
        /// Gets the logger by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public abstract ILog GetLogger(string name);


        /// <summary>
        /// Gets the logger from the specific repository.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public abstract ILog GetLogger(string repositoryName, string name);

        /// <summary>
        /// Gets the current class logger.
        /// </summary>
        /// <returns></returns>
        public ILog GetCurrentClassLogger()
        {
            return GetLogger(GetClassName(false));
        }

        /// <summary>
        /// Gets the current class logger.
        /// </summary>
        /// <param name="shortName">if set to <c>true</c> [short name].</param>
        /// <returns></returns>
        public ILog GetCurrentClassLogger(bool shortName)
        {
            return GetLogger(GetClassName(shortName));
        }

        private static string GetClassName(bool shortName)
        {
            int framesToSkip = 2;

            var frame = new StackFrame(framesToSkip, false);

            var method = frame.GetMethod();
            var declaringType = method.DeclaringType;

            if (declaringType == null)
                return method.Name;

            return shortName ? declaringType.Name : declaringType.FullName;
        }
    }
}
