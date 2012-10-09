using System.Configuration;

namespace Graviton.LoggerConfiguration
{
    public class LoggerConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("loggerName", DefaultValue = "", IsRequired = true)]
        public string LoggerName
        {
            get
            {
                return (string)this["loggerName"];
            }
            set
            {
                this["loggerName"] = value;
            }
        }

        [ConfigurationProperty("filename", DefaultValue = "", IsRequired = true)]
        public string Filename
        {
            get
            {
                return (string)this["filename"];
            }
            set
            {
                this["filename"] = value;
            }
        }

        [ConfigurationProperty("maximumFileSize", DefaultValue = "1MB", IsRequired = false)]
        public string MaximumFileSize
        {
            get
            {
                return (string)this["maximumFileSize"];
            }
            set
            {
                this["maximumFileSize"] = value;
            }
        }

        [ConfigurationProperty("maxSizeRollBackups", DefaultValue = 10, IsRequired = false)]
        public int MaxSizeRollBackups
        {
            get
            {
                return (int)this["maxSizeRollBackups"];
            }
            set
            {
                this["maxSizeRollBackups"] = value;
            }
        }

        [ConfigurationProperty("preserveLogFileNameExtension", DefaultValue = false, IsRequired = false)]
        public bool PreserveLogFileNameExtension
        {
            get
            {
                return (bool)this["preserveLogFileNameExtension"];
            }
            set
            {
                this["preserveLogFileNameExtension"] = value;
            }
        }

        [ConfigurationProperty("datePattern", DefaultValue = "yyyy-MM-dd", IsRequired = false)]
        public string DatePattern
        {
            get
            {
                return (string)this["datePattern"];
            }
            set
            {
                this["datePattern"] = value;
            }
        }

        [ConfigurationProperty("logLevel", DefaultValue = "ALL", IsRequired = false)]
        public string LogLevel
        {
            get
            {
                return (string)this["logLevel"];
            }
            set
            {
                this["logLevel"] = value;
            }
        }
    }
}