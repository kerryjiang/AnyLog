using AnyLog;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace AnyLog.EnterpriseLibrary
{
    [Export(typeof(ILogFactory))]
    [ExportMetadata("Name", "EnterpriseLibraryLogging")]
    [ExportMetadata("ConfigFileName", c_ConfigFileName)]
    public class EnterpriseLibraryLogFactory : LogFactoryBase
    {
        private LogWriter m_LogWriter;

        private const string c_ConfigFileName = "logging.config";

        public EnterpriseLibraryLogFactory()
            : this(new string[] { c_ConfigFileName })
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseLibraryLogFactory"/> class.
        /// </summary>
        /// <param name="configFiles">All the potential configuration files, order by priority.</param>
        public EnterpriseLibraryLogFactory(string[] configFiles) :
            base(configFiles)
        {
            var configurationSource = new FileConfigurationSource(this.ConfigFile);

            var factory = new LogWriterFactory(configurationSource);
            m_LogWriter = factory.Create();
        }

        public override ILog GetLog(string name)
        {
            return new EnterpriseLibraryLog(m_LogWriter, name);
        }

        private ConcurrentDictionary<string, Lazy<LogWriter>> m_LoggerRepositories = new ConcurrentDictionary<string, Lazy<LogWriter>>();

        private Lazy<LogWriter> CreateLazyRepository(string repositoryName)
        {
            var configFilePath = GetRepositoryConfigFile(repositoryName);

            if (!File.Exists(configFilePath))
                return null;

            return new Lazy<LogWriter>(() =>
            {
                var factory = new LogWriterFactory(new FileConfigurationSource(configFilePath));
                return factory.Create();
            });
        }

        private LogWriter EnsureRepository(string repositoryName)
        {
            Lazy<LogWriter> repository;

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
            LogWriter writer = EnsureRepository(repositoryName);

            // repository is not found
            if (writer == null)
                return null;

            return new EnterpriseLibraryLog(writer, name);
        }
    }
}
