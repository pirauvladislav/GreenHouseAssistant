using System;
using System.Text;

namespace GreenHouseAssistant.Network
{
    public class WiFiConnectedEventArgs : EventArgs
    {
        public WiFiConnectedEventArgs(string ipAddress)
        {
            IpAddress = ipAddress;
        }
        public string IpAddress { get; set; }
    }
}
