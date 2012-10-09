using System.Net;
using System.Net.Sockets;

namespace Graviton
{
    public class UdpState
    {
        public UdpState()
        {
            EndPoint = new IPEndPoint(IPAddress.Any, AppSettings.ListenPort); //514
            WorkingSocket = new UdpClient(EndPoint);
        }

        public IPEndPoint EndPoint { get; set; }
        public UdpClient WorkingSocket { get; set; }
    }
}