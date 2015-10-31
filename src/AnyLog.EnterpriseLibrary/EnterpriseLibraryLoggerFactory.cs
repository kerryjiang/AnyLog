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
    [Export(typeof(ILoggerFactory))]
    [LoggerFactoryMetadata("EnterpriseLibraryLogging", ConfigFileName = "logging.config", Priority = 20)]
    public class EnterpriseLibraryLoggerFactory : AdvanceLoggerFactory
    {
        private LogWriter m_LogWriter;

        /// <summary>
        /// Initializes the specified configuration files, we only use the first found one
        /// </summary>
        /// <param name="configFiles">All the potential configuration files, order by priority.</param>
        /// <returns></returns>
        public override bool Initialize(string[] configFiles)
        {
            if (!base.Initialize(configFiles))
                return false;

            var configurationSource = new FileConfigurationSource(this.ConfigFile);

            var factory = new LogWriterFactory(configurationSource);
            m_LogWriter = factory.Create();

            return true;
        }

        public override ILog GetLogger(string name)
        {
            return new EnterpriseLibraryLogger(m_LogWriter, name);
        }

        protected override ILoggerInventory CreateLoggerInventory()
        {
            return new LoggerInventory<LogWriter>(
                (name) => GetRepositoryConfigFile(name),
                (name, file) =>
                {
                    var factory = new LogWriterFactory(new FileConfigurationSource(file));
                    return factory.Create();
                },
                (resp, name) => new EnterpriseLibraryLogger(resp, name));
        }
    }
}
