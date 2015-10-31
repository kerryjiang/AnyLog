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
        /// Gets the config file file path.
        /// </summary>
        protected string ConfigFile { get; private set; }

        /// <summary>
        /// Initializes the specified configuration files, we only use the first found one
        /// </summary>
        /// <param name="configFiles">All the potential configuration files, order by priority.</param>
        /// <returns></returns>
        public virtual bool Initialize(string[] configFiles)
        {
            foreach (var file in configFiles)
            {
                var filePath = file;

                if (!Path.IsPathRooted(filePath))
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                }

                if (File.Exists(filePath))
                {
                    ConfigFile = filePath;
                    m_LoggerInventory = CreateLoggerInventory();
                    return true;
                }
            }

            return false;
        }

        protected virtual string GetRepositoryConfigFile(string repositoryName)
        {
            var configFile = ConfigFile;
            var directory = Path.GetDirectoryName(configFile);
            var name = Path.GetFileNameWithoutExtension(configFile);
            var extension = Path.GetExtension(configFile);

            configFile = string.Format("{0}.{1}{2}", name, repositoryName, extension);
            return Path.Combine(directory, configFile);
        }

        /// <summary>
        /// Gets the logger by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public abstract ILog GetLogger(string name);

        protected abstract ILoggerInventory CreateLoggerInventory();

        private ILoggerInventory m_LoggerInventory;

        private ConcurrentDictionary<string, ILog> m_LoggersDict = new ConcurrentDictionary<string, ILog>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the loggers dictionary, the key composite with repository name and log name.
        /// </summary>
        /// <value>
        /// The loggers dictionary.
        /// </value>
        protected ConcurrentDictionary<string, ILog> LoggersDict
        {
            get { return m_LoggersDict; }
        }

        /// <summary>
        /// Gets the log from the specific repository.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual ILog GetLogger(string repositoryName, string name)
        {
            var logKey = repositoryName + "-" + name;

            ILog log;

            var loggerDict = LoggersDict;

            if (loggerDict.TryGetValue(logKey, out log))
                return log;

            log = m_LoggerInventory.CreateLoggerFromRepository(repositoryName, name);

            if (log == null)
                return null;

            while (true)
            {
                if (loggerDict.TryAdd(logKey, log))
                    return log;

                if (loggerDict.TryGetValue(logKey, out log))
                    return log;
            }
        }

        public ILog GetCurrentClassLogger()
        {
            return GetLogger(GetClassName(false));
        }

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
