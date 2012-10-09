using System;
using System.Configuration;

namespace Graviton.LoggerConfiguration
{
    [ConfigurationCollection(typeof(LoggerConfigurationElement), AddItemName = "logger", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class LoggerConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LoggerConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return ((LoggerConfigurationElement)element).LoggerName;
        }
    }
}