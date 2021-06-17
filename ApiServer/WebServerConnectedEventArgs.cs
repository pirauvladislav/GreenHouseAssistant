using System;
using System.Text;

namespace GreenHouseAssistant.ApiServer
{
    public class WebServerConnectedEventArgs : EventArgs
    {
        public WebServerConnectedEventArgs(string ipAddress)
        {
            IpAddress = ipAddress;
        }
        public string IpAddress { get; set; }
    }
}
