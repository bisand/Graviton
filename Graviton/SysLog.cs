using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Graviton.LoggerConfiguration;
using log4net;

namespace Graviton
{
    public class SysLog
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (SysLog));

        private bool _stopping;
        private UdpClient _udpClient;
        private FileSystemWatcher _watcher;

        public SysLog()
        {
            GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;
            ReadConfiguration();
            StartFileWatcher();
        }

        private void StartFileWatcher()
        {
            try
            {
                _watcher = new FileSystemWatcher();
                _watcher.Path = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                _watcher.Filter = Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                _watcher.NotifyFilter = NotifyFilters.LastWrite;
                _watcher.Changed += FileChanged;
                _watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                Logger.Error("An error occured while creating the file watcher", ex);
            }
        }

        private void FileChanged(object sender, FileSystemEventArgs args)
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Changed -= FileChanged;
                _watcher.Dispose();
                _watcher = null;
            }

            Logger.Info("Detected changes in configuration file. Reloading loggers...");
            ReadConfiguration();

            StartFileWatcher();
        }

        private static void ReadConfiguration()
        {
            try
            {
                ConfigurationManager.RefreshSection("loggerConfigurationSection");
                var configurationSection = (LoggerConfigurationSection) ConfigurationManager.GetSection("loggerConfigurationSection");
                LogConfigurator.Initialize(configurationSection);
            }
            catch (Exception ex)
            {
                Logger.Error("An error occured while trying to read the loggerConfigurationSection.", ex);
            }
        }

        public void Start()
        {
            Logger.Info("Starting Graviton SysLog...");
            var stateObject = new UdpState();
            _stopping = false;
            stateObject.WorkingSocket.BeginReceive(ReceiveCallback, stateObject);
            _udpClient = stateObject.WorkingSocket;
            Logger.Info("Graviton SysLog started.");
        }

        public void Stop()
        {
            Logger.Info("Stopping Graviton SysLog...");
            _stopping = true;
            _udpClient.Close();
            Logger.Info("Graviton SysLog stopped.");
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (ar == null || !(ar.AsyncState is UdpState))
            {
                if (!_stopping)
                {
                    Logger.Warn("Encountered an unexpected AsyncResult. Restarting Graviton SysLog...");

                    Stop();
                    Start();
                }
                return;
            }

            var stateObject = (ar.AsyncState as UdpState);
            try
            {
                var ipEndPoint = stateObject.EndPoint;
                var data = stateObject.WorkingSocket.EndReceive(ar, ref ipEndPoint);

                var stringData = Encoding.ASCII.GetString(data, 0, data.Length);
                var action = new Action<string>(s =>
                    {
                        try
                        {
                            var index = s.IndexOf(':');
                            if (index <= -1)
                                return;

                            // Internal data could be used internally, but not now...
                            var internalData = s.Substring(0, index);

                            var loggData = s.Substring(index + 1).TrimStart();
                            var loggsplit = loggData.Split('|');
                            if (loggsplit.Length <= 1)
                                return;

                            var loggerName = loggsplit[0].Trim();
                            ILog logger;
                            if ((logger = LogManager.Exists(loggerName)) != null)
                            {
                                logger.Info(loggData);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("An error occured while logging events.", ex);
                        }
                    });

                action.BeginInvoke(stringData, null, null);
            }
            catch (Exception ex)
            {
                Logger.Error("An error occured while receiving logging events. Restarting Graviton SysLog...", ex);

                if (_stopping)
                    return;

                Stop();
                Start();
                return;
            }
            try
            {
                stateObject.WorkingSocket.BeginReceive(ReceiveCallback, stateObject);
            }
            catch (Exception ex)
            {
                if (_stopping)
                    return;

                Logger.Error("An error occured while receiving logging events", ex);

                Stop();
                Start();
            }
        }
    }
}