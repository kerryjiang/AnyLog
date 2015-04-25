using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using NLog;
using NLog.Config;

namespace AnyLog.NLog
{
    [Export(typeof(ILogFactory))]
    [ExportMetadata("Name", "NLog")]
    [ExportMetadata("ConfigFileName", "nlog.config")]
    public class NLogLogFactory : LogFactoryBase
    {
        private XmlLoggingConfiguration m_DefaultConfig;

        private LogFactory m_DefaultLogFactory;

        /// <summary>
        /// Initializes the specified configuration files, we only use the first found one
        /// </summary>
        /// <param name="configFiles">All the potential configuration files, order by priority.</param>
        /// <returns></returns>
        public override bool Initialize(string[] configFiles)
        {
            if (!base.Initialize(configFiles))
                return false;

            m_DefaultConfig = new XmlLoggingConfiguration(ConfigFile) { AutoReload = true };
            m_DefaultLogFactory = new LogFactory(m_DefaultConfig);

            return true;
        }

        protected override ILogInventory CreateLogInventory()
        {
            return new LogInventory<LogFactory>(
                (name) => GetRepositoryConfigFile(name),
                (name, file) => new LogFactory(new XmlLoggingConfiguration(file) { AutoReload = true }),
                (resp, name) => new NLogLog(resp.GetLogger(name)));
        }

        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ILog GetLog(string name)
        {
            return new NLogLog(m_DefaultLogFactory.GetLogger(name));
        }
    }
}
