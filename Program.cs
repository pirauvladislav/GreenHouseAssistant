using GreenHouseAssistant.ApiServer;
using GreenHouseAssistant.Gpio;
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

            Debug.WriteLine("Startting WebServer...");
            StartWebServer();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void StartWebServer()
        {
            _boardLed.StartBlinking();

             var webserver = new WebApiServer("PA-WIFI", "!panfilii@#");
             webserver.OnConnected += Webserver_OnConnected;
             webserver.OnConnectingError += Webserver_OnConnectingError;
             webserver.StartWebServer();
        }

        private static void Webserver_OnConnectingError(WebServerConnectionErrorEventArgs e)
        {
            Debug.WriteLine($"Error: {e.ConnectionError} \n Exception: {e.Exception}");
            _boardLed.TurnOn();
        }

        private static void Webserver_OnConnected(WebServerConnectedEventArgs e)
        {
            Debug.WriteLine($"Server started on IP Address: {e.IpAddress}");
            _boardLed.TurnOff();
        }
    }
}
