using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;

namespace AnyLog.Log4Net
{
    /// <summary>
    /// Log4NetLogFactory
    /// </summary>
    [Export(typeof(ILogFactory))]
    [ExportMetadata("Name", "Log4Net")]
    [ExportMetadata("ConfigFileName", c_ConfigFileName)]
    public class Log4NetLogFactory : LogFactoryBase
    {
        private const string c_ConfigFileName = "log4net.config";
        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogFactory"/> class.
        /// </summary>
        public Log4NetLogFactory()
            : this(new string[] { c_ConfigFileName })
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogFactory"/> class.
        /// </summary>
        /// <param name="configFiles">All the potential configuration files, order by priority.</param>
        public Log4NetLogFactory(string[] configFiles)
            : base(configFiles)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(ConfigFile));
        }

        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override ILog GetLog(string name)
        {
            return new Log4NetLog(LogManager.GetLogger(name));
        }

        private ConcurrentDictionary<string, Lazy<ILoggerRepository>> m_LoggerRepositories = new ConcurrentDictionary<string, Lazy<ILoggerRepository>>();

        private Lazy<ILoggerRepository> CreateLazyRepository(string repositoryName)
        {
            var configFilePath = GetRepositoryConfigFile(repositoryName);

            if (!File.Exists(configFilePath))
                return null;

            return new Lazy<ILoggerRepository>(() =>
            {
                var repository = LogManager.CreateRepository(repositoryName);
                log4net.Config.XmlConfigurator.ConfigureAndWatch(repository, new FileInfo(configFilePath));
                return repository;
            });
        }

        private ILoggerRepository EnsureRepository(string repositoryName)
        {
            Lazy<ILoggerRepository> repository;

            if (m_LoggerRepositories.TryGetValue(repositoryName, out repository))
                return repository.Value;

            repository = CreateLazyRepository(repositoryName);

            if (repository == null)
                return null;

            if (m_LoggerRepositories.TryAdd(repositoryName, repository))
                return repository.Value;

            return EnsureRepository(repositoryName);
        }

        protected override ILog CreateLogFromRepository(string repositoryName, string name)
        {
            // try to create log
            // get log respostory at first
            ILoggerRepository repository = EnsureRepository(repositoryName);

            // repository is not found
            if (repository == null)
                return null;

            var innerLog = LogManager.GetLogger(repositoryName, name);

            return new Log4NetLog(innerLog);
        }
    }
}
