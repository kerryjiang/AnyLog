using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace AnyLog
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LoggerFactoryMetadataAttribute : ExportAttribute, ILoggerFactoryMetadata
    {
        public string Name { get; private set; }

        public string ConfigFileName { get; set; }

        public int Priority { get; set; }

        public LoggerFactoryMetadataAttribute(string name)
            : base()
        {
            Name = name;
        }
    }
}
