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
            //dotnet tool update -g nanoff
            //nanoff --update --platform esp32 --serialport COM3 --preview

            ConnectToWiFiRouter();

            Thread.Sleep(Timeout.Infinite);
        }
        private static void ConnectToWiFiRouter()
        {
            _boardLed = OnBoardLed.GetOnBoardLed();

            WiFiNetworkAdapter adapter = new WiFiNetworkAdapter();
            adapter.OnConnected += WiFiAdapter_OnConnected;
            adapter.OnConnectingError += WiFiAdapter_OnConnectingError;

            _boardLed.StartBlinking();
            adapter.InitWithDHCP("PA-WIFI", "!panfilii@#");
            //adapter.InitWithDHCP("wifi-ssid", "password");
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

    }
}
