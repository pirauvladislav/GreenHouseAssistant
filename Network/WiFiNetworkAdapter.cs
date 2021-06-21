using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;

namespace GreenHouseAssistant.Network
{
    public class WiFiNetworkAdapter
    {
        public delegate void Connected(WiFiConnectedEventArgs eventArgs);
        public delegate void ConnectingError(WiFiConnectionErrorEventArgs eventArgs);

        public event Connected OnConnected;
        public event ConnectingError OnConnectingError;

        //public event EventHandler<WebServerConnectedEventArgs> OnConnected;
        //public event EventHandler<WebServerConnectionErrorEventArgs> OnConnectingError;

        public void InitWithDHCP(string wifiSsid, string wifiPassword,  int connectionTimeOutMs = 120000)
        {
            Init(wifiSsid, wifiPassword, null, connectionTimeOutMs);
        }

        public void InitWithFixIpAddress(string wifiSsid, string wifiPassword, IPConfiguration ipConfiguration, int connectionTimeOutMs = 120000)
        {
            Init(wifiSsid, wifiPassword, ipConfiguration, connectionTimeOutMs);
        }

        private void Init(string wifiSsid, string wifiPassword, IPConfiguration ipConfiguration, int connectionTimeOutMs)
        {
            CancellationTokenSource cs = new(connectionTimeOutMs);
            bool isWifiConnected;

            if (ipConfiguration is null)
            {
                 isWifiConnected = NetworkHelper.ConnectWifiDhcp(wifiSsid, wifiPassword,
                                                        setDateTime: true, token: cs.Token);
            }
            else
            {
                isWifiConnected = NetworkHelper.ConnectWifiFixAddress(wifiSsid, wifiPassword, ipConfiguration, 
                                                        setDateTime: true, token: cs.Token);
            }
            if (!isWifiConnected)
            {
                if (NetworkHelper.ConnectionError.Exception != null
                    || !string.IsNullOrEmpty(NetworkHelper.ConnectionError.Error))
                {
                    if (OnConnectingError != null)
                    {
                        var onConnectingError = OnConnectingError;
                        var errorEventArgs = new WiFiConnectionErrorEventArgs(
                                            NetworkHelper.ConnectionError.Error,
                                            NetworkHelper.ConnectionError.Exception);
                        onConnectingError.Invoke(errorEventArgs);
                    }

                    Debug.WriteLine($"Eror: {NetworkHelper.ConnectionError.Error} \n Exception: {NetworkHelper.ConnectionError.Exception}");
                }
                return;
            }

            var onConnected = OnConnected;
            var connectedEventArgs = new WiFiConnectedEventArgs(GetCurrentIpAddress());
            onConnected?.Invoke(connectedEventArgs);
        }

        public static string GetCurrentIpAddress()
        {
            return IPAddress.GetDefaultLocalAddress().ToString();
        }
    }
}
