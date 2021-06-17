using nanoFramework.Networking;
using System;
using System.Net;
using System.Diagnostics;
using System.Threading;
using nanoFramework.WebServer;
using GreenHouseAssistant.Gpio;

namespace GreenHouseAssistant.ApiServer
{
    public class WebApiServer
    {
        private readonly string _wifiSsid;
        private readonly string _wifiPassword;

        public delegate void Connected(WebServerConnectedEventArgs eventArgs);
        public delegate void ConnectingError(WebServerConnectionErrorEventArgs eventArgs);

        public event Connected OnConnected;
        public event ConnectingError OnConnectingError;

        //public event EventHandler<WebServerConnectedEventArgs> OnConnected;
        //public event EventHandler<WebServerConnectionErrorEventArgs> OnConnectingError;

        public WebApiServer(string wifiSsid, string wifiPassword)
        {
            _wifiSsid = wifiSsid;
            _wifiPassword = wifiPassword;
        }

        public void StartWebServer()
        {
            CancellationTokenSource cs = new(120000);
            var isWifiConnected = NetworkHelper.ConnectWifiDhcp(_wifiSsid, _wifiPassword,
                                                    setDateTime: true, token: cs.Token);

            if (!isWifiConnected)
            {
                if (NetworkHelper.ConnectionError.Exception != null 
                    || !string.IsNullOrEmpty(NetworkHelper.ConnectionError.Error))
                {
                    if (OnConnectingError != null)
                    {
                        var onConnectingError = OnConnectingError;
                        var errorEventArgs = new WebServerConnectionErrorEventArgs(
                                            NetworkHelper.ConnectionError.Error,
                                            NetworkHelper.ConnectionError.Exception);
                        onConnectingError.Invoke(errorEventArgs);
                    }

                    Debug.WriteLine($"Eror: {NetworkHelper.ConnectionError.Error} \n Exception: {NetworkHelper.ConnectionError.Exception}");
                }
                return;
            }

            using (WebServer server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(ApiController) }))
            {        
                server.Start();
                Debug.WriteLine($"Server started on IP Address: {GetCurrentIpAddress()}");

                var onConnected = OnConnected;
                var connectedEventArgs = new WebServerConnectedEventArgs(GetCurrentIpAddress());
                onConnected?.Invoke(connectedEventArgs);

                Thread.Sleep(Timeout.Infinite);
            }
        }

        public string GetCurrentIpAddress()
        {
            return IPAddress.GetDefaultLocalAddress().ToString();
        }
    }
}
