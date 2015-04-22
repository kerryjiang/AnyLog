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
