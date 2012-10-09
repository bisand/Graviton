using System.Collections.Generic;
using Graviton.LoggerConfiguration;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Graviton
{
    public static class LogConfigurator
    {
        public static Dictionary<string, ILog> Initialize(LoggerConfigurationSection loggerConfiguration)
        {
            var result = new Dictionary<string, ILog>();
            foreach (var config in loggerConfiguration.LoggerConfigurations)
            {
                if (config is LoggerConfigurationElement)
                {
                    var element = (config as LoggerConfigurationElement);
                    SetLevel(element.LoggerName, element.LogLevel);
                    AddAppender(element.LoggerName, CreateFileAppender(element));
                    result.Add(element.LoggerName, LogManager.GetLogger(element.LoggerName));
                }
            }
            return result;
        }

        public static void SetLevel(string loggerName, string levelName)
        {
            var log = LogManager.GetLogger(loggerName);
            var l = (Logger) log.Logger;

            l.CloseNestedAppenders();
            l.Level = l.Hierarchy.LevelMap[levelName];
        }

        // Add an appender to a logger
        public static void AddAppender(string loggerName, RollingFileAppender appender)
        {
            var log = LogManager.GetLogger(loggerName);
            var l = (Logger) log.Logger;

            l.CloseNestedAppenders();
            l.RemoveAllAppenders();
            l.AddAppender(appender);
            appender.ActivateOptions();
        }

        // Create a new file appender
        public static RollingFileAppender CreateFileAppender(LoggerConfigurationElement element)
        {
            var appender = new RollingFileAppender();
            appender.Name = element.LoggerName;
            appender.File = element.Filename;
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            appender.MaximumFileSize = element.MaximumFileSize;
            appender.MaxSizeRollBackups = element.MaxSizeRollBackups;
            appender.PreserveLogFileNameExtension = element.PreserveLogFileNameExtension;
            appender.DatePattern = element.DatePattern;

            var layout = new PatternLayout();
            layout.ConversionPattern = "%message";
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }
    }
}