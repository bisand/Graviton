using System.Configuration;

namespace Graviton
{
    public static class AppSettings
    {
        public static int ListenPort
        {
            get
            {
                var tmpValue = ConfigurationManager.AppSettings["ListenPort"] ?? "514";
                int result;
                return int.TryParse(tmpValue, out result) ? result : 514;
            }
        }
    }
}