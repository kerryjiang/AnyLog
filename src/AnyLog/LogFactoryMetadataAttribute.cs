using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace AnyLog
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LogFactoryMetadataAttribute : ExportAttribute, ILogFactoryMetadata
    {
        public string Name { get; private set; }

        public string ConfigFileName { get; set; }

        public int Priority { get; set; }

        public LogFactoryMetadataAttribute(string name)
            : base()
        {
            Name = name;
        }
    }
}
