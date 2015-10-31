using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using NLogRef = NLog;
using NLog.Config;

namespace AnyLog.NLog
{
    [Export(typeof(ILoggerFactory))]
    [LoggerFactoryMetadata("NLog", ConfigFileName = "nlog.config", Priority = 10)]
    public class NLogLoggerFactory : LoggerFactoryBase
    {
        private XmlLoggingConfiguration m_DefaultConfig;

        private NLogRef.LogFactory m_DefaultLogFactory;

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
            m_DefaultLogFactory = new NLogRef.LogFactory(m_DefaultConfig);

            return true;
        }

        protected override ILoggerInventory CreateLoggerInventory()
        {
            return new LoggerInventory<NLogRef.LogFactory>(
                (name) => GetRepositoryConfigFile(name),
                (name, file) => new NLogRef.LogFactory(new XmlLoggingConfiguration(file) { AutoReload = true }),
                (resp, name) => new NLogLogger(resp.GetLogger(name)));
        }

        /// <summary>
        /// Gets the logger by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ILog GetLogger(string name)
        {
            return new NLogLogger(m_DefaultLogFactory.GetLogger(name));
        }
    }
}
