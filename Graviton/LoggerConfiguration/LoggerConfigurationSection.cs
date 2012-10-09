using System.Configuration;

namespace Graviton.LoggerConfiguration
{
    public class LoggerConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("LoggerConfigurations", Options = ConfigurationPropertyOptions.IsRequired)]
        public LoggerConfigurationCollection LoggerConfigurations
        {
            get
            {
                return (LoggerConfigurationCollection)this["LoggerConfigurations"];
            }
        }
    }
}