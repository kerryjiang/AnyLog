using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace AnyLog.NLog
{
    [Export(typeof(ILogFactory))]
    [ExportMetadata("Name", "NLog")]
    [ExportMetadata("ConfigFileName", c_ConfigFileName)]
    public class NLogLogFactory : LogFactoryBase
    {
        private const string c_ConfigFileName = "nlog.config";

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

        }

        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ILog GetLog(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the log from repository.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="name">The name of the log.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override ILog CreateLogFromRepository(string repositoryName, string name)
        {
            throw new NotImplementedException();
        }
    }
}
