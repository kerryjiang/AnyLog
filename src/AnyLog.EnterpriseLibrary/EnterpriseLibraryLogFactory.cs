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
    [ExportMetadata("ConfigFileName", "logging.config")]
    public class EnterpriseLibraryLogFactory : LogFactoryBase
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

        public override ILog GetLog(string name)
        {
            return new EnterpriseLibraryLog(m_LogWriter, name);
        }

        protected override ILogInventory CreateLogInventory()
        {
            return new LogInventory<LogWriter>(
                (name) => GetRepositoryConfigFile(name),
                (name, file) =>
                {
                    var factory = new LogWriterFactory(new FileConfigurationSource(file));
                    return factory.Create();
                },
                (resp, name) => new EnterpriseLibraryLog(resp, name));
        }
    }
}
