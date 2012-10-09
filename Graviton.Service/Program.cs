using Topshelf;
using log4net;

namespace Graviton.Service
{
    internal class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        
        private static void Main(string[] args)
        {
            HostFactory.Run(x =>
                {
                    x.Service<SysLog>(s =>
                        {
                            s.ConstructUsing(name => new SysLog());
                            s.WhenStarted(tc => tc.Start());
                            s.WhenStopped(tc => tc.Stop());
                        });
                    x.RunAsLocalSystem();
                    x.SetDescription("Graviton SysLog for collecting remote and local SysLog events ");
                    x.SetDisplayName("Graviton SysLog");
                    x.SetServiceName("Graviton");

                    x.UseLog4Net("log4net.config");
                });
        }
    }
}