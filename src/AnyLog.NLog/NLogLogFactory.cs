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
    [ExportMetadata("ConfigFileName", c_ConfigFileName)]
    public class NLogLogFactory : LogFactoryBase
    {
        private const string c_ConfigFileName = "nlog.config";

        private XmlLoggingConfiguration m_DefaultConfig;

        private LogFactory m_DefaultLogFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLogFactory"/> class.
        /// </summary>
        public NLogLogFactory()
            : this(new string[] { c_ConfigFileName })
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLogFactory"/> class.
        /// </summary>
        /// <param name="configFiles">All the potential configuration files, order by priority.</param>
        public NLogLogFactory(string[] configFiles)
            : base(configFiles)
        {
            m_DefaultConfig = new XmlLoggingConfiguration(ConfigFile) { AutoReload = true };
            m_DefaultLogFactory = new LogFactory(m_DefaultConfig);
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
