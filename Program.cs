using GreenHouseAssistant.Api;
using GreenHouseAssistant.Gpio;
using GreenHouseAssistant.Network;
using System;
using System.Diagnostics;
using System.Threading;

namespace GreenHouseAssistant
{
    public class Program
    {
        private static OnBoardLed _boardLed;

        public static void Main()
        {
            _boardLed = OnBoardLed.GetOnBoardLed();

            WiFiNetworkAdapter adapter = new WiFiNetworkAdapter();
            adapter.OnConnected += WiFiAdapter_OnConnected;
            adapter.OnConnectingError += WiFiAdapter_OnConnectingError;

            _boardLed.StartBlinking();
            adapter.InitWithDHCP("WIFI", "password");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void WiFiAdapter_OnConnectingError(WiFiConnectionErrorEventArgs e)
        {
            Debug.WriteLine($"Wifi connection error: {e.ConnectionError} \n Exception: {e.Exception}");
            _boardLed.TurnOn();
        }

        private static void WiFiAdapter_OnConnected(WiFiConnectedEventArgs e)
        {
            Debug.WriteLine($"Wifi connected with IP Address: {e.IpAddress}");
            StartWebServer();
        }

        private static void StartWebServer()
        {
            _boardLed.StartBlinkingFast();

            var isWebServerStarted = WebApiServer.StartWebServer();
            if (isWebServerStarted)
            {
                _boardLed.TurnOff();
                return;
            }
            _boardLed.TurnOn();
        }
    }
}
